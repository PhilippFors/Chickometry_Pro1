using Interaction.Items;
using Sirenix.Utilities;
using UnityEngine;
using Utlities;

namespace ObjectAbstraction
{
    /// <summary>
    /// Derived class for the AbstractoGenerator.
    /// Specific Colliders need to be disabled.
    /// </summary>
    public class AbstractoGeneratorPickup : BasePickUpInteractable
    {
        public override void OnPickup()
        {
            isPickedUp = true;
            var col = GetComponent<Collider>();
            col.isTrigger = true;

            var children = GetComponentsInChildren<MeshRenderer>();
            children.ForEach(x => x.gameObject.layer = LayerIds.InteractablesTop);
        }

        public override void OnThrow()
        {
            isPickedUp = false;
            var col = GetComponent<Collider>();
            col.isTrigger = false;
            var children = GetComponentsInChildren<MeshRenderer>();
            children.ForEach(x => x.gameObject.layer = LayerIds.Interactable);
        }
    }
}