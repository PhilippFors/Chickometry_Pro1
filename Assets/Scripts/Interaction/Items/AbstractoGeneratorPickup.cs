using Interaction.Items;
using UnityEngine;

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
            GetComponent<BoxCollider>().enabled = false;
        }

        public override void OnThrow()
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}