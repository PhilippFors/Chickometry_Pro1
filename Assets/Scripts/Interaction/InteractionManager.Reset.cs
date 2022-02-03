using Checkpoints;
using UnityEngine;

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
            ReleaseObject(true);
        }
    }
}