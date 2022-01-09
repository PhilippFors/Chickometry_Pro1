using UnityEngine;

namespace Interaction.Interactables
{
    public class KeyEndPart : RoomSnapInteractable
    {
        public bool realKey;
        [SerializeField] private SocketInteractable keySocket;
        
        public override void OnInteract()
        {
            if (!isInteractable) {
                return;
            }
            
            if (keySocket.keyInSocket) {
                Debug.Log("You got a key part");
                canBePickedUp = true;
                pattern = InteractionPattern.PickUp;
                var rb = GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
            }
            else if(keySocket.socketOccupied){
                Debug.Log("Playing something");
            }
            else {
                Debug.Log("Penis");
            }
        }
    }
}