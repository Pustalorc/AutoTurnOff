using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustalorc.Plugins.ReduceLag
{
    [InteractableType(typeof(InteractableOxygenator), "Oxygenator")]
    public sealed class InteractableOxygenatorWrapper : InteractableWrapper
    {
        public InteractableOxygenatorWrapper(string name, InteractableOxygenator interactable) : base(name, interactable)
        {

        }

        public override void SetActive(bool value)
        {
            if (!BarricadeManager.tryGetInfo(Interactable.transform, out var x, out var y,
                out var plant, out var index, out var region))
                ThrowInvalidInteractable();

            if (plant == ushort.MaxValue)
                BarricadeManager.instance.channel.send("tellToggleOxygenator", ESteamCall.ALL, x, y,
                    BarricadeManager.BARRICADE_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y,
                    plant,
                    index, value);
            else
                BarricadeManager.instance.channel.send("tellToggleOxygenator", ESteamCall.ALL,
                    ESteamPacket.UPDATE_RELIABLE_BUFFER, x, y, plant, index, value);

            region.barricades[index].barricade.state[0] = value.ToByte();
            EffectManager.sendEffect(8, EffectManager.SMALL, Interactable.transform.position);
        }
    }
}
