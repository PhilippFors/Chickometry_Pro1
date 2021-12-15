using UnityEngine;

namespace ObjectAbstraction.ModelChanger
{
    public partial class AdvModelChanger
    {
        public Vector3 OriginalPosition { get; set; }
        public Quaternion OriginalRotation { get; set; }
        private bool originalAbstraction;

        public void ResetToCheckpoint()
        {
            transform.position = OriginalPosition;
            transform.rotation = OriginalRotation;
            if (originalAbstraction != IsAbstract) {
                ToggleModels();
            }
        }
    }
}