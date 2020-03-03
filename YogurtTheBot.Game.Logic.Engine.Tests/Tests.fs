module Tests

open Xunit
open YogurtTheBot.Game.Logic.Engine

[<Fact>]
let ``My test`` () =
    let level = LevelLoader.load "Levels/first.yaml"
    
    ()
