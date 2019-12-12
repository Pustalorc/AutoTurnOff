using System;
using System.Linq;
using Pustalorc.Plugins.AutoTurnOff.Configuration;
using Pustalorc.Plugins.AutoTurnOff.Interactables;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff
{
    public class ReduceLagPlugin : RocketPlugin<ReduceLagConfiguration>
    {
        public IInteractableWrapperHandler InteractableWrapperHandler { get; } = new InteractableWrapperHandler();

        protected override void Load()
        {
            U.Events.OnPlayerDisconnected += Disconnected;

            Logger.Log("Reduce Lag by Pustalorc has been loaded!");
        }

        protected override void Unload()
        {
            U.Events.OnPlayerDisconnected -= Disconnected;

            Logger.Log("Reduce Lag by Pustalorc has been unloaded!");
        }

        private void Disconnected(UnturnedPlayer player)
        {
            var playerId = player.CSteamID.m_SteamID;
            var disables = 0;

            if (BarricadeManager.regions == null)
                return;

            foreach (var region in BarricadeManager.regions)
            {
                if (region == null)
                    continue;

                foreach (var interactable in from drop in region.drops where drop != null select drop.interactable)
                {
                    if (interactable == null ||
                        !BarricadeManager.tryGetInfo(interactable.transform, out _, out _, out _, out var index,
                            out _) || region.barricades[index] == null || region.barricades[index].owner != playerId ||
                        region.barricades[index].barricade == null ||
                        region.barricades[index].barricade.state == null)
                        continue;

                    var wrapper = InteractableWrapperHandler.GetInteractableWrapper(interactable);

                    if (!Configuration.Instance.Interactables.Any(c => !c.IsEnabled &&
                                                                       c.Name.Equals(wrapper.Name,
                                                                           StringComparison
                                                                               .InvariantCultureIgnoreCase)))
                        continue;

                    wrapper.SetActive(false);

                    disables++;
                }
            }

            Logger.Log($"Turned off or powered off {disables} barricades.");
        }
    }
}