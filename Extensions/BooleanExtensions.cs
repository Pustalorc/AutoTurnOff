namespace Pustalorc.Plugins.AutoTurnOff.Extensions
{
    public static class BooleanExtensions
    {
        public static byte ToByte(this bool source)
        {
            return source == false ? (byte) 0 : (byte) 1;
        }
    }
}