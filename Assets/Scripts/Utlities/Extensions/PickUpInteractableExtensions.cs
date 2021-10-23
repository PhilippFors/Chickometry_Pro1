using Interactables;
using Interaction.Items;
using UnityEngine;

namespace Utlities
{
    public static class PickUpInteractableExtensions
    {
        public static T GetComponent<T>(this IPickUpInteractable interactable) where T : Component
        {
            var item = (Component) interactable;
            return item.GetComponent<T>();
        }
    }
}
