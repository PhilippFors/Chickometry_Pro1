using System.Collections;
using System.Collections.Generic;
using ObjectAbstraction.AbstractoActions;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction.Utilities
{
    /// <summary>
    /// Trigger setup that detects model changers and executes an action defined in a scriptable object.
    /// </summary>
    public class AbstractoTrigger : MonoBehaviour
    {
        [SerializeField] private AbstractoAction[] triggerEnterAction;
        [SerializeField] private AbstractoAction[] triggerExitAction;

        private List<IModelChanger> changers = new List<IModelChanger>();

        protected virtual void OnTriggerEnter(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null && !changers.Contains(modelChanger)) {
                changers.Add(modelChanger);
                foreach (var action in triggerEnterAction) {
                    action.Execute(other);
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null && changers.Contains(modelChanger)) {
                StartCoroutine(WaitRemove(modelChanger));
                foreach (var action in triggerExitAction) {
                    action.Execute(other);
                }
            }
        }

        private IEnumerator WaitRemove(IModelChanger modelChanger)
        {
            yield return new WaitForSeconds(0.1f);
            changers.Remove(modelChanger);
        }
    }
}