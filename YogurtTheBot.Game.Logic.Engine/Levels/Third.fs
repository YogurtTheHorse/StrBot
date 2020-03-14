module YogurtTheBot.Game.Logic.Engine.Levels.Third

open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions
open YogurtTheBot.Game.Logic.Engine.Models

let wolvesEatRabbits = createAction wolves eat rabbits

let rabbitsEatGrass = createAction rabbits eat grass

let level =
    {
        name = "third"
        permissions = [
            grant everyone eat everyone
        ]
        callbacks = [
            { reason = createAction wolves eat grass 
              result = Answer "levels.third.rabbits_starve" }
            { reason = createAction wolves eat grass 
              result = Failure "levels.third.herbivorous_wolves" }
            
            { reason = rabbitsEatGrass
              result = Action wolvesEatRabbits }
            { reason = rabbitsEatGrass
              result = Failure "levels.third.wolves_left" }
            
            { reason = createAction grass eat grass
              result = Answer "levels.third.last_grass" }
            { reason = createAction grass eat grass
              result = Action wolvesEatRabbits }
            
            { reason = createAction wolves eat wolves
              result = Answer "levels.third.last_wolf" }
            
            { reason = createAction rabbits eat rabbits
              result = Answer "levels.third.last_rabbit" }
            { reason = createAction rabbits eat rabbits
              result = Action wolvesEatRabbits }
            
            { reason = createAction rabbits eat wolves
              result = Answer "levels.third.carnivorous_rabbits" }
            { reason = createAction rabbits eat wolves
              result = restrict wolves eat everyone |> Permission }
            
            { reason = createAction grass eat wolves
              result = restrict wolves eat everyone |> Permission }
            
            { reason = createAction grass eat rabbits
              result = restrict rabbits eat everyone |> Permission }
            
            { reason = createAction somebody eat rabbits
              result = Tag "rabbits" }
            { reason = createAction somebody eat wolves
              result = Tag "wolves" }
        ]
        winCondition = ["wolves"; "rabbits"]
        actors = [wolves; grass; rabbits]
        actions = [eat]
        solution =
          [ createAction grass eat wolves
            createAction grass eat rabbits ]
    }


