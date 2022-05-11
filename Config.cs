namespace SmokyPlugin
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;
    public sealed class Config : IConfig {
        [Description("Включен плагин или нет")]
        public bool IsEnabled { get; set; } = true;

        [Description("Адрес API сервера")]
        public string ApiUrl { get; private set; } = "http://localhost:8560";
        [Description("API токен")]
        public string ApiToken { get; private set; } = "XXXXXXXX";


        [Description("Сообщение, которое отправляется вошедшим на сервер игрокам")]
        public string JoinMessage { get; private set; } = "Добро пожаловать!";

        [Description("Длительность сообщения вошедшим игрокам")]
        public ushort JoinMessageDuration { get; private set; } = 10;


        [Description("Перезапускать ли раунд, когда сервер пустой")]
        public bool RestartEmpty { get; private set; } = true;

        [Description("Интервал проверки")]
        public ushort RestartEmptyInterval { get; private set; } = 30;


        [Description("Включать ли огонь по своим в конце раунда")]
        public bool RoundEndFf { get; private set; } = true;
        
        [Description("Сообщение, которое отправляется после завершения раунда")]
        public string RoundEndBroadcast { get; private set; } = "Раунд завершен!";
        [Description("Длительность сообщения")]
        public ushort RoundEndBroadcastDuration { get; private set; } = 10;

        [Description("Таймер запуска раунда в лобби - отсчет времени")]
        public string LobbyTimerCountdown { get; private set; } = "Раунд начнется через {time} секунд";
        [Description("Таймер запуска раунда в лобби - запуск приостановлен")]
        public string LobbyTimerRoundPaused { get; private set; } = "Запуск раунда приостановлен";
        [Description("Таймер запуска раунда в лобби - раунд начинается")]
        public string LobbyTimerRoundStarting { get; private set; } = "Раунд начинается!";
        [Description("Таймер запуска раунда в лобби - подключилось игроков")]
        public string LobbyTimerPlayersConnected { get; private set; } = "{players} игроков подключилось";
        [Description("Таймер запуска раунда в лобби - автоматический ивент")]
        public string LobbyTimerAutoEvent { get; private set; } = "В этом раунде автоматически проводится ивент {event}";

        [Description("Сообщение, которое увидит игрок, если попробует вызвать заблокированный лифт")]
        public string ElevatorLockedDownHint { get; private set; } = "Лифт заблокирован";
        [Description("Длительность сообщения")]
        public ushort ElevatorLockedDownHintDuration { get; private set; } = 5;

        [Description("Через сколько секунд после начала раунда автоматически активировать боеголовку")]
        public ushort AutoWarheadTime { get; private set; } = 1200;
        [Description("Сообщение, которое отправляется до начала детонации боеголовки")]
        public string AutoWarheadMessageBeforeDetonation { get; private set; } = "Альфа боеголовка будет запущена автоматически через {time} секунд";
        [Description("За сколько секунд до детонации отправлять сообщение")]
        public ushort AutoWarheadMessageBeforeDetonationTime { get; private set; } = 30;
        [Description("Сообщение о начале детонации боеголовки")]
        public string AutoWarheadDetonationMessage { get; private set; } = "Альфа боеголовка запущена автоматически. Детонация необратима";
        [Description("Длительность сообщения")]
        public ushort AutoWarheadDetonationMessageDuration { get; private set; } = 10;
        [Description("Сообщение, которое увидит игрок, если попробует отключить заблокированную боеголовку")]
        public string WarheadLockedHint { get; private set; } = "Боеголовка заблокирована";
        [Description("Длительность сообщения")]
        public ushort WarheadLockedHintDuration { get; private set; } = 5;

        [Description("Сообщение всем игрокам во время блокировки лобби для проведения ивента")]
        public string EventLockMessage { get; private set; } = "В этом раунде проводится ивент. Послушайте его правила";
        [Description("Длительность сообщения")]
        public ushort EventLockMessageDuration { get; private set; } = 10;
    }
}
