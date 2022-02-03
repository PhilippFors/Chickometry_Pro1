using System;
using ECM.Components;
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
        public Vector2 absolute;

        private float MouseDeltaX => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta).x;
        private float MouseDeltaY => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta).y;
        private float Width => Screen.width;
        private float Height => Screen.height;

        private MouseLook mouseLook;

        private Vector2 targetDirection;
        private Vector2 targetCharacterDirection;
        private Quaternion yRotation;
        private Quaternion rotation;
        
        private void Start()
        {
            targetDirection = transform.localRotation.eulerAngles;
            // yRotation = Quaternion.Euler(0, transform.rotation.y, 0);
            mouseLook = ServiceLocator.Get<MouseLook>();
            InputController.Instance.Get(InputPatterns.RightClick).Canceled += ctx => EnableMouseLook();
            // ResetTargeDirection();
        }
        
        public override void OnInteract()
        {
            var targetOrientation = Quaternion.Euler(targetDirection);

            mouseLook.lookEnabled = false;
            absolute.x += MouseDeltaX / Width * sensitity;
            absolute.y += MouseDeltaY / Height * sensitity;

            Quaternion rot;
            Quaternion rotY = Quaternion.identity;
            // var currentRot = transform.rotation.eulerAngles;
            // currentRot += new Vector3(deltaY, deltaX, 0);
            // transform.rotation = Quaternion.Euler(currentRot);

            rot = Quaternion.AngleAxis(absolute.x, Vector3.up);

            if (useYAxis) {
                rotY = Quaternion.AngleAxis(-absolute.y, targetOrientation * Vector3.right);
            }
            transform.localRotation = rotY * rot * targetOrientation;

        }

        private void EnableMouseLook()
        {
            mouseLook.lookEnabled = true;
        }
    }
}