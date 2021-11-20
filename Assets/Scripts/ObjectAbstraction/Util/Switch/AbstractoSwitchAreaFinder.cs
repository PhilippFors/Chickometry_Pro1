using ObjectAbstraction.Prototype;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Used to define the area in which the Abstraction Switch takes effect.
    /// </summary>
    public class AbstractoSwitchAreaFinder : MonoBehaviour
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

            var objs = Physics.OverlapBox(transform.position, col.size, transform.rotation, layerMask);

            foreach (var s in objs) {
                var switcher = s.GetComponentInParent<IModelChanger>();
                if (switcher != null) {
                    abstractionSwitch.AddModelSwitcher(switcher);
                }
            }
        }

        private void OnDrawGizmos()
        {
            var col = GetComponent<BoxCollider>();
            Gizmos.color = new Color(255, 255, 255, 0.2f);

            Gizmos.DrawCube(transform.position, col.size);
            Gizmos.DrawWireCube(transform.position, col.size);
            // Gizmos.DrawCube(col.center, col.size);
            // Gizmos.DrawWireCube(col.center, col.size);
        }
    }
}