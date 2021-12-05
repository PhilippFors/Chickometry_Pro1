using UnityEngine;

namespace RoomLoop
{
    [RequireComponent(typeof(BoxCollider))]
    public class RoomTrigger : MonoBehaviour
    {
        [SerializeField] private Transform originalParent;
        [SerializeField] private RoomPuzzle roomPuzzle;
        [SerializeField] private bool isAbstractRoom;

        private void OnTriggerEnter(Collider other)
        {
            var roomObject = other.GetComponent<RoomInteractable>();
            if (roomObject) {
                roomObject.currentParent = originalParent;
                if (isAbstractRoom && roomObject.isAbstract) {
                    roomPuzzle.ReturnObject(roomObject);
                    roomObject.isInOriginalRoom = true;
                }
                else if (!isAbstractRoom && !roomObject.isAbstract) {
                    roomPuzzle.ReturnObject(roomObject);
                    roomObject.isInOriginalRoom = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var roomObject = other.GetComponent<RoomInteractable>();
            if (roomObject) {
                if (isAbstractRoom && roomObject.isAbstract) {
                    roomPuzzle.RemoveObject(roomObject);
                    roomObject.isInOriginalRoom = false;
                }
                else if (!isAbstractRoom && !roomObject.isAbstract) {
                    roomPuzzle.RemoveObject(roomObject);
                    roomObject.isInOriginalRoom = false;
                }
            }
        }
    }
}