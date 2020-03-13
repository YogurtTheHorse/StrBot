module Tests

open Xunit
open FsUnit
open YogurtTheBot.Game.Logic.Engine
open YogurtTheBot.Game.Logic.Engine.LevelsList
open YogurtTheBot.Game.Logic.Engine.Models

let values = levels |> List.map (fun x -> [|x|]) |> List.toArray

[<Theory; MemberData("values")>]
let ``Level has correct solution`` (l) =
    Runner.testSolution l l.solution
    |> (fun r -> r.status)
    |> should equal Runner.Complete
