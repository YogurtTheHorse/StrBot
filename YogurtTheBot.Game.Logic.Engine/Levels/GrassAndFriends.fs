module YogurtTheBot.Game.Logic.Engine.Levels.GrassAndFriends

open YogurtTheBot.Game.Logic.Engine.Default.Actors
open YogurtTheBot.Game.Logic.Engine.Default.Actions
open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.Engine

let wolvesEatRabbits = createAction wolves eat rabbits

let rabbitsEatGrass = createAction rabbits eat grass

let level =
    {
        name = "grass_and_friends"
        permissions = [
            grant everyone eat everyone
        ]
        callbacks = [
            { reason = createAction wolves eat grass 
              result = Failure "levels.third.herbivorous_wolves" }
            
            { reason = rabbitsEatGrass
              result = Action wolvesEatRabbits }
            { reason = createAction somebody eat grass
              result = Failure "levels.third.grass_eaten" }
            
            { reason = createAction grass eat grass
              result = Answer "levels.third.last_grass" }
            { reason = createAction grass eat grass
              result = Action wolvesEatRabbits }
            
            { reason = createAction wolves eat wolves
              result = Answer "levels.third.wolves_dead" }
            
            { reason = createAction rabbits eat rabbits
              result = Answer "levels.third.rabbits_dead" }
            
            { reason = createAction somebody eat rabbits
              result = Tag "rabbits" }
            { reason = createAction somebody eat rabbits
              result = restrict rabbits eat everyone |> Permission }
            
            { reason = createAction somebody eat wolves
              result = Tag "wolves" }
            { reason = createAction somebody eat wolves
              result = restrict wolves eat everyone |> Permission }
        ]
        winCondition = Level.allTags ["wolves"; "rabbits"] None
        actors = [wolves; grass; rabbits]
        actions = [eat]
        solution =
          [ createAction grass eat wolves
            createAction grass eat rabbits ]
    }


