using ObjectAbstraction.New;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Used for models that can turn into billboards.
    /// Detects if model is a billboard and rotates itself towards the target.
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private GameObject lookAtTarget;
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
            if (modelChanger.IsAbstract) {
                var dir = lookAtTarget.transform.position - transform.position;
                dir.y = 0;
                var newRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * rotationSpeed);
            }
        }
    }
}