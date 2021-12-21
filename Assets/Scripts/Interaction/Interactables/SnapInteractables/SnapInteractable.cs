using Interactables;
using Interaction.Items;
using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class SnapInteractable : BasePickUpInteractable
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
                }
            }
        }
    }
}