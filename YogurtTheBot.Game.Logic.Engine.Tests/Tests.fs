module Tests

open Xunit
open FsUnit
open YogurtTheBot.Game.Logic.Engine.LevelsList
open YogurtTheBot.Game.Logic.Engine.Level
open YogurtTheBot.Game.Logic.Engine.Models

let values = levels |> List.map (fun x -> [|x|]) |> List.toArray

[<Theory; MemberData("values")>]
let ``Level has correct solution`` (l) =
    testSolution l l.solution
    |> snd
    |> should equal Complete
