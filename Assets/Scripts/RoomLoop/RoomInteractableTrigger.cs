using Interaction.Interactables;
using UnityEngine;

namespace RoomLoop
{
    [RequireComponent(typeof(BoxCollider))]
    public class RoomInteractableTrigger : MonoBehaviour
    { 
        [SerializeField] private Transform originalParent;
        [SerializeField] private bool isAbstractRoom;

        private void OnTriggerEnter(Collider other)
        {
            var roomObject = other.GetComponent<RoomInteractable>();
            if (roomObject) {
                roomObject.currentParent = originalParent;
                if (isAbstractRoom && roomObject.isAbstract) {
                    roomObject.isInOriginalRoom = true;
                }
                else if (!isAbstractRoom && !roomObject.isAbstract) {
                    roomObject.isInOriginalRoom = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var roomObject = other.GetComponent<RoomInteractable>();
            if (roomObject) {
                if (isAbstractRoom && roomObject.isAbstract) {
                    roomObject.isInOriginalRoom = false;
                }
                else if (!isAbstractRoom && !roomObject.isAbstract) {
                    roomObject.isInOriginalRoom = false;
                }
            }
        }
    }
}