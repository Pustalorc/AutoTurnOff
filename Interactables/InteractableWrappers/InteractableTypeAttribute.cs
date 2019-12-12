using System;
using SDG.Unturned;

namespace Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers
{
    public sealed class InteractableTypeAttribute : Attribute
    {
        public Type InteractableType { get; set; }
        public string Name { get; set; }

        public InteractableTypeAttribute(Type interactableType, string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            if (!interactableType?.IsSubclassOf(typeof(Interactable)) ??
                throw new ArgumentNullException(nameof(interactableType)))
                throw new ArgumentException($"{nameof(interactableType)} is not a sub class of {nameof(Interactable)}");

            InteractableType = interactableType;
        }
    }
}