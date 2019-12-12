using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers
{
    [InteractableType(typeof(InteractableFire), "Fire")]
    public sealed class InteractableFireWrapper : InteractableWrapper
    {
        public InteractableFireWrapper(string name, Interactable interactable) : base(name, interactable)
        {
        }

        public override void SetActive(bool value)
        {
            if (!BarricadeManager.tryGetInfo(Interactable.transform, out var x, out var y,
                out var plant, out var index, out var region))
                ThrowInvalidInteractable();

            if (plant == ushort.MaxValue)
                BarricadeManager.instance.channel.send("tellToggleFire", ESteamCall.ALL, x, y,
                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                    plant,
                    index, false);
            else
                BarricadeManager.instance.channel.send("tellToggleFire", ESteamCall.ALL,
                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, false);

            region.barricades[index].barricade.state[0] = 0;
        }
    }
}