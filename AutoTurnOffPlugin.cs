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
                        if (saf.isPowered)
                            BarricadeManager.toggleSafezone(model);
                        break;
                    case InteractableOxygenator oxy:
                        if (oxy.isPowered)
                            BarricadeManager.toggleOxygenator(model);
                        break;
                    case InteractableSpot spot:
                        if (spot.isPowered)
                            BarricadeManager.toggleSpot(model);
                        break;
                    case InteractableGenerator gen:
                        if (gen.isPowered)
                            BarricadeManager.toggleGenerator(model);
                        break;
                    case InteractableFire fire:
                        if (fire.isLit)
                            BarricadeManager.toggleFire(model);
                        break;
                    case InteractableOven oven:
                        if (oven.isLit)
                            BarricadeManager.toggleOven(model);
                        break;
                    case InteractableStereo stereo:
                        BarricadeManager.ServerSetStereoTrack(stereo, Guid.Empty);
                        break;
                }
            }

            Logger.Log($"Turned off (or verified that they are off) {buildables.Count} barricades.");
        }
    }
}