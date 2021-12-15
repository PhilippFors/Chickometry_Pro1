using UnityEngine;

namespace Checkpoints
{
    public interface IResettable
    {
        public Vector3 OriginalPosition { get; set; }
        public Quaternion OriginalRotation { get; set; }

        public void ResetToCheckpoint();
    }
}