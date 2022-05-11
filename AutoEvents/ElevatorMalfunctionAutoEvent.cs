using System.Threading;
using System.Linq;
using Random = System.Random;
using System.Collections.Generic;
using Exiled.API.Features;
using Toys = Exiled.API.Features.Toys;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using UnityEngine;
using MEC;

namespace SmokyPlugin.AutoEvents
{
    public class ElevatorMalfunctionAutoEvent : Structures.AutoEvent
    {
        public override string name { get; } = "Сбой лифтов";
        public override bool AllowLaterJoinSpawn { get; } = true;
        public override bool AllowTeamsRespawn { get; } = true;
        protected override CoroutineHandle EventCoroutine { get; set; }

        private CoroutineHandle Malfunction;
        protected override IEnumerator<float> EventProgress()
        {
            Map.Broadcast(5, "<color=#be00ff>В этом раунде автоматически проводится ивент Сбой лифтов</color>");
            yield return Timing.WaitForSeconds(5f);
            Map.Broadcast(10, "<color=red>Периодически лифты будут ненадолго выходить из строя</color>");
            Malfunction = Timing.RunCoroutine(MalfunctionTimer());
        }
        public override void OnRoundStarted() {
            EventCoroutine = Timing.RunCoroutine(EventProgress());
        }
        public override void OnRestartingRound()
        {
            Timing.KillCoroutines(EventCoroutine, Malfunction);
        }
        private IEnumerator<float> MalfunctionTimer() {
            Random random = new Random();            
            while(!Round.IsEnded) {
                yield return Timing.WaitForSeconds(random.Next(120, 181));
                var LockedElevators = SmokyPlugin.Singleton.LockedElevators;
                var ElevList = ElevatorList.Keys.ToList();
                var elevator = ElevList[random.Next(ElevList.Count)];
                ElevatorList.TryGetValue(elevator, out string ElevName);
                if(!LockedElevators.Contains(elevator)) LockedElevators.Add(elevator);
                Map.Broadcast(5, $"Сбой лифта <color=red>{ElevName}</color>");
                yield return Timing.WaitForSeconds(random.Next(20, 41));
                if(LockedElevators.Contains(elevator)) LockedElevators.Remove(elevator);
                Map.Broadcast(5, $"Лифт <color=green>{ElevName}</color> отремонтирован!");
            }
        }

        private Dictionary<ElevatorType, string> ElevatorList = new Dictionary<ElevatorType, string>()
        {
            {ElevatorType.LczA, "Лайт Зона А"},
            {ElevatorType.LczB, "Лайт Зона Б"},
            {ElevatorType.Scp049, "SCP 049"},
            {ElevatorType.Nuke, "Боеголовка"},
            {ElevatorType.GateA, "Ворота А"},
            {ElevatorType.GateB, "Ворота Б"},
        };
    }
}