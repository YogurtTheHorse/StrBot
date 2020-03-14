namespace YogurtTheBot.Game.Logic

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Communications
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Handlers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

[<Controller(isMainController = true)>]
type MainMenuController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer) 

    [<Action("screens.main_menu.continue")>]
    member x.Continue(info: PlayerInfo, data: PlayerData) = 
        let data = PlayerData.startLevel data (data.availableLevelsCount - 1) 
        x.Open("LevelController", info, data)

    [<Action("screens.main_menu.start")>]
    member x.StartFromFirst(info: PlayerInfo, data: PlayerData) =
        let data = PlayerData.startLevel data 0 
        x.Open("LevelController", info, data)

    [<Action("screens.main_menu.how_to_play_button")>]
    member x.HowToPlay(info: PlayerInfo) =
        x.Answer (Localization.translate localizer info.Locale "screens.main_menu.how_to_play" |> Localization.value)

    override x.DefaultHandler(message: IncomingMessage, info: PlayerInfo, data: PlayerData) =
        x.Answer (Localization.translate localizer info.Locale "screens.main_menu.default" |> Localization.value)

    override x.OnOpen(info: PlayerInfo, data: PlayerData) =
        x.Answer (localizer.GetString("screens.main_menu.open", info.Locale).Value)

