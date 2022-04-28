using Random = System.Random;

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using MEC;

namespace SmokyPlugin.Handlers
{
    public class WarheadHandler
    {
        public void OnStopping(StoppingEventArgs ev) {
            if(SmokyPlugin.Singleton.WarheadLocked && ev.Player != Server.Host) {
                ev.IsAllowed = false;
                var Config = SmokyPlugin.Singleton.Config;
                ev.Player.ShowHint(Config.WarheadLockedHint, Config.WarheadLockedHintDuration);
            }
        }
    }
}