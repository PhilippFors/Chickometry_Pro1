using UsefulCode.Input;
using UsefulCode.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.PlayerInput
{
    /// <summary>
    /// Contains all of the inputs the player can use.
    /// Class is singleton so other classes can easily access input actions.
    /// </summary>
    public class PlayerInputController : SingletonBehaviour<PlayerInputController>
    {
        public InputActionData<float> Jump => jump ?? (jump = new InputActionData<float>(jumpAction));
        public InputActionData<float> Interact => interact ?? (interact = new InputActionData<float>(interactAction));
        public InputActionData<Vector2> Movement => movement ?? (movement = new InputActionData<Vector2>(movementAction));
        public InputActionData<Vector2> MouseDelta => mouseDelta ?? (mouseDelta = new InputActionData<Vector2>(mouseDeltaAction));
        public InputActionData<Vector2> MousePosition => mousePosition ?? (mousePosition = new InputActionData<Vector2>(mousePositionAction));
        public InputActionData<float> LeftMouseButton => leftMouseButton ?? (leftMouseButton = new InputActionData<float>(leftMouseButtonAction));
        public InputActionData<float> RightMouseButton => rightMouseButton ?? (rightMouseButton = new InputActionData<float>(rightMouseButtonAction));
        public InputActionData<float> Mousewheel => mouswheel ?? (mouswheel = new InputActionData<float>(mousewheelAction));
        public InputActionData<float> ThrowItem => throwItem ?? (throwItem = new InputActionData<float>(throwItemAction));

        
        [SerializeField] private InputActionAsset inputActions;
        
        [SerializeField] private InputActionProperty jumpAction;
        [SerializeField] private InputActionProperty interactAction;
        [SerializeField] private InputActionProperty movementAction;
        [SerializeField] private InputActionProperty mouseDeltaAction;
        [SerializeField] private InputActionProperty mousePositionAction;
        [SerializeField] private InputActionProperty leftMouseButtonAction;
        [SerializeField] private InputActionProperty rightMouseButtonAction;
        [SerializeField] private InputActionProperty mousewheelAction;
        [SerializeField] private InputActionProperty throwItemAction;
        private InputActionData<float> jump;
        private InputActionData<float> interact;
        private InputActionData<Vector2> movement;
        private InputActionData<Vector2> mouseDelta;
        private InputActionData<Vector2> mousePosition;
        private InputActionData<float> leftMouseButton;
        private InputActionData<float> rightMouseButton;
        private InputActionData<float> mouswheel;
        private InputActionData<float> throwItem;

        private void OnEnable()
        {
            EnableControls();
        }

        private void OnDisable()
        {
            DisableControls();
        }

        public void EnableControls()
        {
            foreach (var action in inputActions)
            {
                action?.Enable();
            }
        }

        public void DisableControls()
        {
            foreach (var action in inputActions)
            {
                action?.Disable();
            }
        }
    }
}