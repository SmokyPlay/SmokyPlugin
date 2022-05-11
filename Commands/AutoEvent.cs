using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using MEC;

namespace SmokyPlugin.Commands
{
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class AutoEvent : ICommand {
            public string Command { get; } = "autoevent";
            public string[] Aliases { get; } = {"aev"};
            public string Description { get; } = "Запустить автоматический ивент в этом раунде";
            public string[] Usage = {"event_name"};
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.events")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                if(!Round.IsLobby) {
                    response = "Раунд уже запущен";
                    return false;
                }
                var AutoEvents = SmokyPlugin.Singleton.AutoEvents;
                var AutoEventType = arguments.At(0).ToLower();
                if(!AutoEvents.ContainsKey(AutoEventType)) {
                    response = "Ивент не найден";
                    return false;
                }
                AutoEvents.TryGetValue(arguments.At(0).ToLower(), out Structures.AutoEvent AutoEvent);
                AutoEvent.PrepareRound();
                SmokyPlugin.Singleton.AutoEvent = true;
                SmokyPlugin.Singleton.AutoEventType = AutoEventType;

                response = $"Ивент {AutoEvent.name} будет проведен автоматически!";
                return true;
            }
        }
}