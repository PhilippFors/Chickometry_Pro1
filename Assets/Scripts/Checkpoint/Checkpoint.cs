using Entities.Player;
using UnityEngine;

namespace Checkpoints
{
    [RequireComponent(typeof(BoxCollider))]
    public class Checkpoint : MonoBehaviour
    {
        public ResetButton resetButton;
        public Transform checkPointPosition;
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>()) {
                CheckpointManager.Instance.SetActiveCheckpoint(this);
            }
        }

        private void OnDrawGizmos()
        {
            var col = GetComponent<BoxCollider>();
            var tr = transform.localScale;
            var newVec = new Vector3(col.size.x * tr.x, col.size.y * tr.y, col.size.z * tr.z);
            Gizmos.color = new Color(0, 50, 200, 0.1f);
            Gizmos.DrawCube(transform.position, newVec);
            Gizmos.color = new Color(0, 50, 200, 0.5f);
            Gizmos.DrawWireCube(transform.position, newVec);
        }
    }
}