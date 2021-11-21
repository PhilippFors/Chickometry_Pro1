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
        [SerializeField] private bool usePrefab;

        [SerializeField, ShowIf("usePrefab")] private GameObject prefab;
        [SerializeField, HideIf("usePrefab")] private Mesh mesh;
        [SerializeField, HideIf("usePrefab")] private Mesh colliderMesh;

        [SerializeField] private Texture2D modelTexture;
        [SerializeField] private bool useRigidbodySettings;
        [SerializeField] private RigidbodySettings rigidbodySettings;

        public void ApplyMesh(MeshFilter filter)
        {
            if (usePrefab) {
                var prefabFilter = prefab.GetComponentInChildren<MeshFilter>();
                if (prefabFilter) {
                    filter.sharedMesh = prefabFilter.sharedMesh;
                }
            }
            else {
                filter.sharedMesh = mesh;
            }
        }

        public void ApplyMeshCollider(MeshCollider collider)
        {
            if (usePrefab) {
                var prefabMeshCol = prefab.GetComponent<MeshCollider>();

                if (prefabMeshCol) {
                    collider.sharedMesh = prefabMeshCol.sharedMesh;
                }
            }
            else {
                if (colliderMesh) {
                    collider.sharedMesh = colliderMesh;
                }
                else {
                    collider.sharedMesh = mesh;
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
        [SerializeField] private RigidbodyConstraints rigidbodyConstraints;

        public void ApplySettings(Rigidbody rb)
        {
            rb.mass = mass;
            rb.drag = drag;
            rb.useGravity = useGravity;
            rb.constraints = rigidbodyConstraints;
        }
    }
}