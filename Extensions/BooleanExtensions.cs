namespace Pustalorc.Plugins.AutoTurnOff.Extensions
{
    public static class BooleanExtensions
    {
        public static unsafe byte ToByte(this bool source)
            => *((byte*)(&source));
    }
}