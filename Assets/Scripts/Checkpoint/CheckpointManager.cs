using Entities.Player;
using UsefulCode.Utilities;

namespace Checkpoints
{
    public class CheckpointManager : SingletonBehaviour<CheckpointManager>
    {
        private Checkpoint activeCheckopint;

        public void SetActiveCheckpoint(Checkpoint checkpoint)
        {
            activeCheckopint = checkpoint;
        }

        public void ResetToCheckpoint()
        {
            if (activeCheckopint.resetButton) {
                activeCheckopint.resetButton.Reset();
            }

            var checkPos = activeCheckopint.checkPointPosition;
            if (checkPos) {
                transform.position = checkPos.position;
                GetComponentInChildren<SmoothMouseLook>().ForceLookAt(checkPos.rotation);
            }
            else {
                transform.position = activeCheckopint.transform.position;
                GetComponentInChildren<SmoothMouseLook>().ForceLookAt(activeCheckopint.transform.rotation);
            }
        }
    }
}