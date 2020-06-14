using SDG.Unturned;
using System;

namespace Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers
{
    [InteractableType(typeof(InteractableStereo), "Stereo")]
    public sealed class InteractableStereoWrapper : InteractableWrapper
    {
        public InteractableStereoWrapper(string name, Interactable interactable) : base(name, interactable)
        {
        }

        public override void SetActive(bool value)
        {
            if (!BarricadeManager.tryGetInfo(Interactable.transform, out var x, out var y,
                out var plant, out var index, out var region))
                ThrowInvalidInteractable();

			if (plant == 65535)
			{
				BarricadeManager.instance.channel.send("tellUpdateStereoTrack", ESteamCall.ALL, x, y, BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
							x,
							y,
							plant,
							index,
							null
				});
			}
			else
			{
				BarricadeManager.instance.channel.send("tellUpdateStereoTrack", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
							x,
							y,
							plant,
							index,
							null
				});
			}
			byte[] state = region.barricades[index].barricade.state;
			Array.Clear(state, 0, state.Length);
		}
    }
}
