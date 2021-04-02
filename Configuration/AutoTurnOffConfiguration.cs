using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rocket.API;

namespace Pustalorc.Plugins.AutoTurnOff.Configuration
{
    public sealed class AutoTurnOffConfiguration : IRocketPluginConfiguration
    {
        public List<ushort> ItemsToDisable;

        public void LoadDefaults()
        {
            ItemsToDisable = new List<ushort>
            {
                458, 1230, 362, 459, 1050, 1466, 1250, 1261, 359, 360, 361, 1049, 1222, 
            };
        }
    }
}