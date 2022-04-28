using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.Permissions.Extensions;
using RemoteAdmin;

namespace SmokyPlugin.Commands
{
        [CommandHandler(typeof(RemoteAdminCommandHandler))]
        public class EventLockdown : ICommand {
            public string Command { get; } = "event_lockdown";
            public string[] Aliases { get; } = {"eventlock", "evlock"};
            public string Description { get; } = "Замутить всех игроков в лобби и очистить их инвентарь для объяснения правил ивента";
            public string[] Usage = {"lcza/lczb/nuke/scp049/gatea/gateb/all"};
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.eventlockdown")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                var elevators = SmokyPlugin.Singleton.LockedElevators;
                var Config = SmokyPlugin.Singleton.Config;
                var EventLockdown = SmokyPlugin.Singleton.EventLockdown;
                if(!Round.IsLobby) {
                    response = "Раунд уже запущен";
                    return false;
                }
                if(!SmokyPlugin.Singleton.EventLockdown) {
                    Round.IsLobbyLocked = true;
                    SmokyPlugin.Singleton.EventLockdown = true;
                    foreach(Player pl in Player.List) {
                        if(pl != player) {
                            pl.ClearInventory(false);
                            pl.IsMuted = true;
                            pl.Broadcast(Config.EventLockMessageDuration, Config.EventLockMessage);
                        }
                    }
                    response = "Раунд заблокирован для объяснения правил ивента";
                    return true;
                }
                else {
                    SmokyPlugin.Singleton.EventLockdown = false;
                    foreach(Player pl in Player.List) {
                        if(pl != player) pl.IsMuted = false;
                    }
                    Round.IsLobbyLocked = false;
                    response = "Раунд разблокирован";
                    return true;
                }
            }
        }
}