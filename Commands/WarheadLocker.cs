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
        public class WarheadLocker : ICommand {
            public string Command { get; } = "warhead_locker";
            public string[] Aliases { get; } = {"wl", "nl"};
            public string Description { get; } = "Заблокировать указанный лифт";
            public string[] Usage = {"lock/unlock"};
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.whlock")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                if(arguments.At(0).ToLower() == "lock" || arguments.At(0).ToLower() == "l") {
                    response = "Кнопка отключения боеголовки заблокирована";
                    SmokyPlugin.Singleton.WarheadLocked = true;
                    return true;
                }
                else if(arguments.At(0).ToLower() == "unlock" || arguments.At(0).ToLower() == "u") {
                    response = "Кнопка отключения боеголовки разблокирована";
                    SmokyPlugin.Singleton.WarheadLocked = false;
                    return true;
                }
                else {
                    response = "Укажите lock - заблокировать, unlock - разблокировать";
                    return false;
                }
            }
        }
}