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
        public class AutoWarheadManager : ICommand {
            public string Command { get; } = "autowarhead";
            public string[] Aliases { get; } = {"aw"};
            public string Description { get; } = "Включить/отключить таймер автотматической боеголовки или запустить ее сейчас";
            public string[] Usage = {"enable/disable/start"};
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.autowarhead")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                if(arguments.At(0).ToLower() == "enable" || arguments.At(0).ToLower() == "e") {
                    if(AutoWarhead.IsEnabled) {
                        response = "Таймер автоматической боеголовки уже включен";
                        return false;
                    }
                    SmokyPlugin.Singleton.AutoWarheadEnabled = true;
                    AutoWarhead.Enable();
                    response = "Таймер автоматической боеголовки включен";
                    return true;
                }
                else if(arguments.At(0).ToLower() == "disable" || arguments.At(0).ToLower() == "d") {
                    if(!AutoWarhead.IsEnabled) {
                        response = "Таймер автоматической боеголовки не включен";
                        return false;
                    }
                    SmokyPlugin.Singleton.AutoWarheadEnabled = false;
                    AutoWarhead.Disable();
                    response = "Таймер автоматической боеголовки отключен";
                    return true;
                }
                else if(arguments.At(0).ToLower() == "start" || arguments.At(0).ToLower() == "s") {
                    AutoWarhead.Start();
                    response = "Автоматическая боеголовка запущена";
                    return true;
                }
                else {
                    response = "enable - включить, disable - отключить, start - запустить боеголовку";
                    return false;
                }
            }
        }
}