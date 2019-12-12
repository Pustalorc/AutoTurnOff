using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustalorc.Plugins.ReduceLag
{
    public sealed class InteractableTypeAttribute : Attribute
    {
        public Type InteractableType { get; set; }
        public string Name { get; set; }
        public InteractableTypeAttribute(Type interactableType, string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            if (!interactableType?.IsSubclassOf(typeof(Interactable)) ?? throw new ArgumentNullException(nameof(interactableType)))
                throw new ArgumentException($"{nameof(interactableType)} is not a sub class of {nameof(Interactable)}");
            this.InteractableType = interactableType;
        }
    }
}
