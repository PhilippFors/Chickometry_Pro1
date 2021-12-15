using Entities.Player;
using UnityEngine;

namespace Checkpoints
{
    [RequireComponent(typeof(BoxCollider))]
    public class OutOfBoundsTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>()) {
                CheckpointManager.Instance.ResetToCheckpoint();
            }
        }
    }
}