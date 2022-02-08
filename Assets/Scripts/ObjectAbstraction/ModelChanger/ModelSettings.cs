using Cysharp.Threading.Tasks.Triggers;
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
        
        public void ApplyMeshCollider(MeshCollider collider, Mesh mesh)
        {
            if (!collider) {
                return;
            }

            var obj = collider.gameObject;
            MeshCollider col = collider;
            // none of this works aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
            if (useMeshCollider) {
                if (Application.isPlaying) {
                    var convex = collider.convex;
                    var trigger = collider.isTrigger;
                    Object.Destroy(collider);
                    col = obj.AddComponent<MeshCollider>();
                    col.convex = convex;
                    col.isTrigger = trigger;
                }

                if (mesh) {
                    col.sharedMesh = mesh;
                }
                else {
                    col.sharedMesh = customColliderMesh;
                }
            }
            else {
                col.enabled = false;
            }
        }

        public void ApplyCollider(ref GameObject previousColliders)
        {
            if (!useMeshCollider) {
                if (previousColliders) {
                    previousColliders.SetActive(false);
                }

                if (colliderParent) {
                    colliderParent.SetActive(true);
                    previousColliders = colliderParent;
                }
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