using System.Data.SqlTypes;
using Entities.Player.PlayerInput;
using UnityEngine;

namespace Services
{
    public class ApplicationManager : MonoBehaviour
    {
        private bool Ptriggered => InputController.Instance.Triggered(InputPatterns.P);
        private bool Esctriggered => InputController.Instance.Triggered(InputPatterns.Esc);

        private bool cursorLocked = true;
        // Update is called once per frame
        void Update()
        {
            if (Esctriggered) {
                Application.Quit();
            }

            if (Ptriggered) {
                if (cursorLocked) {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }

                cursorLocked = !cursorLocked;
            }
        }
    }
}