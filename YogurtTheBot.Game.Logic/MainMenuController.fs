namespace YogurtTheBot.Game.Logic

open YogurtTheBot.Game.Core
open YogurtTheBot.Game.Core.Controllers
open YogurtTheBot.Game.Core.Controllers.Handlers
open YogurtTheBot.Game.Core.Controllers.Language.Controllers

[<Controller(isMainController = true)>]
type MainMenuController(cp, localizer) =
    inherit LanguageController<PlayerData>(cp, localizer) 

    [<Action("screens.main_menu.start")>]
    member x.SayMyName(player: PlayerInfo, data: PlayerData) = x.Open("LevelController", player, data)

    [<Action("screens.main_menu.attack")>]
    member x.AttackAction() = x.Answer "test"


