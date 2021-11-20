using System.Collections;
using System.Collections.Generic;
using ObjectAbstraction.AbstractoActions;
using UnityEngine;

namespace ObjectAbstraction.Utilities
{
    /// <summary>
    /// Trigger setup that detects model changers and executes an action defined in a scriptable object.
    /// </summary>
    public class AbstractoBoxTrigger : MonoBehaviour
    {
        [SerializeField] private AbstractoAction action;

        private List<IModelChanger> changers = new List<IModelChanger>();
        private AbstractoGrenadeThrower grenadeThrower;
        
        private void OnTriggerEnter(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null && !changers.Contains(modelChanger)) {
                changers.Add(modelChanger);
                action.Execute(modelChanger);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null && changers.Contains(modelChanger)) {
                action.Execute(modelChanger);
                StartCoroutine(WaitRemove(modelChanger));
            }
        }

        private IEnumerator WaitRemove(IModelChanger modelChanger)
        {
            yield return new WaitForSeconds(0.1f);
            changers.Remove(modelChanger);
        }
    }
}