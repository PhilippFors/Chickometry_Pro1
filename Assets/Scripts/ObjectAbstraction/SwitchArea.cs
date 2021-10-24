using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction
{
    public class SwitchArea : MonoBehaviour
    {
        private void Awake() {
            FindInArea();
        }

        [Button]
        private void FindInArea() {
            var col = GetComponent<BoxCollider>();
            var abstractionSwitch = GetComponentInParent<AbstractionSwitch>();
            var objs = Physics.OverlapBox(col.transform.position, col.size, col.transform.rotation);
            foreach (var s in objs) {
                var switcher = s.GetComponentInParent<ModelSwitcher>();
                if (switcher) {
                    abstractionSwitch.AddModelSwitcher(switcher);
                }
            }
        }

        private void OnDrawGizmos() {
            var col = GetComponent<BoxCollider>();
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = new Color(255, 255, 255, 0.2f);
            Gizmos.DrawCube(col.center, col.size);
            Gizmos.DrawWireCube(col.center, col.size);
        }
    }
}