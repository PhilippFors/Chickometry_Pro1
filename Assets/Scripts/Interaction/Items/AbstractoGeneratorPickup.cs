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

            ChangeLayer(true);
        }

        public override void OnThrow()
        {
            isPickedUp = false;
            var col = GetComponent<Collider>();
            col.isTrigger = false;
            ChangeLayer(false);
        }
    }
}