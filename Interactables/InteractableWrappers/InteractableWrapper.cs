using System;
using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers
{
    public abstract class InteractableWrapper
    {
        public string Name { get; }
        public Interactable Interactable { get; set; }

        protected InteractableWrapper(string name, Interactable interactable)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Interactable = interactable ? interactable : throw new ArgumentNullException(nameof(interactable));
        }

        public abstract void SetActive(bool value);

        protected void ThrowInvalidInteractable()
        {
            throw new Exception($"Invalid interactable: {nameof(Interactable)}!");
        }
    }
}