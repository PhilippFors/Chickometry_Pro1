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
        
        [SerializeField] private Mesh mesh;

        [SerializeField, ShowIf("useMeshCollider")]
        private Mesh colliderMesh;

        [SerializeField] private Texture2D modelTexture;
        [SerializeField] private bool useRigidbodySettings;
        [SerializeField] private RigidbodySettings rigidbodySettings;

        public void ApplyMesh(MeshFilter filter)
        {
            filter.sharedMesh = mesh;
        }

        public void ApplyMeshCollider(MeshCollider collider)
        {
            if (useMeshCollider) {
                collider.enabled = true;
                if (colliderMesh) {
                    collider.sharedMesh = colliderMesh;
                }
                else {
                    collider.sharedMesh = mesh;
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
            if (useRigidbodySettings) {
                rigidbodySettings.ApplySettings(rb);
            }
        }

        public void ApplyTexture(MeshRenderer renderer)
        {
            if (modelTexture) {
                renderer.sharedMaterial.SetTexture("_MainTex", modelTexture);
            }
        }

        public Mesh GetMesh() => mesh;
        public Texture2D GetTexture() => modelTexture;
    }

    [System.Serializable]
    public class RigidbodySettings
    {
        [SerializeField] private float mass = 1;
        [SerializeField] private float drag;
        [SerializeField] private bool useGravity = true;
        private RigidbodyConstraints rigidbodyConstraints;

        public void ApplySettings(Rigidbody rb)
        {
            rb.mass = mass;
            rb.drag = drag;
            rb.useGravity = useGravity;
            // rb.constraints = rigidbodyConstraints;
        }
    }
}