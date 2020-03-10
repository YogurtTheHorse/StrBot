module YogurtTheBot.Game.Logic.LevelController

open System

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Core.Controllers.Language.Parsing
open YogurtTheBot.Game.Logic.Engine.Levels

open YogurtTheBot.Game.Core.Localizations
open YogurtTheBot.Game.Logic.Engine
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


let buildReflex (localizer: ILocalizer) locale level reflex =
    let actorName = get reflex "actor" |> value
    let actionName = get reflex "action" |> value
    let recipientName = get reflex "recipient"

    let action =
        level
        |> Level.getActions
        |> Seq.find (fun a ->
            let localization = localizer.GetString("actions." + a.name + ".name", locale)

            localization.MatchesMessage actionName)

    let findActor name =
        level
        |> Level.getActors
        |> Seq.find (fun a ->
            let localization = localizer.GetString("actors." + a.name + ".name", locale)

            localization.MatchesMessage name)

    let actor = findActor actorName

    { actor = actor
      recipient =
          match recipientName with
          | Value "self" -> actor
          | Value v -> findActor v
          | _ -> actor
      action = action }

let formatRule (localizer: ILocalizer) locale rule =
    let translate s =
        (localizer.GetString(s, locale)).Value

    let translateActor (a: Actor) =
        translate ("actors." + a.name + ".name")

    let translateStimulus (s: Reflex) =
        (translateActor s.actor) + " " + translate ("actions." + s.action.name + ".present") + " "
        + (translateActor s.recipient)

    match rule with
    | Reflex(stimulus, reflex) ->
        (translate "screens.level.if") + " " + (translateStimulus stimulus) + " " + (translate "screens.level.then")
        + " " + (translateStimulus reflex)
    | Permission { actor = a; action = { name = actionName } } ->
        String.Format
            (translate "screens.level.permission", translateActor a, translate ("actions." + actionName + ".infinitive"))


[<Controller>]
type LevelController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer)

    let level = First.level

    [<LanguageAction("NewRuleRule")>]
    member x.NewRule(parsingesult: ParsingResult, info: PlayerInfo) =
        let node = (Seq.head parsingesult.Possibilities).Node
        let visit = visit node |> clearUnnammed

        let stimulus = buildReflex localizer info.Locale level (get visit "stimulus")
        let reflex = buildReflex localizer info.Locale level (get visit "reflex")

        x.Answer(stimulus.ToString() + "\n" + reflex.ToString())


    override x.DefaultHandler(message: IncomingMessage, info: PlayerInfo, data: PlayerData) = x.OnOpen(info, data)

    override x.OnOpen(info: PlayerInfo, data: PlayerData) =
        let actors =
            level.actors
            |> List.map (fun actor -> localizer.GetString("actors." + actor.name + ".name", info.Locale).Value)
            |> String.concat ", "

        let rules =
            level.rules
            |> List.map (formatRule localizer info.Locale)
            |> String.concat "\n"

        x.Answer ((localizer.GetString("screens.level.open", info.Locale)).Format(actors, rules)).Value

    member x.NewRuleRule = newRuleRule
