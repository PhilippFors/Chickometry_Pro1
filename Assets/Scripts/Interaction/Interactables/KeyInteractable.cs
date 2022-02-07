using UnityEngine;

namespace Interaction.Interactables
{
    public class KeyInteractable : RoomSnapInteractable
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
                isInteractable = false;
                pattern = InteractionPattern.PickUp;
                var rb = GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                roomPuzzle.SyncPair(this);
            }
            else if(keySocket.socketOccupied){
                Debug.Log("Playing something");
            }
            else {
                Debug.Log("Penis");
            }
        }

        public override void MakeVisible()
        {
        }

        public override void Sync(RoomInteractable interactable)
        {
            canBePickedUp = true;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}