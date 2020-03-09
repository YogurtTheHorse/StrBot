module YogurtTheBot.Game.Logic.LevelController

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Core.Controllers.Language.Parsing
open YogurtTheBot.Game.Logic.Engine.Levels

open YogurtTheBot.Game.Logic.NodeVisitor

let englishAlphabet = "abcdefghijklmnopqrstuvwxyz"
let russianAlphabet = "абвгдеёжзийклмнопстуфхцчшщъыьэюя"

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
    + (word |> named "action") + spaces + (word |> named "recipient")
    |> named "reflex"

let newRuleRule =
    (LocalizedTerminal "screens.level.if") + spaces + reflex
    + optional (spaces + LocalizedTerminal "screens.level.then") + spaces + reflex + Expression.End


[<Controller>]
type LevelController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer)

    member private x.Level = First.level

    [<LanguageAction("NewRuleRule")>]
    member x.NewRule(message: IncomingMessage, parsingesult: ParsingResult) =
        let p = Seq.head parsingesult.Possibilities
        let visit = visit p.Node |> clearUnnammed
        x.Answer p.Node.Value

    member x.DefaultHandler(message: IncomingMessage, info: PlayerInfo, data: PlayerData) =
        x.Answer (localizer.GetString("screens.level.default", info.Locale)).Value

    member x.OnOpen(info: PlayerInfo, data: PlayerData) = x.Answer "on open"

    member x.NewRuleRule = newRuleRule
