using UnityEngine;

namespace ObjectAbstraction.AbstractoActions
{
    [CreateAssetMenu(fileName = "toggle wireframe beam action", menuName = "Abstracto Actions/Toggle wireframe beam action")]
    public class ToggleWireframeBeam : AbstractoAction
    {
        public override void Execute(Collider other)
        {
            var beam = other.GetComponent<WireframeBeamReflector>();
            if (beam) {
                beam.ToggleBeam();
            }
        }
    }
}