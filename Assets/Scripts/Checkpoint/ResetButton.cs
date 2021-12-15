using Interactables;
using Interaction.Interactables;

namespace Checkpoints
{
    /// <summary>
    /// Resets a give area when interacted with.
    /// </summary>
    public class ResetButton : BaseInteractable
    {
        private ResetAreaFinder resetter;
        private void Start()
        {
            resetter = GetComponentInChildren<ResetAreaFinder>();
        }

        public override void OnInteract()
        {
            Reset();
        }

        public void Reset()
        {
            resetter.Reset();
            CheckpointManager.Instance.ResetSingleBehaviour<InteractionManager>();
        }
    }
}