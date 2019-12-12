using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;

namespace Pustalorc.Plugins.ReduceLag
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
            List<InteractableItem> result = new List<InteractableItem>();
            foreach(var item in typeof(InteractableWrapper).Assembly.GetTypes()
                .Where(c => c.IsSubclassOf(typeof(InteractableWrapper)) &&
                c.IsAbstract))
            {
                var attribute = item.GetCustomAttribute<InteractableTypeAttribute>();
                if (attribute == null)
                    continue;

                result.Add(new InteractableItem()
                {
                    IsEnabled = true,
                    Name = attribute.Name
                });
            }
            return result;
        }
    }
}
