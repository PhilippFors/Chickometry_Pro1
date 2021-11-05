using System;
using Interactables;
using Interaction.Items;
using ObjectAbstraction;
using UnityEngine;

namespace Utlities
{
    public static class PickUpInteractableExtensions
    {
        public static T GetIComponent<T, Y>(this Y interactable) where T : Component 
                                                                where Y : notnull
        {
            var item = (Component) Convert.ChangeType(interactable, typeof(Y));
            return item.GetComponent<T>();
        }
    }
}
