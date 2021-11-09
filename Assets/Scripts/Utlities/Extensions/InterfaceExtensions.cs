using Interaction.Items;
using ObjectAbstraction;
using UnityEngine;

namespace Utlities
{
    public static class InterfaceExtensions
    {
        public static T GetComponent<T>(this IPickUpInteractable interactable) where T : Component
        {
            var item = (Component) interactable;
            return item.GetComponent<T>();
        }
        
        public static T GetComponent<T>(this IModelChanger interactable) where T : Component
        {
            var item = (Component) interactable;
            return item.GetComponent<T>();
        }
    }
}
