using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace SmokyPlugin
{
    public class AutoWarhead
    {
        public static bool IsEnabled = true;
        private static CoroutineHandle AutoWarheadMessageCoroutine;
        private static CoroutineHandle AutoWarheadMessage;
        private static CoroutineHandle AutoWarheadCoroutine;

        public static IEnumerator<float> AutoWarheadMessageTimer() {
            var Config = SmokyPlugin.Singleton.Config;
            ushort seconds = Config.AutoWarheadMessageBeforeDetonationTime;
            while(seconds > 0) {
                var broadcast = new Broadcast();
                Map.Broadcast(1, Config.AutoWarheadMessageBeforeDetonation.Replace("{time}", seconds.ToString()));
                seconds--;
                yield return Timing.WaitForSeconds(1);
            }
        }

        public static void Start() {
            var Config = SmokyPlugin.Singleton.Config;
            SmokyPlugin.Singleton.WarheadLocked = true;
            if(!Warhead.IsInProgress) Warhead.Start();
            Timing.KillCoroutines(AutoWarheadMessage, AutoWarheadMessageCoroutine, AutoWarheadCoroutine);
            Map.Broadcast(Config.AutoWarheadDetonationMessageDuration, Config.AutoWarheadDetonationMessage);
        }

        public static void Enable() {
            IsEnabled = true;
            var Config = SmokyPlugin.Singleton.Config;
            AutoWarheadMessage = Timing.CallDelayed(Config.AutoWarheadTime - Config.AutoWarheadMessageBeforeDetonationTime, () => {
                AutoWarheadMessageCoroutine = Timing.RunCoroutine(AutoWarheadMessageTimer());
            });
            AutoWarheadCoroutine = Timing.CallDelayed(Config.AutoWarheadTime, Start);
        }

        public static void Disable() {
            IsEnabled = false;
            Timing.KillCoroutines(AutoWarheadMessage, AutoWarheadMessageCoroutine, AutoWarheadCoroutine);
        }
    }
}