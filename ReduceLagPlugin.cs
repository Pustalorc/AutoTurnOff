using System.Linq;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace Pustalorc.Plugins.ReduceLag
{
    public class ReduceLagPlugin : RocketPlugin
    {
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

            if (BarricadeManager.regions == null) return;

            foreach (var region in BarricadeManager.regions)
            {
                if (region == null) continue;

                foreach (var interactable in from drop in region.drops where drop != null select drop.interactable)
                {
                    if (interactable == null ||
                        !BarricadeManager.tryGetInfo(interactable.transform, out var x, out var y, out var plant,
                            out var index, out _) || region.barricades[index] == null ||
                        region.barricades[index].owner != playerId || region.barricades[index].barricade == null ||
                        region.barricades[index].barricade.state == null)
                        continue;

                    switch (interactable)
                    {
                        case InteractableFire fire:
                        {
                            if (fire != null && !fire.isLit) continue;

                            if (plant == ushort.MaxValue)
                                BarricadeManager.instance.channel.send("tellToggleFire", ESteamCall.ALL, x, y,
                                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                                    plant,
                                    index, false);
                            else
                                BarricadeManager.instance.channel.send("tellToggleFire", ESteamCall.ALL,
                                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

                            region.barricades[index].barricade.state[0] = 0;
                            ++disables;
                            break;
                        }

                        case InteractableGenerator generator:
                        {
                            if (generator != null && !generator.isPowered) continue;

                            if (plant == ushort.MaxValue)
                                BarricadeManager.instance.channel.send("tellToggleGenerator", ESteamCall.ALL, x, y,
                                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                                    plant,
                                    index, false);
                            else
                                BarricadeManager.instance.channel.send("tellToggleGenerator", ESteamCall.ALL,
                                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

                            region.barricades[index].barricade.state[0] = 0;
                            EffectManager.sendEffect(8, EffectManager.SMALL, interactable.transform.position);
                            ++disables;
                            break;
                        }

                        case InteractableSpot spotlight:
                        {
                            if (spotlight != null && !spotlight.isPowered) continue;

                            if (plant == ushort.MaxValue)
                                BarricadeManager.instance.channel.send("tellToggleSpot", ESteamCall.ALL, x, y,
                                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                                    plant,
                                    index, false);
                            else
                                BarricadeManager.instance.channel.send("tellToggleSpot", ESteamCall.ALL,
                                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

                            region.barricades[index].barricade.state[0] = 0;
                            EffectManager.sendEffect(8, EffectManager.SMALL, interactable.transform.position);
                            ++disables;
                            break;
                        }

                        case InteractableOven oven:
                        {
                            if (oven != null && !oven.isLit) continue;

                            if (plant == ushort.MaxValue)
                                BarricadeManager.instance.channel.send("tellToggleOven", ESteamCall.ALL, x, y,
                                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                                    plant,
                                    index, false);
                            else
                                BarricadeManager.instance.channel.send("tellToggleOven", ESteamCall.ALL,
                                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

                            region.barricades[index].barricade.state[0] = 0;
                            ++disables;
                            break;
                        }

                        case InteractableOxygenator oxygenator:
                        {
                            if (oxygenator != null && !oxygenator.isPowered) continue;

                            if (plant == ushort.MaxValue)
                                BarricadeManager.instance.channel.send("tellToggleOxygenator", ESteamCall.ALL, x, y,
                                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                                    plant,
                                    index, false);
                            else
                                BarricadeManager.instance.channel.send("tellToggleOxygenator", ESteamCall.ALL,
                                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

                            region.barricades[index].barricade.state[0] = 0;
                            EffectManager.sendEffect(8, EffectManager.SMALL, interactable.transform.position);
                            ++disables;
                            break;
                        }

                        case InteractableSafezone safezone:
                        {
                            if (safezone != null && !safezone.isPowered) continue;

                            if (plant == ushort.MaxValue)
                                BarricadeManager.instance.channel.send("tellToggleSafezone", ESteamCall.ALL, x, y,
                                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                                    plant,
                                    index, false);
                            else
                                BarricadeManager.instance.channel.send("tellToggleSafezone", ESteamCall.ALL,
                                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

                            region.barricades[index].barricade.state[0] = 0;
                            EffectManager.sendEffect(8, EffectManager.SMALL, interactable.transform.position);
                            ++disables;
                            break;
                        }
                    }
                }
            }

            Logger.Log($"Turned off or powered off {disables} barricades.");
        }
    }
}