using Pustalorc.Plugins.AutoTurnOff.Extensions;
using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers
{
    [InteractableType(typeof(InteractableSafezone), "Safezone")]
    public sealed class InteractableSafezoneWrapper : InteractableWrapper
    {
        public InteractableSafezoneWrapper(string name, Interactable interactable) : base(name, interactable)
        {
        }

        public override void SetActive(bool value)
        {
            if (!BarricadeManager.tryGetInfo(Interactable.transform, out var x, out var y,
                out var plant, out var index, out var region))
                ThrowInvalidInteractable();

            if (plant == ushort.MaxValue)
                BarricadeManager.instance.channel.send("tellToggleSafezone", ESteamCall.ALL, x, y,
                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                    plant,
                    index, value);
            else
                BarricadeManager.instance.channel.send("tellToggleSafezone", ESteamCall.ALL,
                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, value);

            region.barricades[index].barricade.state[0] = value.ToByte();
            EffectManager.sendEffect(8, EffectManager.SMALL, Interactable.transform.position);
        }
    }
}