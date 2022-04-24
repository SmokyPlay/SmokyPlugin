using Exiled.API.Features;
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
                foreach(Player player in Player.List) {
                    switch(timer) {
                        case -2:
                            player.ShowHint(Config.LobbyTimerRoundPaused + "\n" + playersConnected);
                        break;
                        case -1:
                            player.ShowHint(Config.LobbyTimerRoundStarting + "\n" + playersConnected);
                        break;
                        case 0:
                            player.ShowHint(Config.LobbyTimerRoundStarting + "\n" + playersConnected);
                        break;
                        default:
                            player.ShowHint(Config.LobbyTimerCountdown.Replace("{time}", timer.ToString()) + "\n" + playersConnected);
                        break;
                    }
                }
                yield return Timing.WaitForSeconds(1);
            }
        }
    }
}