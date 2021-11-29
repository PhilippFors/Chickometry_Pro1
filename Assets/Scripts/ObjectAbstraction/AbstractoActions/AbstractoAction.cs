using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction.AbstractoActions
{
    /// <summary>
    /// Action used in AbstractoBoxTrigger
    /// </summary>
    public abstract class AbstractoAction : ScriptableObject
    {
        public abstract void Execute(Collider other);
    }
}