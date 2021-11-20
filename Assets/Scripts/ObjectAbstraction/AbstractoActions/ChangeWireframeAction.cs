using ObjectAbstraction.ModelChanger;
using ObjectAbstraction.Wireframe;
using UnityEngine;
using Utlities;

namespace ObjectAbstraction.AbstractoActions
{
    [CreateAssetMenu(fileName = "Change wireframe action", menuName = "Abstracto Actions/Change Wireframe action")]
    public class ChangeWireframeAction : AbstractoAction
    {
        public override void Execute(IModelChanger modelChanger)
        {
            var wireframe = modelChanger.GetComponent<WireframeIdentifier>();
            if (wireframe) {
                wireframe.ToggleWireFrame();
            }
        }
    }
}