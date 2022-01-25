using System.Collections;
using System.Collections.Generic;
using ObjectAbstraction.AbstractoActions;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Any object that enters or exits the radius will have its abstraction level toggled.
    /// Has varius settings that can be initialized.
    /// </summary>
    public class AbstractoRadius : MonoBehaviour
    {
        [SerializeField] private AbstractoAction action;

        [SerializeField] private float radius = 1;

        // [SerializeField] private Transform radiusVis;
        [SerializeField] private bool permanentChange;
        [SerializeField] private bool hasTimer;
        [SerializeField] private float timer;

        private List<IModelChanger> changers = new List<IModelChanger>();
        private AbstractoGrenadeThrower grenadeThrower;

        private void Start()
        {
            // For the pickup thing at start
            var cols = Physics.OverlapSphere(transform.position, radius);
            foreach (var col in cols) {
                var modelChanger = GetComponentInParent<IModelChanger>();
                if (modelChanger != null && !changers.Contains(modelChanger)) {
                    modelChanger.ToggleModels();
                    changers.Add(modelChanger);
                }
            }
        }

        private void OnEnable()
        {
            if (hasTimer) {
                StartCoroutine(Timer());
            }
        }

        public void Init(AbstractoGrenadeThrower thrower, bool permanent, float t = 0)
        {
            grenadeThrower = thrower;
            permanentChange = permanent;
            if (t != 0) {
                timer = t;
                StartCoroutine(Timer());
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(timer);
            if (grenadeThrower) {
                grenadeThrower.Remove(gameObject);
            }

            if (!permanentChange) {
                ToggleAll();
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null && !changers.Contains(modelChanger)) {
                changers.Add(modelChanger);
                action.Execute(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var modelChanger = other.gameObject.GetComponentInParent<IModelChanger>();
            if (modelChanger != null && changers.Contains(modelChanger) && !permanentChange) {
                action.Execute(other);
            }
            StartCoroutine(WaitRemove(modelChanger));
        }

        private IEnumerator WaitRemove(IModelChanger modelChanger)
        {
            if (modelChanger != null) {
                yield return new WaitForSeconds(0.1f);
                changers.Remove(modelChanger);
            }
        }

        private void ToggleAll()
        {
            // foreach (var m in changers) {
            //     action.Execute(m);
            // }
        }

        private void OnCollisionEnter(Collision other)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        // private void OnValidate()
        // {
        //     GetComponent<SphereCollider>().radius = radius;
        //     radiusVis.localScale = new Vector3(radius, radius, radius) * 2;
        // }
    }
}