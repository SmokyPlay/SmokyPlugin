using System;
using System.Timers;
using System.Collections.Generic;
using MEC;

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

        public List<string> players = new List<string>();

        public Structures.RoundDuration RoundDuration = new Structures.RoundDuration();

        public List<ElevatorType> LockedElevators = new List<ElevatorType>();

        public bool WarheadLocked = false;

        public bool EventLockdown = false;

        public bool AutoEvent = false;
        public string AutoEventType = "";
        public Dictionary<string, Structures.AutoEvent> AutoEvents = new Dictionary<string, Structures.AutoEvent>();

        public bool AutoWarheadEnabled = true;

        public override void OnEnabled() {
            Singleton = this;
            RegisterEvents();
            RegisterAutoEvents();
        }

        public override void OnDisabled() {
            UnregisterEvents();
            UnregisterAutoEvents();
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
            EPlayer.Escaping += player.OnEscaping;
            EPlayer.Died += player.OnDied;
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
            EPlayer.Escaping -= player.OnEscaping;
            EPlayer.Died -= player.OnDied;
            EPlayer.Left -= player.OnLeft;

            EWarhead.Stopping -= warhead.OnStopping;
        }

        private void RegisterAutoEvents() {
            AutoEvents.Add("blackout", new AutoEvents.BlackoutAutoEvent());
            AutoEvents.Add("elevatormalfunction", new AutoEvents.ElevatorMalfunctionAutoEvent());
            AutoEvents.Add("scp173infection", new AutoEvents.SCP173InfectionAutoEvent());
            AutoEvents.Add("hideandseek", new AutoEvents.HideAndSeekAutoEvent());
        }

        private void UnregisterAutoEvents() {
            AutoEvents.Clear();
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