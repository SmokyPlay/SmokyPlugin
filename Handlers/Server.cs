using Exiled.API.Features;
using Toys = Exiled.API.Features.Toys;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using UnityEngine;
using MEC;

namespace SmokyPlugin.Handlers
{
    public class ServerHandler
    {
        private CoroutineHandle lobbyTimer;
        
        public CoroutineHandle AutoWarheadBefore;
        private CoroutineHandle AutoWarhead;

        public void OnWaitingForPlayers() {
            if(SmokyPlugin.Singleton.Config.RestartEmpty) SmokyPlugin.Singleton.CheckEmptyTimer(SmokyPlugin.Singleton.Config.RestartEmptyInterval);
            var RoundDuration = SmokyPlugin.Singleton.RoundDuration;
            RoundDuration.CurrentUnit = "none";
            RoundDuration.ElapsedTime = 0;

            GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            lobbyTimer = Timing.RunCoroutine(Methods.LobbyTimer());
            Toys.Light.Create(new Vector3(53.6f, 1019.4f, -44.6f));
            Toys.ShootingTargetToy.Create(ShootingTargetType.Sport, new Vector3(51.1f, 1018.2f, -40.5f), new Vector3(0, -90, 0));
            Toys.ShootingTargetToy.Create(ShootingTargetType.Sport, new Vector3(55.6f, 1018.2f, -40.5f), new Vector3(0, -90, 0));
            Toys.ShootingTargetToy.Create(ShootingTargetType.ClassD, new Vector3(57.8f, 1018.2f, -42.0f));
            Toys.ShootingTargetToy.Create(ShootingTargetType.ClassD, new Vector3(57.8f, 1018.2f, -46.2f));
        }

        public void OnRoundStarted() {
            Server.FriendlyFire = false;
            Timing.KillCoroutines(lobbyTimer);

            AutoWarhead = Timing.RunCoroutine(Methods.AutoWarhead());
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
            var LockedElevators = SmokyPlugin.Singleton.LockedElevators;
            players.Clear();
            LockedElevators.Clear();
            Timing.KillCoroutines(AutoWarhead);
        }
    }
}