using Checkpoints;
using Interaction.Items;
using ObjectAbstraction.ModelChanger;
using RoomLoop.Portal;
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

        public static Transform GetTransform(this IPortalTraveller traveller)
        {
            var item = (Component) traveller;
            return item.transform;
        }

        public static T GetComponent<T>(this IPortalTraveller traveller) where T : Component
        {
            var item = (Component) traveller;
            return item.GetComponent<T>();
        }

        public static T GetComponent<T>(this IResettableBehaviour behaviour) where T : Component
        {
            var item = (Component) behaviour;
            return item.GetComponent<T>();
        }
    }
}
