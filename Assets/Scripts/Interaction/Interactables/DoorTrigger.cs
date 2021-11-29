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
            if (other.GetComponent<Gudrun>()) {
                door.OpenDoor();
                // contested = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Gudrun>()) {
                door.CloseDoor();
                // contested = false;
            }
        }
    }
}