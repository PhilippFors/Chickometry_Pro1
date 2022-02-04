using Entities.Companion;
using UnityEngine;

namespace Interaction.Interactables
{
    public class DoorTrigger : MonoBehaviour
    {
        private bool contested = false;

        public Door door;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<Gudrun>()) {
                door.OpenDoor();
                // contested = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<Gudrun>()) {
                door.CloseDoor();
                // contested = false;
            }
        }
    }
}