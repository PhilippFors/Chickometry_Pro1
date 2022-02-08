using System;
using Entities.Player.PlayerInput;
using UnityEngine;
using UnityEngine.SceneManagement;
using UsefulCode.Utilities;

namespace Services
{
    public class ApplicationManager : SingletonBehaviour<ApplicationManager>
    {
        private bool Esctriggered => InputController.Instance.Triggered(InputPatterns.Esc);
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Esctriggered) {
                if (SceneManager.GetActiveScene().name != "Menu") {
                    SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
                }
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}