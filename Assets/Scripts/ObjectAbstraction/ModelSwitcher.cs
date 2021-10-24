using UnityEngine;

namespace ObjectAbstraction
{
    public class ModelSwitcher : MonoBehaviour
    {
        public bool IsAbstract => isAbstract;
        [SerializeField] private bool isAbstract;
        [SerializeField] private GameObject normalMesh;
        [SerializeField] private GameObject abstractMesh;

        public void ToggleModels()
        {
            if (isAbstract)
            {
                EnableNormal();
                isAbstract = false;
            }
            else
            {
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
            if (isAbstract)
            {
                EnableAbstract();
            }
            else
            {
                EnableNormal();
            }
        }
    }
}