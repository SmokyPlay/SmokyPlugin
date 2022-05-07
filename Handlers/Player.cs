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
        private ItemType[] Guns = {
            ItemType.GunCOM15,
            ItemType.GunCOM18,
            ItemType.GunFSP9,
            ItemType.GunCrossvec,
            ItemType.GunE11SR,
            ItemType.GunAK,
            ItemType.GunShotgun,
            ItemType.GunRevolver,
            ItemType.GunLogicer
        };

        private AmmoType[] Ammo = {
            AmmoType.Nato9,
            AmmoType.Nato556,
            AmmoType.Nato762,
            AmmoType.Ammo12Gauge,
            AmmoType.Ammo44Cal
        };

        public void OnVerified(VerifiedEventArgs ev) {
            var players = SmokyPlugin.Singleton.players;
            if(!Round.IsStarted && !Round.IsEnded) {
                Timing.CallDelayed(1, () => SpawnTutorial(ev.Player));
            }
            if(!players.ContainsKey(ev.Player.UserId)) {
                Random random = new Random();
                int randClass = random.Next(1, 9);
                players.Add(ev.Player.UserId, true);
                if(((Round.ElapsedTime.TotalSeconds - SmokyPlugin.Singleton.RoundDuration.ElapsedTime) < 120) &&
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
            if(SmokyPlugin.Singleton.LockedElevators.ContainsValue(ev.Lift.Type)) {
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

        public void OnLeft(LeftEventArgs ev) {
        }

        private void SpawnTutorial(Player player) {
            var Config = SmokyPlugin.Singleton.Config;
            player.SetRole(RoleType.Tutorial);
            if(!SmokyPlugin.Singleton.EventLockdown) {
                player.AddItem(ItemType.SCP1853);
                player.AddItem(ItemType.ArmorHeavy);
                player.AddItem(ItemType.ParticleDisruptor);
                player.AddItem(Guns.RandomItem());
                foreach (AmmoType ammo in Ammo)
                {
                    if(ammo == AmmoType.Ammo12Gauge) player.SetAmmo(ammo, 74);
                    else if(ammo == AmmoType.Ammo44Cal) player.SetAmmo(ammo, 68);
                    else player.SetAmmo(ammo, 200);
                }
            }
            else {
                player.IsMuted = true;
                player.Broadcast(Config.EventLockMessageDuration, Config.EventLockMessage);
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