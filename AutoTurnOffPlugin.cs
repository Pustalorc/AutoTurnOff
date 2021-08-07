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
    public sealed class AutoTurnOffPlugin : RocketPlugin<AutoTurnOffConfiguration>
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

            var buildables = BarricadeManager.regions.Cast<BarricadeRegion>().Concat(BarricadeManager.vehicleRegions)
                .SelectMany(k => k.drops)
                .Select(k =>
                    !Configuration.Instance.ItemsToDisable.Contains(k.asset.id) ||
                    k.GetServersideData().owner != playerId
                        ? null
                        : k.interactable).Where(k => k != null).ToList();

            foreach (var interactable in buildables)
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

            Logger.Log($"Turned off {buildables.Count} barricades.");
        }
    }
}