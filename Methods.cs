using Exiled.API.Features;
using Exiled.API.Enums;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace SmokyPlugin
{
    public class Methods
    {
        public static IEnumerator<float> LobbyTimer() {
            var Config = SmokyPlugin.Singleton.Config;
            while(!Round.IsStarted) {
                short timer = GameCore.RoundStart.singleton.NetworkTimer;
                string playersConnected = Config.LobbyTimerPlayersConnected.Replace("{players}", Player.List.Count().ToString());
                string AutoEventMessage = "";
                if(SmokyPlugin.Singleton.AutoEvent) {
                    SmokyPlugin.Singleton.AutoEvents.TryGetValue(SmokyPlugin.Singleton.AutoEventType, out Structures.AutoEvent AutoEvent);
                    AutoEventMessage = SmokyPlugin.Singleton.Config.LobbyTimerAutoEvent.Replace("{event}", AutoEvent.name);
                }
                foreach(Player player in Player.List) {
                    switch(timer) {
                        case -2:
                            player.ShowHint(AutoEventMessage + "\n" + Config.LobbyTimerRoundPaused + "\n" + playersConnected);
                        break;
                        case -1:
                            player.ShowHint(AutoEventMessage + "\n" + Config.LobbyTimerRoundStarting + "\n" + playersConnected);
                        break;
                        case 0:
                            player.ShowHint(AutoEventMessage + "\n" + Config.LobbyTimerRoundStarting + "\n" + playersConnected);
                        break;
                        default:
                            player.ShowHint(AutoEventMessage + "\n" + Config.LobbyTimerCountdown.Replace("{time}", timer.ToString()) + "\n" + playersConnected);
                        break;
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }

        public static void SpawnTutorial(Player player) {
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

        private static ItemType[] Guns = {
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

        private static AmmoType[] Ammo = {
            AmmoType.Nato9,
            AmmoType.Nato556,
            AmmoType.Nato762,
            AmmoType.Ammo12Gauge,
            AmmoType.Ammo44Cal
        };
    }
}