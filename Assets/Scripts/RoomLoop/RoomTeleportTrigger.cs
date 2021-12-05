using Entities.Player;
using UnityEngine;

namespace RoomLoop
{
    public class RoomTeleportTrigger : MonoBehaviour
    {
        public bool isEnabled = true;
        [SerializeField] private bool forwardDoor;
        [SerializeField] private RoomTeleporter roomConnector;
        [SerializeField] private RoomTeleportTrigger opposite;
        
        private Room room;
        private bool hasPassed;

        private void Start()
        {
            room = GetComponentInParent<Room>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnabled) {
                isEnabled = true;
                return;
            }

            var player = other.GetComponentInParent<PlayerMovement>();
            if (player) {
                if (forwardDoor && roomConnector.RoomBackward != room) {
                    opposite.isEnabled = !roomConnector.Teleport(forwardDoor);
                }
                else if (!forwardDoor && roomConnector.RoomForward != room) {
                    opposite.isEnabled = !roomConnector.Teleport(forwardDoor);
                }
            }
        }
    }
}