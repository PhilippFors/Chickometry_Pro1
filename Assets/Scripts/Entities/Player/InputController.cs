using System.Collections.Generic;
using UsefulCode.Input;
using UsefulCode.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player.PlayerInput
{
    public enum InputPatterns
    {
        RightClick,
        LeftClick,
        Jump,
        Interact,
        Movement,
        MouseDelta,
        MousePosition,
        MouseWheel,
        Throw
    }

    /// <summary>
    /// Contains all of the inputs the player can use.
    /// Class is singleton so other classes can easily access input actions.
    /// </summary>
    public class InputController : SingletonBehaviour<InputController>
    {
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
        
        private readonly Dictionary<InputPatterns, InputActionData> inputDictonary = new Dictionary<InputPatterns, InputActionData>();
        
        private void OnEnable()
        {
            EnableControls();
        }

        private void OnDisable()
        {
            DisableControls();
        }

        public override void Awake()
        {
            base.Awake();
            inputDictonary.Add(InputPatterns.Jump, new InputActionData(jumpAction));
            inputDictonary.Add(InputPatterns.Interact, new InputActionData(interactAction));
            inputDictonary.Add(InputPatterns.Movement, new InputActionData(movementAction));
            inputDictonary.Add(InputPatterns.MouseDelta, new InputActionData(mouseDeltaAction));
            inputDictonary.Add(InputPatterns.MousePosition, new InputActionData(mousePositionAction));
            inputDictonary.Add(InputPatterns.LeftClick, new InputActionData(leftMouseButtonAction));
            inputDictonary.Add(InputPatterns.RightClick, new InputActionData(rightMouseButtonAction));
            inputDictonary.Add(InputPatterns.MouseWheel, new InputActionData(mousewheelAction));
            inputDictonary.Add(InputPatterns.Throw, new InputActionData(throwItemAction));
        }

        public InputActionData Get(InputPatterns pattern)
        {
            inputDictonary.TryGetValue(pattern, out var val);
            return val;
        }

        public T GetValue<T>(InputPatterns pattern) where T : struct => Get(pattern).ReadValue<T>();
        
        public bool Triggered(InputPatterns pattern) => Get(pattern).Triggered;

        public bool IsPressed(InputPatterns pattern) => Get(pattern).IsPressed;

        public void EnableInput(InputPatterns pattern) => Get(pattern).Enable();
        
        public void DisableInput(InputPatterns pattern) => Get(pattern).Disable();

        public void EnableControls()
        {
            foreach (var action in inputActions) {
                action?.Enable();
            }
        }

        public void DisableControls()
        {
            foreach (var action in inputActions) {
                action?.Disable();
            }
        }
    }
}