using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pustalorc.Plugins.AutoTurnOff.Interactables.InteractableWrappers;
using Rocket.API;

namespace Pustalorc.Plugins.AutoTurnOff.Configuration
{
    public sealed class ReduceLagConfiguration : IRocketPluginConfiguration
    {
        public List<InteractableItem> Interactables { get; set; } = new List<InteractableItem>();

        public void LoadDefaults()
        {
            Interactables = GetInteractableItems();
        }

        private List<InteractableItem> GetInteractableItems()
        {
            return (from item in typeof(InteractableWrapper).Assembly.GetTypes()
                    .Where(c => c.IsSubclassOf(typeof(InteractableWrapper)) && c.IsSealed)
                select item.GetCustomAttribute<InteractableTypeAttribute>()
                into attribute
                where attribute != null
                select new InteractableItem {KeepEnabled = false, Name = attribute.Name}).ToList();
        }
    }
}