using System;
using System.Timers;
using System.Collections.Generic;

using Exiled.API.Features;
using Exiled.API.Enums;

using EServer = Exiled.Events.Handlers.Server;
using EPlayer = Exiled.Events.Handlers.Player;
using EWarhead = Exiled.Events.Handlers.Warhead;

namespace SmokyPlugin
{
    public class SmokyPlugin : Plugin<Config> {
        public override PluginPriority Priority { get; } = PluginPriority.Medium;

        public static SmokyPlugin Singleton;
        private Handlers.ServerHandler server;
        private Handlers.PlayerHandler player;
        private Handlers.WarheadHandler warhead;

        public Dictionary<string, bool> players = new Dictionary<string, bool>();

        public Interfaces.RoundDuration RoundDuration = new Structures.RoundDuration();

        public Dictionary<string, ElevatorType> LockedElevators = new Dictionary<string, ElevatorType>();

        public bool WarheadLocked = false;

        public bool EventLockdown = false;

        public override void OnEnabled() {
            Singleton = this;
            RegisterEvents();
        }

        public override void OnDisabled() {
            UnregisterEvents();
        }

        public void RegisterEvents() {
            player = new Handlers.PlayerHandler();
            server = new Handlers.ServerHandler();
            warhead = new Handlers.WarheadHandler();

            EServer.WaitingForPlayers += server.OnWaitingForPlayers;
            EServer.RoundStarted += server.OnRoundStarted;
            EServer.RespawningTeam += server.OnRespawningTeam;
            EServer.RoundEnded += server.OnRoundEnded;
            EServer.RestartingRound += server.OnRestartingRound;

            EPlayer.Verified += player.OnVerified;
            EPlayer.InteractingShootingTarget += player.OnInteractingShootingTarget;
            EPlayer.TriggeringTesla += player.OnTriggeringTesla;
            EPlayer.InteractingElevator += player.OnInteractingElevator;
            EPlayer.PickingUpScp330 += player.OnPickingUpScp330;
            EPlayer.Left += player.OnLeft;

            EWarhead.Stopping += warhead.OnStopping;
        }

        public void UnregisterEvents() {
            EServer.WaitingForPlayers -= server.OnWaitingForPlayers;
            EServer.RoundStarted -= server.OnRoundStarted;
            EServer.RespawningTeam -= server.OnRespawningTeam;
            EServer.RoundEnded -= server.OnRoundEnded;
            EServer.RestartingRound -= server.OnRestartingRound;

            EPlayer.Verified -= player.OnVerified;
            EPlayer.InteractingShootingTarget -= player.OnInteractingShootingTarget;
            EPlayer.TriggeringTesla -= player.OnTriggeringTesla;
            EPlayer.InteractingElevator -= player.OnInteractingElevator;
            EPlayer.PickingUpScp330 -= player.OnPickingUpScp330;
            EPlayer.Left -= player.OnLeft;

            EWarhead.Stopping -= warhead.OnStopping;
        }

        public void CheckEmptyTimer(ushort interval) {
            Timer checkEmptyTimer = new System.Timers.Timer();
            checkEmptyTimer.Interval = interval * 1000;
            checkEmptyTimer.AutoReset = true;
            checkEmptyTimer.Enabled = true;
            checkEmptyTimer.Elapsed += CheckEmpty;
        }

        public void CheckEmpty(Object source, System.Timers.ElapsedEventArgs e) {
            if(Server.PlayerCount == 0 && Round.IsStarted) Round.Restart();
        }
    }
}