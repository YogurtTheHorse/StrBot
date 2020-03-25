# StrBot
This repo contains my game, currently developing for telegram and for Yandex.Alice in the future probably. Core concept 
is in development and for a while i'm working on periphery around bot.a

# Dependencies 
Bot is developed with .NET Core 3.1. But to run it you will be required to have RabbitMQ server and MongoDB. All 
configuration could be found in appsettings. To test it in Telegram you will ned a telegram bot token.

# Docker & docker-compose

## Starting docker-compose

If you want to have start all services follow up these commands:
```bash
 docker-compose -f YogurtTheBot.ComposeFiles/docker-compose.yml up --build -d
```

# Projects 
A lot of projects, yeah? :)

## MongoDB.Bson.FSharp
This one contains some stuff to connect F# with MongoDB. (Not my code tbh)

## YogurtTheBot.Alice
This project contains web server to run Yandex.Alice client

## YogurtTheBot.Game.Core.Controllers
This project contains Controllers API for core. So it lets build some hierarchy similar to something between ASP.NET controllers and stack-based screens model. 

### YogurtTheBot.Game.Core.Controllers.Autofac
This project contains some extensions to easily integrate YogurtTheBot.Game.Core.Controllers with Autofac

### YogurtTheBot.Game.Core.Controllers.Language
My own formal grammatics parser. So I decided that Irnoy and FParsec would be too complex for a game and wrote my own, but simplier. Maybe I will migrate to FParsec or Irony someday.

#### YogurtTheBot.Game.Core.Controllers.Language.Tests
Tests for language project

## YogurtTheBot.Game.Core
Core of the messaging mechanism. Have a bit of abstractions and localization stuff.

## YogurtTheBot.Game.Data
Core models and MongoDB repositories.

## YogurtTheBot.Game.Logic.Engine
Game engine. Core of the very game. Contains models and connected functions.
 
### YogurtTheBot.Game.Logic.Engine.Tests
Game engine tests. Currently there is only one test which tests default levels.

## YogurtTheBot.Game.Logic
Game controllers.

## YogurtTheBot.Game.Server
Console server.

### YogurtTheBot.Game.Server.RabbitMq
Abstractions of server to work with rabbitmq.

## YogurtTheBot.Telegram.Polling
Telegram client. 

# Running
Run server and client in the same time to launch everything. 
