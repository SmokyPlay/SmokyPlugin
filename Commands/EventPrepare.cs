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
        public class EventPrepare : ICommand {
            public string Command { get; } = "event_prepare";
            public string[] Aliases { get; } = {"evprepare", "evprep"};
            public string Description { get; } = "Отключает обеззараживание лайт зоны и автоматическую боеголовку, а также блокирует раунд и все двери";
            public string[] Usage = {};
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
                SmokyPlugin.Singleton.AutoWarheadEnabled = false;
                AutoWarhead.Disable();
                LightContainmentZoneDecontamination.DecontaminationController.Singleton.disableDecontamination = true;
                Round.IsLocked = true;
                foreach(Door door in Door.List) {
                    door.Lock(float.MaxValue, DoorLockType.AdminCommand);
                }
                response = "Раунд подготовлен к проведению ивента";
                return true;
            }
        }
}