module YogurtTheBot.Game.Logic.LevelController

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Localizations
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Handlers

open YogurtTheBot.Game.Core.Controllers.Language.Controllers

open YogurtTheBot.Game.Core.Controllers.Answers
open YogurtTheBot.Game.Core.Controllers.Language.Expressions
open YogurtTheBot.Game.Core.Controllers.Language.Parsing
open YogurtTheBot.Game.Data
open YogurtTheBot.Game.Logic.Engine
open YogurtTheBot.Game.Logic.Engine.Levels
open YogurtTheBot.Game.Logic.Engine.Models
open YogurtTheBot.Game.Logic.NodeVisitor

type BuildCallbackResult =
    | ActorNotFound
    | ActionNotFound
    | RecipientNotFound
    | Success of ActorAction

let buildAction (translate: string -> Localization) level parsed =
    let actorName = get parsed "actor" |> value
    let actionName = get parsed "action" |> value
    let recipientName = get parsed "recipient"

    let action =
        level
        |> Level.getActions
        |> Seq.tryFind (fun a ->
            let localization = translate ("actions." + a.name + ".name")

            localization.MatchesMessage actionName)

    let findActor name (actors: Actor seq) =
        actors
        |> Seq.tryFind (fun a ->
            let localization = translate ("actors." + a.name + ".name")

            localization.MatchesMessage name)

    let actor = findActor actorName level.actors
    
    let recipient = 
      match recipientName with
      | Value "self" -> actor
      | Value v -> findActor v (Level.getActors level)
      | _ -> actor

    match actor, action, recipient with
    | (Option.None, _, _) -> ActorNotFound
    | (_, Option.None, _) -> ActionNotFound
    | (_, _, Option.None) -> RecipientNotFound
    | (Some actor, Some action, Some recipient) -> Success (createAction actor action recipient)

let formatAction (translate: string -> string) action: string =
    let translateActor (a: Actor) = translate ("actors." + a.name + ".name")

    (translateActor action.actor)
    + " "
    + translate ("actions." + action.action.name + ".present")
    + " "
    + (translateActor action.recipient)


[<Controller>]
type LevelController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer)

    let level = First.level
    
    [<Action("common.back")>]
    member x.GoBack(info: PlayerInfo, data: PlayerData) = x.Back(info, data)
    
    [<Action("screens.level.next")>]
    member x.NextLevel() =
        x.Answer "next"
    
    [<Action("screens.level.restart")>]
    member x.Restart() =
        x.Answer "next"
    
    member x.MakeActionRule = Language.actorAction + Expression.End

    [<LanguageAction("MakeActionRule")>]
    member x.MakeAction(parsingesult: ParsingResult, info: PlayerInfo, data: PlayerData) =
        let parsed =
            (Seq.head parsingesult.Possibilities).Node
            |> visit
            |> clearUnnammed
            
        let translate = Localization.translate localizer info.Locale
        let actionResult = buildAction translate level parsed
        
        match actionResult with
        | ActorNotFound -> x.Answer (translate "screens.level.actor_not_found" |> Localization.value)
        | ActionNotFound -> x.Answer (translate "screens.level.action_not_found" |> Localization.value)
        | RecipientNotFound -> x.Answer (translate "screens.level.recipient_not_found"  |> Localization.value)
        | Success action -> // TODO: Extract method
            let runResult = PlayerData.runAction level data action
                
            let formattedRanActions =
                runResult.ranActions
                |> List.map (formatAction (translate >> Localization.value))
                |> String.concat "\n"
                

            let selectedAnswerString, suggestionsStrings =
                match runResult.status with
                | Runner.Complete ->
                    "screens.level.level_complete", [|
                        "screens.level.next"
                        "screens.level.restart"
                        "common.back"
                    |]
                | Runner.Fail ->
                    "screens.level.level_failed", [|
                        "screens.level.restart"
                        "common.back"
                    |]
                | Runner.Nothing ->
                    "screens.level.level_continues", [|
                        "screens.level.restart"
                        "common.back"
                    |] 

            let suggestions =
                suggestionsStrings
                |> Array.map (translate >> Localization.value)
                |> Array.map Suggestion
                        
            let answer =
                selectedAnswerString
                |> translate 
                |> Localization.format [| formattedRanActions |]
                |> Localization.value
                
            ControllerAnswer(Text = answer, Suggestions = suggestions) :> IControllerAnswer


    override x.DefaultHandler(message: IncomingMessage, info: PlayerInfo, data: PlayerData) = x.OnOpen(info, data)

    override x.OnOpen(info: PlayerInfo, data: PlayerData) =
        let description = (localizer.GetString("levels." + level.name + ".description", info.Locale)).Value
                        
        x.Answer ((localizer.GetString("screens.level.open", info.Locale)).Format(description)).Value