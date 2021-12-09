using Entities.Player;
using UnityEngine;

namespace RoomLoop
{
    public class RoomTeleportTrigger : MonoBehaviour
    {
        public bool isEnabled = true;
        [SerializeField] private bool forwardDoor;
        [SerializeField] private RoomTeleportTrigger opposite;
        
        private Room room;
        private RoomTeleporter roomTeleporter;

        private void Start()
        {
            room = GetComponentInParent<Room>();
            roomTeleporter = GetComponentInParent<RoomTeleporter>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isEnabled) {
                isEnabled = true;
                return;
            }

            var player = other.GetComponentInParent<PlayerMovement>();
            if (player) {
                if (forwardDoor && roomTeleporter.RoomBackward != room) {
                    opposite.isEnabled = !roomTeleporter.Teleport(forwardDoor);
                }
                else if (!forwardDoor && roomTeleporter.RoomForward != room) {
                    opposite.isEnabled = !roomTeleporter.Teleport(forwardDoor);
                }
            }
        }
    }
}