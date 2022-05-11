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
    public class BlackoutAutoEvent : Structures.AutoEvent
    {
        public override string name { get; } = "Блэкаут";
        public override bool AllowLaterJoinSpawn { get; } = true;
        public override bool AllowTeamsRespawn { get; } = true;
        protected override CoroutineHandle EventCoroutine { get; set; }

        private CoroutineHandle Blackout;
        protected override IEnumerator<float> EventProgress()
        {
            Map.Broadcast(5, "<color=#be00ff>В этом раунде автоматически проводится ивент Блэкаут</color>");
            yield return Timing.WaitForSeconds(5f);
            Map.Broadcast(10, "<color=red>Комплекс периодически будет переходить на резервное питание</color>");
            Blackout = Timing.RunCoroutine(BlackoutTimer());
        }
        public override void OnRoundStarted() {
            EventCoroutine = Timing.RunCoroutine(EventProgress());
        }
        public override void OnRestartingRound()
        {
            Timing.KillCoroutines(EventCoroutine, Blackout);
        }
        private IEnumerator<float> BlackoutTimer()
        {
            Random random = new Random();
            var Config = SmokyPlugin.Singleton.Config;
            while(!Round.IsEnded)
            {
                yield return Timing.WaitForSeconds(random.Next(120, 241));
                Map.TurnOffAllLights(3);
                yield return Timing.WaitForSeconds(2f);
                foreach(Room room in Room.List)
                {
                    room.FlickerableLightController.LightIntensityMultiplier = 0.2f;
                    room.Color = new Color(1, 0, 0);
                }
                yield return Timing.WaitForSeconds(60f);
                Map.TurnOffAllLights(3);
                yield return Timing.WaitForSeconds(2f);
                foreach(Room room in Room.List)
                {
                    room.FlickerableLightController.LightIntensityMultiplier = 1;
                    room.ResetColor();
                }
            }
        }
    }
}