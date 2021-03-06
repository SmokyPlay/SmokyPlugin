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
        public class UnlockElevatorCommand : ICommand {
            public string Command { get; } = "unlock_elevator";
            public string[] Aliases { get; } = {"eunlock"};
            public string Description { get; } = "Разблокировать указанный лифт";
            public string[] Usage = {"lcza/lczb/nuke/scp049/gatea/gateb/all"};
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.elevators")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                if(arguments.Count < 1) {
                    response = "Укажите название лифта";
                    return false;
                }
                var elevators = SmokyPlugin.Singleton.LockedElevators;
                if(arguments.At(0).ToLower() == "all") {
                    elevators.Clear();
                    response = "Все лифты успешно разблокированы";
                    return true;
                }
                if(!ElevatorList.TryGetValue(arguments.At(0).ToLower(), out ElevatorType elevator)) {
                    response = "Лифт не найден";
                    return false;
                }
                if(!elevators.Contains(elevator)) {
                    response = "Этот лифт не заблокирован";
                    return false;
                }
                elevators.Remove(elevator);
                response = $"Лифт {elevator} разблокирован";
                return true;   
            }
            private Dictionary<string, ElevatorType> ElevatorList = new Dictionary<string, ElevatorType>()
            {
                {"lcza", ElevatorType.LczA},
                {"lczb", ElevatorType.LczB},
                {"scp049", ElevatorType.Scp049},
                {"nuke", ElevatorType.Nuke},
                {"gatea", ElevatorType.GateA},
                {"gateb", ElevatorType.GateB},
            };
        }
}