using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction.AbstractoActions
{
    [CreateAssetMenu(fileName = "toggle model action", menuName = "Abstracto Actions/Toggle Model action")]
    public class ToggleModelAction : AbstractoAction
    {
        
        public override void Execute(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null) {
                modelChanger.ToggleModels();
            }
        }
    }
}