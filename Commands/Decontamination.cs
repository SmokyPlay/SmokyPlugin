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
        public class Decontamination : ICommand {
            public string Command { get; } = "start_decontamination";
            public string[] Aliases { get; } = {"sd"};
            public string Description { get; } = "Запустить обеззараживание лайт зоны";
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.decontamination")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                Map.StartDecontamination();
                response = "Обеззараживание лайт зоны запущено";
                return true;
            }
        }
}