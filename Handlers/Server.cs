using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace SmokyPlugin.Handlers
{
    public class ServerHandler
    {
        public void OnWaitingForPlayers() {
            if(SmokyPlugin.Singleton.Config.RestartEmpty) SmokyPlugin.Singleton.CheckEmptyTimer(SmokyPlugin.Singleton.Config.RestartEmptyInterval);
            var RoundDuration = SmokyPlugin.Singleton.RoundDuration;
            RoundDuration.CurrentUnit = "none";
            RoundDuration.ElapsedTime = 0;
        }

        public void OnRoundStarted() {
            Server.FriendlyFire = false;
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev) {
            var RoundDuration = SmokyPlugin.Singleton.RoundDuration;
            switch(ev.NextKnownTeam) {
                case Respawning.SpawnableTeamType.NineTailedFox:
                    RoundDuration.CurrentUnit = "mtf";
                break;
                case Respawning.SpawnableTeamType.ChaosInsurgency:
                    RoundDuration.CurrentUnit = "ci";
                break;
            }
            RoundDuration.ElapsedTime = Round.ElapsedTime.TotalSeconds;
        }

        public void OnRoundEnded(RoundEndedEventArgs ev) {
            Map.Broadcast(SmokyPlugin.Singleton.Config.RoundEndBroadcastDuration,
                SmokyPlugin.Singleton.Config.RoundEndBroadcast);
            if(SmokyPlugin.Singleton.Config.RoundEndFf) Server.FriendlyFire = true;
        }

        public void OnRestartingRound() {
            var players = SmokyPlugin.Singleton.players;
            players.Clear();
        }
    }
}