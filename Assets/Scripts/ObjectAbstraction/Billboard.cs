using ObjectAbstraction.New;
using UnityEngine;

namespace ObjectAbstraction
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private GameObject lookAtTarget;
        [SerializeField] private int abstractLayer;
        [SerializeField] private float rotationSpeed = 4f;
        private AdvModelChanger modelChanger;

        private void Awake()
        {
            if (!lookAtTarget) {
                lookAtTarget = GameObject.FindWithTag("Gudrun");
            }

            modelChanger = GetComponent<AdvModelChanger>();
        }
    
        void Update()
        {
            if (modelChanger.AbstractLayer == abstractLayer) {
                var dir = lookAtTarget.transform.position - transform.position;
                dir.y = 0;
                var newRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotationSpeed);
            }
        }
    }
}