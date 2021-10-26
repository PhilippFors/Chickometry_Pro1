using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Used to define the area in which the Abstraction Switch takes effect.
    /// </summary>
    public class SwitchAreaFinder : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        // TODO...?: Make it look for things in its area at runtime to update the active objects
        private void Start()
        {
            FindInArea();
        }

        [Button]
        private void FindInArea()
        {
            var col = GetComponent<BoxCollider>();
            var abstractionSwitch = GetComponentInParent<AbstractoSwitch>();

            var offset = col.center - transform.position;

            var objs = Physics.OverlapBox(transform.position + offset, col.size * 2, transform.rotation, layerMask);

            foreach (var s in objs) {
                var switcher = s.GetComponentInParent<ModelChanger>();
                if (switcher) {
                    abstractionSwitch.AddModelSwitcher(switcher);
                }
            }
        }

        private void OnDrawGizmos()
        {
            var col = GetComponent<BoxCollider>();
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(255, 255, 255, 0.2f);

            var offset = col.center - transform.position;
            Gizmos.DrawCube(transform.position + offset, col.size);
            Gizmos.DrawWireCube(transform.position + offset, col.size);
            // Gizmos.DrawCube(col.center, col.size);
            // Gizmos.DrawWireCube(col.center, col.size);
        }
    }
}