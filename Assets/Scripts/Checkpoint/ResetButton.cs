using Interaction.Interactables;

namespace Checkpoints
{
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
        }
    }
}