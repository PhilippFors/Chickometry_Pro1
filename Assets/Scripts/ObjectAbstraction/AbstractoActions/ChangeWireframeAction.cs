using ObjectAbstraction.Wireframe;
using UnityEngine;

namespace ObjectAbstraction.AbstractoActions
{
    [CreateAssetMenu(fileName = "Change wireframe action", menuName = "Abstracto Actions/Change Wireframe action")]
    public class ChangeWireframeAction : AbstractoAction
    {
        public override void Execute(Collider other)
        {
            var wireframe = other.GetComponentInParent<WireframeIdentifier>();
            if (wireframe) {
                wireframe.ToggleWireFrame();
            }
        }
    }
}