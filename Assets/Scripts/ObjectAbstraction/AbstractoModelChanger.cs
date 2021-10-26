using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// References a normal and abstract model and toggles between them.
    /// </summary>
    public class AbstractoModelChanger : MonoBehaviour
    {
        public bool Shootable => shootable;
        public bool IsAbstract => isAbstract;
        public bool changeOverride;
        [SerializeField] private bool isAbstract;
        [SerializeField] private bool shootable = true;
        [SerializeField] private GameObject normalMesh;
        [SerializeField] private GameObject abstractMesh;

        public void ToggleModels()
        {
            if (isAbstract) {
                EnableNormal();
                isAbstract = false;
            }
            else {
                EnableAbstract();
                isAbstract = true;
            }
        }

        private void EnableAbstract()
        {
            abstractMesh.SetActive(true);
            normalMesh.SetActive(false);
        }

        private void EnableNormal()
        {
            abstractMesh.SetActive(false);
            normalMesh.SetActive(true);
        }

        private void OnValidate()
        {
            if (isAbstract) {
                EnableAbstract();
            }
            else {
                EnableNormal();
            }
        }
    }
}