using Entities.Player.PlayerInput;
using UnityEngine;
using Utlities;

namespace Interaction.Interactables
{
    /// <summary>
    /// Rotates object on y axis based on mousedelta.
    /// Disables mouse look while rotating.
    /// </summary>
    public class MouseBasedRotator : BaseInteractable
    {
        [SerializeField] private float sensitity = 200;
        [SerializeField] private bool useYAxis;

        private float MouseDeltaX => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta).x;
        private float MouseDeltaY => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta).y;
        private float Width => Screen.width;
        private float Height => Screen.height;
        private FirstPersonController mouseLook;
        private Vector2 absolute;
        private Vector2 targetDirection;

        private void Start()
        {
            targetDirection = transform.localRotation.eulerAngles;
            mouseLook = ServiceLocator.Get<FirstPersonController>();
            InputController.Instance.Get(InputPatterns.RightClick).Canceled += ctx => EnableMouseLook();
        }
        
        public override void OnInteract()
        {
            var targetOrientation = Quaternion.Euler(targetDirection);

            mouseLook.cameraCanMove = false;
            absolute.x += MouseDeltaX / Width * sensitity;
            absolute.y += MouseDeltaY / Height * sensitity;

            Quaternion rot;
            Quaternion rotY = Quaternion.identity;

            rot = Quaternion.AngleAxis(absolute.x, Vector3.up);

            if (useYAxis) {
                rotY = Quaternion.AngleAxis(-absolute.y, targetOrientation * Vector3.right);
            }
            transform.localRotation = rotY * rot * targetOrientation;

        }

        private void EnableMouseLook()
        {
            mouseLook.cameraCanMove = true;
        }
    }
}