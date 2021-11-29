using System.Linq;
using ObjectAbstraction.AbstractoActions;
using UnityEngine;

namespace ObjectAbstraction.Util
{
    public class WireframeBeamTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject[] exclude;

        [SerializeField] private AbstractoAction[] triggerEnterAction;
        [SerializeField] private AbstractoAction[] triggerExitAction;
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (exclude.Contains(other.gameObject)) {
                return;
            }
            
            foreach (var action in triggerEnterAction) {
                action.Execute(other);
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (exclude.Contains(other.gameObject)) {
                return;
            }
            
            foreach (var action in triggerExitAction) {
                action.Execute(other);
            }
        }
    }
}