using Entities.Player;
using UnityEngine;

namespace RoomLoop
{
    public class RoomConnectorTrigger : MonoBehaviour
    {
        public bool isEnabled = true;
        public bool forwardDoor;
        [SerializeField] private RoomConnector roomConnector;
        [SerializeField] private RoomConnectorTrigger opposite;
        [SerializeField] private Room room;
        private bool hasPassed;
    
        private void OnTriggerEnter(Collider other)
        {
            if (!isEnabled) {
                isEnabled = true;
                return;
            }

            var player = other.GetComponentInParent<PlayerMovement>();
            if (player) {
                if (forwardDoor && roomConnector.roomBackward != room) {
                    opposite.isEnabled = !roomConnector.Teleport(forwardDoor);
                }
                else if (!forwardDoor && roomConnector.roomForward != room) {
                    opposite.isEnabled = !roomConnector.Teleport(forwardDoor);
                }
            }
        }
    }
}