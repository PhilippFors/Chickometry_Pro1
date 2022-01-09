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
                var attachPoint = interactable.attachPoint;
                interactable.transform.rotation = socket.rotation;
                interactable.transform.position =
                    socket.position + (interactable.transform.position - attachPoint.position);
                manager.ReleaseObject();
                var rb = interactable.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                if (interactable && interactable == keyItem) {
                    interactable.canBePickedUp = false;
                    keyInSocket = true;
                }

                socketOccupied = true;
                socketActivated?.Invoke(this);
                return true;
            }

            return false;
        }
    }
}