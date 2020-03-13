module YogurtTheBot.Game.Logic.Language

open YogurtTheBot.Game.Core.Controllers.Language.Expressions

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

let actorAction =
    optional (LocalizedTerminal "common.let" + spaces)
    + named "actor" word
    + spaces
    + optional (LocalizedTerminal "common.will" + spaces)
    + named "action" word
    + OneOf [ spaces + (word |> named "recipient")
              LocalizedTerminal "common.slf" ]


