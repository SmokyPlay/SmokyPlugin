using System.Threading;
using System.Linq;
using Random = System.Random;
using System.Collections.Generic;
using Exiled.API.Features;
using Toys = Exiled.API.Features.Toys;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using UnityEngine;
using MEC;

namespace SmokyPlugin.AutoEvents
{
    public class SCP173InfectionAutoEvent : Structures.AutoEvent
    {
        public override string name { get; } = "Заражение SCP-173";
        public override bool AllowLaterJoinSpawn { get; } = false;
        public override bool AllowTeamsRespawn { get; } = false;
        protected override CoroutineHandle EventCoroutine { get; set; }

        private bool IsStarted = false;
        private bool IsEnded = false;
        public override void PrepareRound()
        {
            SmokyPlugin.Singleton.AutoWarheadEnabled = false;
            AutoWarhead.Disable();
            LightContainmentZoneDecontamination.DecontaminationController.Singleton.disableDecontamination = true;
            Round.IsLocked = true;
            foreach(Door door in Door.List) {
                door.Lock(float.MaxValue, DoorLockType.AdminCommand);
            }
            Door.Get(DoorType.GateA).IsOpen = true;
            Door.Get(DoorType.GateB).IsOpen = true;
        }
        protected override IEnumerator<float> EventProgress()
        {
            Random random = new Random();
            Map.Broadcast(5, "<color=#be00ff>В этом раунде автоматически проводится ивент Заражение SCP-173</color>");
            yield return Timing.WaitForSeconds(5f);
            foreach(Player player in Player.List) {
                player.SetRole(RoleType.ClassD);
                player.AddItem(ItemType.KeycardZoneManager);
            }
            Map.Broadcast(10, "<color=orange>Все игроки были заспавнены за Класс-Д\nИх задача - сбежать из комплекса, ворота А и Б открыты</color>");
            yield return Timing.WaitForSeconds(10f);
            Player SCP173Player = Player.List.ElementAt(random.Next(Player.List.Count()));
            SCP173Player.SetRole(RoleType.Scp173);
            Map.Broadcast(10, "<color=red>Один из игроков был заспавнен за SCP-173\nЕго задача - убить весь Класс-Д</color>");
            yield return Timing.WaitForSeconds(10f);
            Map.Broadcast(10, "<color=blue>Убитые дешки будут также становиться печеньками</color>");
            yield return Timing.WaitForSeconds(10f);
            Map.Broadcast(10, "<color=#be00ff>Двери разблокированы\nУдачи!</color>");
            foreach(Door door in Door.List) {
                door.Unlock();
            }
            IsStarted = true;
        }
        public override void OnRoundStarted() {
            EventCoroutine = Timing.RunCoroutine(EventProgress());
        }
        public override void OnVerified(VerifiedEventArgs ev)
        {
            if(!SmokyPlugin.Singleton.players.Contains(ev.Player.UserId)) {
                if(IsStarted) {
                    ev.Player.SetRole(RoleType.Scp173);
                    ev.Player.ShowHint("<color=#be00ff>На сервере автоматически проводится ивент Заражение SCP-173</color>", 10);
                }
                else {
                    ev.Player.SetRole(RoleType.ClassD);
                    ev.Player.AddItem(ItemType.KeycardZoneManager);
                }
            }
        }
        public override void OnEscaping(EscapingEventArgs ev)
        {
            if(IsEnded) return;
            IsEnded = true;
            Map.Broadcast(20, $"<color=orange>Победа Класса-Д\nПервым сбежал</color> <color=#be00ff>{ev.Player.Nickname}</color>");
            Timing.CallDelayed(5, () => {
                foreach(Player player in Player.List) {
                    if(player.Role == RoleType.Scp173) player.SetRole(RoleType.Spectator);
                }
                Round.IsLocked = false;
            });
        }
        public override void OnDied(DiedEventArgs ev)
        {
            if(!IsStarted || IsEnded) return;
            if(ev.TargetOldRole == RoleType.ClassD) ev.Target.SetRole(RoleType.Scp173);
            if(Player.List.Where(pl => pl.Role == RoleType.ClassD).Count() == 0) {
                IsEnded = true;
                Map.Broadcast(20, "<color=red>Победа SCP\nВсе дешки были убиты</color>");
                Timing.CallDelayed(5, () => Round.IsLocked = false);
            }
            else if(Player.List.Where(pl => pl.Role == RoleType.Scp173).Count() == 0) {
                IsEnded = true;
                Map.Broadcast(20, "<color=orange>Победа Класса-Д\nВсе SCP-173 были уничтожены</color>");
                Timing.CallDelayed(5, () => Round.IsLocked = false);
            }
        }
        public override void OnLeft(LeftEventArgs ev)
        {
            Log.Info(ev.Player.Role);
            if(!IsStarted && ev.Player.Role == RoleType.Scp173) {
                Timing.CallDelayed(1, () => {
                    if(IsStarted) return;
                    Random random = new Random();
                    Player SCP173Player = Player.List.ElementAt(random.Next(Player.List.Count()));
                    SCP173Player.SetRole(RoleType.Scp173);
                    SCP173Player.ShowHint("<color=#be00ff>Вы стали SCP-173 из-за выхода предыдущего игрока", 10);
                });
            }
        }
        public override void OnRestartingRound()
        {
            IsStarted = false;
            IsEnded = false;
            Timing.KillCoroutines(EventCoroutine);
        }
    }
}