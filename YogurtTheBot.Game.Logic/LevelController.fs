module YogurtTheBot.Game.Logic.LevelController

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Core.Controllers.Language.Parsing
open YogurtTheBot.Game.Logic.Engine.Levels

open YogurtTheBot.Game.Core.Controllers.Language.Expressions
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
    + (word |> named "action") + OneOf [spaces + (word |> named "recipient"); LocalizedTerminal "screens.level.slf"]

let newRuleRule =
    (LocalizedTerminal "screens.level.if") + spaces + (reflex |> named "stimulus")
    + optional (spaces + LocalizedTerminal "screens.level.then") + spaces + (reflex |> named "reflex") + Expression.End


[<Controller>]
type LevelController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer)

    member private x.Level = First.level

    [<LanguageAction("NewRuleRule")>]
    member x.NewRule(parsingesult: ParsingResult, info: PlayerInfo) =
        let node = (Seq.head parsingesult.Possibilities).Node
        let visit = visit node |> clearUnnammed

        let buildReflex level reflex =
            let actorName = get reflex "actor" |> value
            let actionName = get reflex "action" |> value
            let recipientName = get reflex "recipient"

            let action =
                level
                |> Level.getActions
                |> Seq.find (fun a ->
                    let localization = localizer.GetString("actions." + a.name + ".name", info.Locale)

                    localization.MatchesMessage actionName)

            let findActor name =
                level
                |> Level.getActors
                |> Seq.find (fun a ->
                    let localization = localizer.GetString("actors." + a.name + ".name", info.Locale)

                    localization.MatchesMessage name)

            let actor = findActor actorName

            { actor = actor
              recipient =
                  match recipientName with
                  | Value "self" -> actor
                  | Value v -> findActor v
                  | None -> actor
              action = action }

        let stimulus = buildReflex x.Level (get visit "stimulus")
        let reflex = buildReflex x.Level (get visit "reflex")

        x.Answer(stimulus.ToString() + "\n" + reflex.ToString())


    member x.DefaultHandler(message: IncomingMessage, info: PlayerInfo, data: PlayerData) =
        x.Answer (localizer.GetString("screens.level.default", info.Locale)).Value

    member x.OnOpen(info: PlayerInfo, data: PlayerData) = x.Answer "on open"

    member x.NewRuleRule = newRuleRule
