using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;

namespace Pustalorc.Plugins.ReduceLag
{

    [InteractableType(typeof(InteractableSafezone), "Safezone")]
    public class InteractableSafezoneWrapper : InteractableWrapper
    {
        public InteractableSafezoneWrapper(string name, InteractableSafezone interactable) : base(name, interactable)
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
