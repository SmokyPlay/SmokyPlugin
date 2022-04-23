namespace SmokyPlugin
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;
    public sealed class Config : IConfig {
        [Description("Включен плагин или нет")]
        public bool IsEnabled { get; set; } = true;


        [Description("Сообщение, которое отправляется вошедшим на сервер игрокам")]
        public string JoinMessage { get; private set; } = "Добро пожаловать!";

        [Description("Длительность сообщения вошедшим игроам")]
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
    }
}
