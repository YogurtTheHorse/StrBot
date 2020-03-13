module YogurtTheBot.Game.Logic.LevelController

open System

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Localizations
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Handlers

open YogurtTheBot.Game.Core.Controllers.Language.Controllers
open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Core.Controllers.Language.Parsing

open YogurtTheBot.Game.Logic.Engine
open YogurtTheBot.Game.Logic.Engine.Levels
open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.NodeVisitor

let englishAlphabet = "abcdefghijklmnopqrstuvwxyz"
let russianAlphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"

let word =
    englishAlphabet + russianAlphabet
    |> Seq.toArray
    |> Array.map string
    |> Array.map Terminal
    |> Array.map (fun t -> t :> Expression)
    |> OneOf
    |> Many

let spaces =
    " "
    |> Terminal
    |> Many :> Expression

let optional e = OneOf [| e; Expression.Empty |] :> Expression

let named name e = NonTerminal(name, e) :> Expression

let reflex =
    (word |> named "actor") + spaces + ((LocalizedTerminal "screens.level.will" + spaces) |> optional)
    + (word |> named "action") + OneOf
                                     [ spaces + (word |> named "recipient")
                                       LocalizedTerminal "screens.level.slf" ]

let newRuleRule =
    (LocalizedTerminal "screens.level.if") + spaces + (reflex |> named "stimulus")
    + optional (spaces + LocalizedTerminal "screens.level.then") + spaces + (reflex |> named "reflex") + Expression.End


let buildReflex (translate: string -> Localization) level reflex =
    let actorName = get reflex "actor" |> value
    let actionName = get reflex "action" |> value
    let recipientName = get reflex "recipient"

    let action =
        level
        |> Level.getActions
        |> Seq.find (fun a ->
            let localization = translate ("actions." + a.name + ".name")

            localization.MatchesMessage actionName)

    let findActor name =
        level
        |> Level.getActors
        |> Seq.find (fun a ->
            let localization = translate ("actors." + a.name + ".name")

            localization.MatchesMessage name)

    let actor = findActor actorName

    { actor = actor
      recipient =
          match recipientName with
          | Value "self" -> actor
          | Value v -> findActor v
          | _ -> actor
      action = action }

let formatRule (translate: string -> string) rule =
    let translateActor (a: Actor) =
        translate ("actors." + a.name + ".name")

    let translateStimulus (s: ActorAction) =
        (translateActor s.actor) + " " + translate ("actions." + s.action.name + ".present") + " "
        + (translateActor s.recipient)

    match rule with
    | Callback(stimulus, reflex) ->
        (translate "screens.level.if") + " " + (translateStimulus stimulus) + " " + (translate "screens.level.then")
        + " " + (translateStimulus reflex)
    | Permission { actor = a; action = { name = actionName } } ->
        String.Format
            (translate "screens.level.permission", translateActor a, translate ("actions." + actionName + ".infinitive"))


[<Controller>]
type LevelController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer)

    let level = First.level
    
    [<Action("common.back")>]
    member x.GoBack(info: PlayerInfo, data: PlayerData) = x.Back(info, data)

    [<LanguageAction("NewRuleRule")>]
    member x.NewRule(parsingesult: ParsingResult, info: PlayerInfo, data: PlayerData) =
        let visit =
            (Seq.head parsingesult.Possibilities).Node
            |> visit
            |> clearUnnammed
            
        let translate s = localizer.GetString(s, info.Locale)

        let buildReflex name = buildReflex translate level (get visit name)
        let rule = Callback (buildReflex "stimulus", buildReflex "reflex")

        match PlayerData.addRule level data rule with
        | Added -> x.Answer (translate "screens.level.rule_added").Value
        | Error l -> x.Answer l.Value


    override x.DefaultHandler(message: IncomingMessage, info: PlayerInfo, data: PlayerData) = x.OnOpen(info, data)

    override x.OnOpen(info: PlayerInfo, data: PlayerData) =
        let actors =
            level.actors
            |> List.map (fun actor -> localizer.GetString("actors." + actor.name + ".name", info.Locale).Value)
            |> String.concat ", "

        let rules =
            level.rules
            |> List.map (formatRule (fun s -> localizer.GetString(s, info.Locale).Value))
            |> String.concat "\n"

        x.Answer ((localizer.GetString("screens.level.open", info.Locale)).Format(actors, rules)).Value

    member x.NewRuleRule = newRuleRule
