using System;
using Entities.Player.PlayerInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class ApplicationManager : MonoBehaviour
    {
        private bool Esctriggered => InputController.Instance.Triggered(InputPatterns.Esc);

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            // if (Esctriggered) {
            //     SceneManager.LoadSceneAsync("StartMenu", LoadSceneMode.Single);
            // }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}