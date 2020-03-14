namespace YogurtTheBot.Game.Logic

open Engine.Models
open System.Collections.Generic
open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers.Abstractions
open YogurtTheBot.Game.Logic.Engine
open YogurtTheBot.Game.Logic.Engine.Runner

type PlayerData() as this =
    inherit PlayerDataBase()

    [<DefaultValue>]
    val mutable controllersStack: List<string>
    
    [<DefaultValue>]    
    val mutable availableLevelsCount: int
    
    [<DefaultValue>]    
    val mutable currentLevel: int
    
    [<DefaultValue>]
    val mutable savedPermissions: Permission list
    
    [<DefaultValue>]
    val mutable savedTags: string list
    
    do
        this.controllersStack <- List<string>()
        this.availableLevelsCount <- 1
        this.currentLevel <- 0
        this.savedPermissions <- List.empty
        this.savedTags <- List.empty

    interface IControllersData with
        member x.ControllersStack
            with get () = x.controllersStack
            and set v = x.controllersStack <- v
            
type NextLevelResult =
    | LevelChanged of PlayerData
    | NoMoreLevels
    | LevelNotAllowed
            
module PlayerData =
    let runAction level (data: PlayerData) action =
        let result = Runner.runAction action data.savedPermissions data.savedTags level
        
        if result.status <> Fail then
            data.savedPermissions <- data.savedPermissions @ result.addedPermissions
            data.savedTags <- data.savedTags @ result.savedTags
        
        result
        
    let level (data: PlayerData) =
        List.item data.currentLevel LevelsList.levels
        
    let clearLevel (data: PlayerData) =
        data.savedPermissions <- List.Empty
        data.savedTags <- List.Empty
        
        data
        
    let nextLevel (data: PlayerData) =
        if data.currentLevel + 1 >= data.availableLevelsCount then
            LevelNotAllowed
        else if data.currentLevel + 1 >= List.length LevelsList.levels then
            NoMoreLevels
        else
            data.currentLevel <- data.currentLevel + 1
            
            LevelChanged (clearLevel data)
            
    let startLevel (data: PlayerData) level =
        if level >= 0 && level < data.availableLevelsCount then
            data.currentLevel <- level
        
        clearLevel data
        
        