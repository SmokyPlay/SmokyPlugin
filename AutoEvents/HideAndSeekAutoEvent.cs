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
    public class HideAndSeekAutoEvent : Structures.AutoEvent
    {
        public override string name { get; } = "Прятки";
        public override bool AllowLaterJoinSpawn { get; } = false;
        public override bool AllowTeamsRespawn { get; } = false;
        protected override CoroutineHandle EventCoroutine { get; set; }

        private bool IsStarted = false;
        private bool IsEnded = false;
        private Player SCP939Player;
        public override void PrepareRound()
        {
            SmokyPlugin.Singleton.AutoWarheadEnabled = false;
            AutoWarhead.Disable();
            LightContainmentZoneDecontamination.DecontaminationController.Singleton.disableDecontamination = true;
            Round.IsLocked = true;
            foreach(Door door in Door.List) {
                door.Lock(float.MaxValue, DoorLockType.AdminCommand);
            }
            SmokyPlugin.Singleton.LockedElevators.Add(ElevatorType.LczA);
            SmokyPlugin.Singleton.LockedElevators.Add(ElevatorType.LczB);
        }
        protected override IEnumerator<float> EventProgress()
        {
            Random random = new Random();
            Map.Broadcast(5, "<color=#be00ff>В этом раунде автоматически проводится ивент Прятки</color>");
            yield return Timing.WaitForSeconds(5f);
            foreach(Player player in Player.List) {
                player.SetRole(RoleType.ClassD);
                Timing.CallDelayed(1, () => {
                    Vector3 DoorPosition = Door.Get(DoorType.HeavyContainmentDoor).Position;
                    player.Teleport(new Vector3(DoorPosition.x, -998, DoorPosition.z));
                });
            }
            Map.Broadcast(10, "<color=orange>Все игроки были заспавнены за Класс-Д\nИх задача - спрятаться от SCP-939 в хард зоне</color>");
            yield return Timing.WaitForSeconds(10f);
            SCP939Player = Player.List.ElementAt(random.Next(Player.List.Count()));
            SCP939Player.SetRole(RoleType.Tutorial);
            Methods.SpawnTutorial(SCP939Player);
            SCP939Player.ShowHint("<b><color=#be00ff>Вы будете заспавнены за SCP-939 через 2 минуты</color></b>", 25);
            Map.Broadcast(10, "<color=red>Один из игроков будет заспавнен за SCP-939 через 2 минуты\nЕго задача - найти и убить весь Класс-Д</color>");
            yield return Timing.WaitForSeconds(10f);
            Map.Broadcast(10, "<color=blue>Убитые дешки будут также становиться собаками\nПоследний выживший победит</color>");
            yield return Timing.WaitForSeconds(10f);
            foreach(Door door in Door.List) {
                if(door.Type != DoorType.CheckpointEntrance && door.Type != DoorType.Scp079First && door.Type != DoorType.Scp079Second
                && door.Type != DoorType.Scp096 && door.Type != DoorType.HID) {
                    door.Unlock();
                    if(door.Zone == ZoneType.HeavyContainment) door.IsOpen = true;
                }
            }
            Map.Broadcast(10, "SCP-939 начнет охоту через <color=red>2 минуты</color>\nУдачи!");
            yield return Timing.WaitForSeconds(60f);
            Map.Broadcast(10, "SCP-939 начнет охоту через <color=red>1 минуту</color>!");
            yield return Timing.WaitForSeconds(50f);
            Map.Broadcast(5, "SCP-939 начнет охоту через <color=red>10 секунд</color>!");
            yield return Timing.WaitForSeconds(10f);
            Map.Broadcast(10, "<color=red>SCP-939 начал охоту!</color>");
            SCP939Player.SetRole(random.Next(0, 2) == 0 ? RoleType.Scp93953 : RoleType.Scp93989);
            IsStarted = true;
            yield return Timing.WaitForSeconds(60f);
            Map.Broadcast(10, "Чекпоинт в офисы будет открыт через <color=red>5 минут</color>!");
            yield return Timing.WaitForSeconds(240f);
            Map.Broadcast(10, "Чекпоинт в офисы будет открыт через <color=red>1 минуту</color>!");
            yield return Timing.WaitForSeconds(60f);
            Map.Broadcast(10, "<color=red>Чекпоинт в офисы открыт!</color>\n<color=orange>Первая сбежавшая или последняя выжившая дешка победит</color>");
            foreach(Player player in Player.List.Where(pl => pl.Role == RoleType.ClassD)) {
                player.AddItem(ItemType.KeycardNTFLieutenant);
                player.AddItem(ItemType.Adrenaline);
            }
            Door.Get(DoorType.CheckpointEntrance).Unlock();
        }
        public override void OnRoundStarted() {
            EventCoroutine = Timing.RunCoroutine(EventProgress());
        }
        public override void OnVerified(VerifiedEventArgs ev)
        {
            Random random = new Random();
            if(!SmokyPlugin.Singleton.players.Contains(ev.Player.UserId)) {
                if(IsStarted) {
                    ev.Player.SetRole(random.Next(0, 2) == 0 ? RoleType.Scp93953 : RoleType.Scp93989);
                    ev.Player.ShowHint("<color=#be00ff>На сервере автоматически проводится ивент Прятки</color>", 10);
                }
                else {
                    Vector3 DoorPosition = Door.Get(DoorType.HeavyContainmentDoor).Position;
                    ev.Player.SetRole(RoleType.ClassD);
                    ev.Player.Teleport(new Vector3(DoorPosition.x, -998, DoorPosition.z));
                }
            }
        }
        public override void OnEscaping(EscapingEventArgs ev)
        {
            if(IsEnded) return;
            IsEnded = true;
            Map.Broadcast(20, $"<color=orange>Победитель ивента</color> <color=#be00ff>{ev.Player.Nickname}</color>");
            Timing.CallDelayed(5, () => {
                foreach(Player player in Player.List.Where(pl => pl.Role == RoleType.ClassD)) {
                    player.SetRole(RoleType.Spectator);
                }
                Round.IsLocked = false;
            });
        }
        public override void OnDied(DiedEventArgs ev)
        {
            Random random = new Random();
            if(!IsStarted || IsEnded) return;
            if(ev.TargetOldRole == RoleType.ClassD) ev.Target.SetRole(random.Next(0, 2) == 0 ? RoleType.Scp93953 : RoleType.Scp93989);
            IEnumerable<Player> ClassD = Player.List.Where(pl => pl.Role == RoleType.ClassD);
            if(ClassD.Count() == 1) {
                IsEnded = true;
                Player ClassDPlayer = ClassD.ElementAt(0);
                Map.Broadcast(20, $"<color=orange>Победитель ивента</color> <color=#be00ff>{ClassDPlayer.Nickname}</color>");
                Timing.CallDelayed(5, () => {
                    ClassDPlayer.SetRole(RoleType.Spectator);
                    Round.IsLocked = false;
                });
            }
            else if(Player.List.Where(pl => pl.Role == RoleType.Scp93989 || pl.Role == RoleType.Scp93953).Count() == 0) {
                IsEnded = true;
                Map.Broadcast(20, "<color=orange>Победа Класса-Д\nВсе SCP-939 были уничтожены</color>");
                Timing.CallDelayed(5, () => Round.IsLocked = false);
            }
        }
        public override void OnLeft(LeftEventArgs ev)
        {
            Random random = new Random();
            if(!IsStarted && SCP939Player.UserId == ev.Player.UserId) {
                Timing.CallDelayed(1, () => {
                    SCP939Player = Player.List.ElementAt(random.Next(Player.List.Count()));
                    Methods.SpawnTutorial(SCP939Player);
                    SCP939Player.ShowHint("<color=#be00ff>Вы будете заспавнены за SCP-939 из-за выхода предыдущего игрока", 10);
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