using Interactables;
using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class SocketInteractable : BaseInteractable
    {
        [SerializeField] private Transform socket;
        [SerializeField] private BaseInteractable keyInteractable;
        
        public virtual void Activate(SnapInteractable interactable, InteractionManager manager)
        {
            if (interactable == keyInteractable) {
                var attachPoint = interactable.attachPoint;
                interactable.transform.rotation = socket.rotation;
                interactable.transform.position = socket.position + (interactable.transform.position - attachPoint.position);
                interactable.canBePickedUp = false;
                manager.ReleaseObject();
                var rb = interactable.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }   
        }
    }
}