using System;
using System.Linq;
using JetBrains.Annotations;
using Pustalorc.Plugins.AutoTurnOff.Configuration;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;

namespace Pustalorc.Plugins.AutoTurnOff
{
    // This references the original objects. Changes done to them should be reflected in the property accessors here.
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

        private void Disconnected([NotNull] UnturnedPlayer player)
        {
            var playerId = player.CSteamID.m_SteamID;

            if (BarricadeManager.regions == null)
                return;

            var regions = BarricadeManager.regions.Cast<BarricadeRegion>().Concat(BarricadeManager.vehicleRegions)
                .ToList();
            var drops = regions.SelectMany(k => k.drops).ToList();
            var data = regions.SelectMany(k => k.barricades).ToList();
            var buildables = data.Select((k, i) =>
            {
                if (!Configuration.Instance.ItemsToDisable.Contains(k.barricade.asset.id))
                    return null;

                if (k.owner != playerId)
                    return null;

                var drop = drops.ElementAt(i);
                return drop == null ? null : new Buildable(k, drop);
            }).Where(k => k != null).ToList();

            foreach (var interactable in buildables.Select(build => build.Interactable))
            {
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