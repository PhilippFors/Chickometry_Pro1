using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class ReturnToMenuTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) {
                SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
            }
        }
    }
}