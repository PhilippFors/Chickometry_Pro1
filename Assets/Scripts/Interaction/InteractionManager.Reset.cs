using Checkpoints;

namespace Interactables
{
    public partial class InteractionManager
    {
        public void ResetBehaviour()
        {
            if (!currentlyHeldItem) {
                return;
            }
            currentlyHeldItem.transform.parent = null;
            ReleaseObject();
        }
    }
}