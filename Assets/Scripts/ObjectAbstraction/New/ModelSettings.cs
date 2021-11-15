using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.New
{
    /// <summary>
    /// Settings used by the AdvModelChanger
    /// </summary>
    [System.Serializable]
    public class ModelSettings
    {
        public Mesh mesh;
        public bool hasSeperateColliderMesh;
        [EnableIf("hasSeperateColliderMesh")] public Mesh colliderMesh;
        public Texture2D modelTexture;
        [ReadOnly] public Material material;
        public bool useRigidbodySettings;
        public RigidbodySettings rigidbodySettings;
        private Material instanceMat;

        public void ApplyMesh(MeshFilter filter)
        {
            filter.sharedMesh = mesh;
        }

        public void ApplyMeshCollider(MeshCollider collider)
        {
            if (hasSeperateColliderMesh) {
                collider.sharedMesh = colliderMesh;
            }
            else {
                collider.sharedMesh = mesh;
            }
        }
        
        public void ApplyRigidbodySettings(Rigidbody rb)
        {
            if (useRigidbodySettings) {
                rigidbodySettings.ApplySettings(rb);
            }
        }

        public void ApplyMaterial(MeshRenderer renderer)
        {
            if (material) {
                if (instanceMat == null) {
                    instanceMat = new Material(material);
                }

                renderer.material = instanceMat;
            }

            ApplyTexture(renderer);
        }

        public void ApplyTexture(MeshRenderer renderer)
        {
            if (modelTexture) {
                renderer.material.SetTexture("_MainTex", modelTexture);
            }
        }
    }

    [System.Serializable]
    public class RigidbodySettings
    {
        public float mass;
        public float drag;
        public bool useGravity;
        public RigidbodyConstraints rigidbodyConstraints;

        public void ApplySettings(Rigidbody rb)
        {
            rb.mass = mass;
            rb.drag = drag;
            rb.useGravity = useGravity;
            rb.constraints = rigidbodyConstraints;
        }
    }
}