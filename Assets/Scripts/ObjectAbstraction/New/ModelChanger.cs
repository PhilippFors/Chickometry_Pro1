using UnityEngine;

namespace ObjectAbstraction.New
{
    /// <summary>
    /// Stinky poopoo
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class ModelChanger : MonoBehaviour, IModelChanger
    {
        public bool Shootable => shootable;
        public bool IsAbstract => isAbstract;
        
        [SerializeField] private bool isAbstract;
        [SerializeField] private bool shootable = true;
        
        [SerializeField] private Mesh normalMesh;
        [SerializeField] private Mesh abstractMesh;

        private MeshCollider meshCollider;
        private MeshFilter meshFilter;
        private void Start()
        {
            meshCollider = GetComponent<MeshCollider>();
            meshFilter = GetComponent<MeshFilter>();
        }
        
        public void ToggleModels()
        {
            if (isAbstract) {
                EnableNormal();
            }
            else {
                EnableAbstract();
            }
            
            isAbstract = !isAbstract;
        }
        
        private void EnableAbstract()
        {
            meshFilter.sharedMesh = abstractMesh;
            meshCollider.sharedMesh = abstractMesh;
        }

        private void EnableNormal()
        {
            meshFilter.sharedMesh = normalMesh;
            meshCollider.sharedMesh = normalMesh;
        }

        private void OnValidate()
        {
            meshCollider = GetComponent<MeshCollider>();
            meshFilter = GetComponent<MeshFilter>();
            if (isAbstract) {
                EnableAbstract();
            }
            else {
                EnableNormal();
            }
        }
    }
}