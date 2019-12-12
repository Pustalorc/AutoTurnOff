using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pustalorc.Plugins.ReduceLag
{
    public interface IInteractableWrapperHandler
    {
        InteractableWrapper GetInteractableWrapper(Interactable interactable);
    }
    public class InteractableWrapperHandler : IInteractableWrapperHandler
    {
        private ICollection<InteractableTypeEntry> interactableTypes = new List<InteractableTypeEntry>();


        public void FindInteractableTypes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes().Where(c => !c.IsAbstract &&
             c.IsSubclassOf(typeof(InteractableWrapper))))
            {
                var attribute = type.GetCustomAttribute<InteractableTypeAttribute>();
                if (attribute == null)
                    throw new Exception($"InteractableWrapper descendant: {type.Name} does not have the {nameof(InteractableTypeAttribute)} attribute!");


                interactableTypes.Add(new InteractableTypeEntry()
                {
                    Attribute = attribute,
                    InteractableWrapperType = type
                });
            }
        }

        public InteractableWrapper GetInteractableWrapper(Interactable interactable)
        {
            var value = interactableTypes.FirstOrDefault(c => c.Attribute.InteractableType == interactable.GetType());
            if(value == null)
                throw new NotImplementedException($"Interactable type: {interactable.GetType().Name} does not have an implemented wrapper!");

            return (InteractableWrapper)Activator.CreateInstance(value.InteractableWrapperType, value.Attribute.Name, interactable);
        }

        private class InteractableTypeEntry
        {
            public InteractableTypeAttribute Attribute { get; set; }
            public Type InteractableWrapperType { get; set; }
        }
    }
}
