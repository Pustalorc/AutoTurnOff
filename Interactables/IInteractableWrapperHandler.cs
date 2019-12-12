using Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers;
using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff.Interactables
{
    public interface IInteractableWrapperHandler
    {
        InteractableWrapper GetInteractableWrapper(Interactable interactable);
    }
}