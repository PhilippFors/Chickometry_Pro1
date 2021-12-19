using Interactables;
using Interaction.Items;
using RoomLoop;
using UnityEngine;

namespace Interaction.Interactables
{
    public class SnapInteractable : BasePickUpInteractable
    {
        public Transform attachPoint;
        public bool inSocket;

        public override void OnUse(BaseInteractable interactable, InteractionManager manager)
        {
            if (interactable is SocketInteractable) {
                var socket = (SocketInteractable) interactable;
                socket.Activate(this, manager);
            }
        }
    }
}