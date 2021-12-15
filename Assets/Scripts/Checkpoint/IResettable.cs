using UnityEngine;

namespace Checkpoints
{
    /// <summary>
    /// Custom reset functions for those who need it
    /// </summary>
    public interface IResettable
    {
        public Vector3 OriginalPosition { get; set; }
        public Quaternion OriginalRotation { get; set; }

        public void ResetToCheckpoint();
    }
}