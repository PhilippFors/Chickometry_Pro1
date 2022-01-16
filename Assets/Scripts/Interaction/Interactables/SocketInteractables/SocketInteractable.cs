using System.Linq;
using Interactables;
using UnityEngine;

namespace Interaction.Interactables
{
    public class SocketInteractable : BaseInteractable
    {
        public event System.Action<SocketInteractable> socketActivated;
        
        public bool socketOccupied;
        public bool keyInSocket;

        [SerializeField] private Transform socket;
        [SerializeField] private BaseInteractable[] placeableItems;
        [SerializeField] private BaseInteractable keyItem;

        public virtual bool Activate(RoomSnapInteractable interactable, InteractionManager manager)
        {
            if (placeableItems.Contains(interactable) && !socketOccupied) {
                manager.ReleaseObject();
                AttachObject(interactable);
                return true;
            }

            return false;
        }

        public virtual void AttachObject(RoomSnapInteractable interactable)
        {
            var currentParent = interactable.transform.parent;
            
            if (placeableItems.Contains(interactable) && !socketOccupied) {
                interactable.transform.parent = null;
                var attachPoint = interactable.attachPoint;
                interactable.transform.rotation = socket.rotation;
                interactable.transform.position = socket.position + (interactable.transform.position - attachPoint.position);

                var rb = interactable.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                if (interactable && interactable == keyItem) {
                    interactable.canBePickedUp = false;
                    keyInSocket = true;
                }

                socketOccupied = true;

                socketActivated?.Invoke(this);
            }
            else {
                interactable.transform.parent = currentParent;
            }
        }
    }
}