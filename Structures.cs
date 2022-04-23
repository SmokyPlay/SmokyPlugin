namespace SmokyPlugin.Structures
{
    public class RoundDuration : Interfaces.RoundDuration
    {
        public double ElapsedTime { get; set; } = 0;
        public string CurrentUnit { get; set; } = "none";
    }
}