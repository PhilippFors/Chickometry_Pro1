using System.Collections.Generic;
using Interaction.Interactables;
using ObjectAbstraction.ModelChanger;
using ObjectAbstraction.Prototype;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Used to define the area in which the Abstraction Switch takes effect.
    /// </summary>
    public class SwitchFindInArea : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        
        private void Start()
        {
            Find();
        }

        [Button]
        private void Find()
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