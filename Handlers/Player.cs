using Random = System.Random;

using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs;
using Exiled.API.Enums;
using InventorySystem;
using MEC;

namespace SmokyPlugin.Handlers
{
    public class PlayerHandler
    {
        public void OnVerified(VerifiedEventArgs ev) {
            var players = SmokyPlugin.Singleton.players;
            bool AllowLaterJoinSpawn = true;
            if(SmokyPlugin.Singleton.AutoEvent) {
                SmokyPlugin.Singleton.AutoEvents.TryGetValue(SmokyPlugin.Singleton.AutoEventType, out Structures.AutoEvent AutoEvent);
                AutoEvent.OnVerified(ev);
                AllowLaterJoinSpawn = AutoEvent.AllowLaterJoinSpawn;
            }
            if(!Round.IsStarted && !Round.IsEnded) {
                Timing.CallDelayed(1, () => Methods.SpawnTutorial(ev.Player));
            }
            if(!players.Contains(ev.Player.UserId)) {
                Random random = new Random();
                int randClass = random.Next(1, 9);
                players.Add(ev.Player.UserId);
                if(AllowLaterJoinSpawn && ((Round.ElapsedTime.TotalSeconds - SmokyPlugin.Singleton.RoundDuration.ElapsedTime) < 120) &&
                    (Round.ElapsedTime.TotalSeconds >= Server.LaterJoinTime)) {
                    switch(SmokyPlugin.Singleton.RoundDuration.CurrentUnit) {
                        case "none":
                            Timing.CallDelayed(1, () => ev.Player.SetRole(randClass <= 2 ? RoleType.Scientist : 
                                (randClass > 5 ? RoleType.FacilityGuard : RoleType.ClassD)));
                        break;
                        case "mtf":
                            Timing.CallDelayed(1, () => ev.Player.SetRole(randClass <= 3 ? RoleType.NtfSergeant : RoleType.NtfPrivate));
                        break;
                        case "ci":
                            Timing.CallDelayed(1, () => ev.Player.SetRole(randClass <= 2 ? RoleType.ChaosMarauder : 
                                (randClass > 4 ? RoleType.ChaosRifleman : RoleType.ChaosRepressor)));
                        break;
                    }
                }
            } 
            string message = SmokyPlugin.Singleton.Config.JoinMessage.Replace("{player}", ev.Player.Nickname);
            ushort duration = SmokyPlugin.Singleton.Config.JoinMessageDuration;
            ev.Player.Broadcast(duration, message);
        }

        public void OnInteractingShootingTarget(InteractingShootingTargetEventArgs ev) {
            if(ev.TargetButton == ShootingTargetButton.Remove) ev.IsAllowed = false;
        }

        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev) {
            if(TeslaWhitelist.Contains(ev.Player.Role)) ev.IsTriggerable = false;
        }

        public void OnInteractingElevator(InteractingElevatorEventArgs ev) {
            if(SmokyPlugin.Singleton.LockedElevators.Contains(ev.Lift.Type)) {  
                ev.IsAllowed = false;
                var Config = SmokyPlugin.Singleton.Config;
                ev.Player.ShowHint(Config.ElevatorLockedDownHint, Config.ElevatorLockedDownHintDuration);
            }
        }

        public void OnPickingUpScp330(PickingUpScp330EventArgs ev) {
            Random random = new Random();
            if(random.Next(1, 11) <= 2) {
                ev.IsAllowed = false;
                ev.Player.TryAddCandy(InventorySystem.Items.Usables.Scp330.CandyKindID.Pink);
            }
        }

        public void OnEscaping(EscapingEventArgs ev) {
            if(SmokyPlugin.Singleton.AutoEvent) {
                SmokyPlugin.Singleton.AutoEvents.TryGetValue(SmokyPlugin.Singleton.AutoEventType, out Structures.AutoEvent AutoEvent);
                AutoEvent.OnEscaping(ev);
            }
        }

        public void OnDied(DiedEventArgs ev) {
            if(SmokyPlugin.Singleton.AutoEvent) {
                SmokyPlugin.Singleton.AutoEvents.TryGetValue(SmokyPlugin.Singleton.AutoEventType, out Structures.AutoEvent AutoEvent);
                AutoEvent.OnDied(ev);
            }
        }

        public void OnLeft(LeftEventArgs ev) {
            if(SmokyPlugin.Singleton.AutoEvent) {
                SmokyPlugin.Singleton.AutoEvents.TryGetValue(SmokyPlugin.Singleton.AutoEventType, out Structures.AutoEvent AutoEvent);
                AutoEvent.OnLeft(ev);
            }
        }

        private RoleType[] TeslaWhitelist = {
            RoleType.Scientist,
            RoleType.FacilityGuard,
            RoleType.NtfPrivate,
            RoleType.NtfSpecialist,
            RoleType.NtfSergeant,
            RoleType.NtfCaptain
        };
    }
}