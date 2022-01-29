using Interaction.Interactables;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace Checkpoints
{
    [RequireComponent(typeof(BoxCollider))]
    public class GudrunNest : BaseInteractable
    {
        [SerializeField] private Transform resetPosition;
        public override void OnInteract()
        {
            var gudrun = GudrunNestManager.Instance.gudrun;
            var modelChanger = gudrun.GetComponent<AdvModelChanger>();
            if (modelChanger) {
                if (!modelChanger.IsAbstract) {
                    modelChanger.ToggleModels();
                }
            }

            gudrun.position = resetPosition.position;
        }
    }
}