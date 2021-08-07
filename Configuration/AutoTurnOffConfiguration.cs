using System;
using System.Collections.Generic;
using Rocket.API;

namespace Pustalorc.Plugins.AutoTurnOff.Configuration
{
    [Serializable]
    public sealed class AutoTurnOffConfiguration : IRocketPluginConfiguration
    {
        public HashSet<ushort> ItemsToDisable;

        public void LoadDefaults()
        {
            ItemsToDisable = new HashSet<ushort>
            {
                458, 1230, 362, 459, 1050, 1466, 1250, 1261, 359, 360, 361, 1049, 1222
            };
        }
    }
}