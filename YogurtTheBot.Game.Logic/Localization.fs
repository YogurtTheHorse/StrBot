module YogurtTheBot.Game.Logic.Localization

open System
open YogurtTheBot.Game.Core.Localizations

let translate (localizer: ILocalizer) locale s =
    localizer.GetString(s, locale)
    
let format args (localization: Localization) =
    localization.Format(args)
    
let value (localization: Localization) = localization.Value
