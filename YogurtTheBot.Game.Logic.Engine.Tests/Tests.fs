module Tests

open Xunit
open YogurtTheBot.Game.Logic.Engine.LevelsList
open YogurtTheBot.Game.Logic.Engine.Level

[<Fact>]
let ``My test`` () =
    levels
    |> List.map (fun l -> testSolution l l.solution)
    |> List.forall (fun r -> r = Complete)
    |> Assert.True 
