using Entities.Player;
using Entities.Player.PlayerInput;
using UnityEngine;
using Utlities;

namespace Interaction.Interactables
{
    public class MouseBasedRotator : BaseInteractable
    {
        [SerializeField] private float sensitity = 200;
        private float MouseDeltaX => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta).x;
        private float Width => Screen.width;
        
        private SmoothMouseLook mouseLook;

        private void Start()
        {
            mouseLook = ServiceLocator.Get<SmoothMouseLook>();
            InputController.Instance.Get(InputPatterns.RightClick).Canceled += ctx => EnableMouseLook();
        }

        public override void OnInteract()
        {
            mouseLook.enableLook = false;
            var delta = MouseDeltaX / Width * sensitity;
            var currentRot = transform.rotation.eulerAngles;
            currentRot += new Vector3(0, delta, 0);
            transform.rotation = Quaternion.Euler(currentRot);
        }

        private void EnableMouseLook()
        {
            mouseLook.enableLook = true;
        }
    }
}