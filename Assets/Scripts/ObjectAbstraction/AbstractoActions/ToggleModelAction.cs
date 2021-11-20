using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction.AbstractoActions
{
    [CreateAssetMenu(fileName = "toggle model action", menuName = "Abstracto Actions/Toggle Model action")]
    public class ToggleModelAction : AbstractoAction
    {
        
        public override void Execute(IModelChanger modelChanger)
        {
            modelChanger.ToggleModels();
        }
    }
}