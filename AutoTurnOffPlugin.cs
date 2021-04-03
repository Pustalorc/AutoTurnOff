using System;
using System.Linq;
using Pustalorc.Plugins.AutoTurnOff.Configuration;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff
{
    public class AutoTurnOffPlugin : RocketPlugin<AutoTurnOffConfiguration>
    {
        protected override void Load()
        {
            U.Events.OnPlayerDisconnected += Disconnected;
            Logger.Log("Auto Turn Off has been loaded!");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerDisconnected -= Disconnected;
            Logger.Log("Auto Turn Off has been unloaded!");
        }

        private void Disconnected(UnturnedPlayer player)
        {
            var playerId = player.CSteamID.m_SteamID;

            if (BarricadeManager.regions == null)
                return;

            var buildables = BarricadeManager.regions.Cast<BarricadeRegion>().Concat(BarricadeManager.vehicleRegions).SelectMany(brd => brd.drops).ToList().Where(k => Configuration.Instance.ItemsToDisable.Contains(k.asset.id)).ToList();

            foreach (BarricadeDrop build in buildables)
            {
                var interactable = build.interactable;
                var model = build.model;

                if (!BarricadeManager.tryGetInfo(interactable.transform, out var x, out var y, out var plant, out var index, out var region))
                    continue;

                switch (interactable)
                {
                    case InteractableSafezone saf:
                        BarricadeManager.ServerSetSafezonePowered(saf, false);
                        break;
                    case InteractableOxygenator oxy:
                        BarricadeManager.ServerSetOxygenatorPowered(oxy, false);
                        break;
                    case InteractableSpot spot:
                        BarricadeManager.ServerSetSpotPowered(spot, false);
                        break;
                    case InteractableGenerator gen:
                        BarricadeManager.ServerSetGeneratorPowered(gen, false);
                        break;
                    case InteractableFire fire:
                        BarricadeManager.ServerSetFireLit(fire, false);
                        break;
                    case InteractableOven oven:
                        BarricadeManager.ServerSetOvenLit(oven, false);
                        break;
                    case InteractableStereo stereo:
                        BarricadeManager.ServerSetStereoTrack(stereo, Guid.Empty);
                        break;
                }
            }

            Logger.Log($"Turned off {buildables.Count} barricades.");
        }
    }
}