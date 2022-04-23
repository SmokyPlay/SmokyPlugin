using System;
using System.IO;
using System.Linq;
using System.Text;

using Exiled.API.Features;
using Exiled.Events.EventArgs;
using IEnumerator = System.Collections.Generic.IEnumerator<float>;
using MEC;

namespace SmokyPlugin.Handlers
{
    public class PlayerHandler
    {
        public void OnVerified(VerifiedEventArgs ev) {
            var players = SmokyPlugin.Singleton.players;
            if(!players.ContainsKey(ev.Player.UserId)) {
                Random random = new Random();
                int randClass = random.Next(1, 9);
                players.Add(ev.Player.UserId, true);
                Log.Info("Проверка");
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

        public void OnLeft(LeftEventArgs ev) {
        }
    }
}