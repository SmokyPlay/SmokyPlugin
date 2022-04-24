using Exiled.API.Enums;

namespace SmokyPlugin.Interfaces
{
    public interface RoundDuration
    {
        double ElapsedTime { get; set; }
        string CurrentUnit { get; set; }
    }
}