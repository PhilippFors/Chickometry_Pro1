using Interactables;
using Interaction.Interactables;
using UnityEngine;

namespace Interaction.Items
{
    /// <summary>
    /// An item that is literally a key to a door or a similiar use case.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RoomLoopKey : RoomSnapInteractable
    {
        [SerializeField] public GameObject door;
        public override void OnInteract()
        {
        }

        public override void OnUseWithInteractable(BaseInteractable interactable, InteractionManager manager)
        {
            
        }
    }
}
