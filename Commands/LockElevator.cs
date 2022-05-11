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
        public class LockElevatorCommand : ICommand {
            public string Command { get; } = "lock_elevator";
            public string[] Aliases { get; } = {"elock"};
            public string Description { get; } = "Заблокировать указанный лифт";
            public string[] Usage = {"lcza/lczb/nuke/scp049/gatea/gateb/all"};
            public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
                Player player = Player.Get(sender);
                if(!player.CheckPermission("sp.elevators")) {
                    response = "У вас недостаточно прав для использования этой команды";
                    return false;
                }
                var elevators = SmokyPlugin.Singleton.LockedElevators;
                if(arguments.Count < 1) {
                    response = "Укажите название лифта";
                    return false;
                }
                if(arguments.At(0).ToLower() == "all") {
                    foreach (KeyValuePair<string, ElevatorType> elev in ElevatorList)
                    {
                        if(!elevators.Contains(elev.Value)) elevators.Add(elev.Value);
                    }
                    response = "Все лифты успешно заблокированы";
                    return true;
                }
                if(!ElevatorList.TryGetValue(arguments.At(0).ToLower(), out ElevatorType elevator)) {
                    response = "Лифт не найден";
                    return false;
                }
                if(elevators.Contains(elevator)) {
                    response = "Этот лифт уже заблокирован";
                    return false;
                }
                elevators.Add(elevator);
                response = $"Лифт {elevator} заблокирован";
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