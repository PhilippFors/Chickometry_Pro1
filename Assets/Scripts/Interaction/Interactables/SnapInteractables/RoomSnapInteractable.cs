using Interactables;
using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class RoomSnapInteractable : RoomInteractable
    {
        public Transform attachPoint;
        [SerializeField] private SocketInteractable currentSocket;
        public override void OnPickup()
        {
            base.OnPickup();
            if (currentSocket) {
                currentSocket.socketOccupied = false;
                currentSocket = null;
            }
        }

        public override void OnUseWithInteractable(BaseInteractable interactable, InteractionManager manager)
        {
            if (interactable is SocketInteractable) {
                var socket = (SocketInteractable) interactable;
                if (socket.Activate(this, manager)) {
                    currentSocket = socket;
                    ReturnObject();
                }
            }
        }

        public override void MakeInvisible()
        {
            if (currentSocket && !currentSocket.socketOccupied) {
                currentSocket.socketOccupied = false;
            }
        }

        public override void MakeVisible()
        {
            var hits = Physics.OverlapSphere(transform.position, 0.5f, LayerMask.GetMask("Interactable"), QueryTriggerInteraction.Collide);
            foreach (var hit in hits) {
                var socket = hit.GetComponent<SocketInteractable>();
                if (socket) {
                    socket.AttachObject(this);
                }
            }
            
            if (currentSocket) {
                currentSocket.socketOccupied = true;
            }
        }
    }
}