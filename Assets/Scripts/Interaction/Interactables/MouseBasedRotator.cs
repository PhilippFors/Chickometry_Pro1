using Entities.Player;
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

        private SmoothMouseLook mouseLook;

        private Vector2 targetDirection;
        private Vector2 targetCharacterDirection;
        private Vector2 absolute;
        private void Start()
        {
            targetDirection = transform.localRotation.eulerAngles;

            mouseLook = ServiceLocator.Get<SmoothMouseLook>();
            InputController.Instance.Get(InputPatterns.RightClick).Canceled += ctx => EnableMouseLook();
        }
        
        public override void OnInteract()
        {
            var targetOrientation = Quaternion.Euler(targetDirection);

            mouseLook.enableLook = false;
            absolute.x += MouseDeltaX / Width * sensitity;
            Quaternion rot;
            Quaternion rotY = Quaternion.identity;
            // var currentRot = transform.rotation.eulerAngles;
            // currentRot += new Vector3(deltaY, deltaX, 0);
            // transform.rotation = Quaternion.Euler(currentRot);

            if (useYAxis) {
                absolute.y += MouseDeltaY / Height * sensitity;
                rotY = Quaternion.AngleAxis(absolute.y, targetOrientation * Vector3.right) * targetOrientation;
            }

            rot = Quaternion.AngleAxis(absolute.x, Vector3.up);
            transform.localRotation = rot * rotY * targetOrientation;
        }

        private void EnableMouseLook()
        {
            mouseLook.enableLook = true;
        }
    }
}