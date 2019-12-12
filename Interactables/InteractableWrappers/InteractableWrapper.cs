using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustalorc.Plugins.ReduceLag
{
    public abstract class InteractableWrapper
    {
        public string Name { get; }
        public Interactable Interactable { get; set; }
        
        protected InteractableWrapper(string name, Interactable interactable)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Interactable = interactable ?? throw new ArgumentNullException(nameof(interactable));
        }
        public abstract void SetActive(bool value);

        protected void ThrowInvalidInteractable()
            => throw new Exception($"Invalid interactable: {nameof(Interactable)}!");
    }
}
