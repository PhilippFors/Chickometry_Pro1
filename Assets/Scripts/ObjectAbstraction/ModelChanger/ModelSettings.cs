using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.ModelChanger
{
    /// <summary>
    /// Settings used by the AdvModelChanger
    /// </summary>
    [System.Serializable]
    public class ModelSettings
    {
        [SerializeField] private bool useMeshCollider;

        [SerializeField, HideIf("useMeshCollider")]
        public GameObject colliderParent;
        
        [SerializeField, ShowIf("useMeshCollider"), Tooltip("Fill if object has special collider. Otherwise leave empty.")]
        private Mesh customColliderMesh;
        
        [SerializeField] private bool useRigidbodySettings;
        [SerializeField] private RigidbodySettings rigidbodySettings;
        
        public void ApplyMeshCollider(MeshCollider collider, MeshFilter filter = null)
        {
            if (!collider) {
                return;
            }
            
            if (useMeshCollider) {
                collider.enabled = true;
                if (filter) {
                    collider.sharedMesh = filter.sharedMesh;
                }
                else {
                    collider.sharedMesh = customColliderMesh;
                }
            }
            else {
                collider.enabled = false;
            }
        }

        public void ApplyCollider(ref GameObject previousColliders)
        {
            if (!useMeshCollider) {
                if (previousColliders) {
                    previousColliders.SetActive(false);
                }

                colliderParent.SetActive(true);
                previousColliders = colliderParent;
            }
            else {
                if (previousColliders) {
                    previousColliders.SetActive(false);
                }

                if (colliderParent) {
                    colliderParent.SetActive(false);
                }
            }
        }

        public void ApplyRigidbodySettings(Rigidbody rb)
        {
            if (!rb) {
                return;
            }
            
            if (useRigidbodySettings) {
                rigidbodySettings.ApplySettings(rb);
            }
        }
    }

    [System.Serializable]
    public class RigidbodySettings
    {
        [SerializeField] private float mass = 1;
        [SerializeField] private float drag;
        [SerializeField] private bool useGravity = true;

        public void ApplySettings(Rigidbody rb)
        {
            rb.mass = mass;
            rb.drag = drag;
            rb.useGravity = useGravity;
        }
    }
}