using System.Linq;
using Interactables;
using UnityEngine;

namespace Interaction.Interactables
{
    public class SocketInteractable : BaseInteractable
    {
        [SerializeField] private Transform socket;
        [SerializeField] private BaseInteractable[] placeableItems;
        [SerializeField] private BaseInteractable keyItem;
        public bool socketOccupied;
        public bool keyInSocket;
        public virtual bool Activate(SnapInteractable interactable, InteractionManager manager)
        {
            if (placeableItems.Contains(interactable) && !socketOccupied) {
                var attachPoint = interactable.attachPoint;
                interactable.transform.rotation = socket.rotation;
                interactable.transform.position = socket.position + (interactable.transform.position - attachPoint.position);
                manager.ReleaseObject();
                var rb = interactable.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                
                if (interactable && interactable == keyItem) {
                    interactable.canBePickedUp = false;
                    keyInSocket = true;
                }

                return true;
            }

            return false;
        }
    }
}