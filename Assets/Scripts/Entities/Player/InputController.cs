using System;
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
            inputDictonary.Add(InputPatterns.Jump, new InputActionData<float>(jumpAction));
            inputDictonary.Add(InputPatterns.Interact, new InputActionData<float>(interactAction));
            inputDictonary.Add(InputPatterns.Movement, new InputActionData<Vector2>(movementAction));
            inputDictonary.Add(InputPatterns.MouseDelta, new InputActionData<Vector2>(mouseDeltaAction));
            inputDictonary.Add(InputPatterns.MousePosition, new InputActionData<Vector2>(mousePositionAction));
            inputDictonary.Add(InputPatterns.LeftClick, new InputActionData<float>(leftMouseButtonAction));
            inputDictonary.Add(InputPatterns.RightClick, new InputActionData<float>(rightMouseButtonAction));
            inputDictonary.Add(InputPatterns.MouseWheel, new InputActionData<float>(mousewheelAction));
            inputDictonary.Add(InputPatterns.Throw, new InputActionData<float>(throwItemAction));
        }

        public InputActionData<T> Get<T>(InputPatterns pattern) where T : struct
        {
            inputDictonary.TryGetValue(pattern, out var val);
            InputActionData<T> d = (InputActionData<T>) val;
            return d;
        }

        public T GetValue<T>(InputPatterns pattern) where T : struct => Get<T>(pattern).ReadValue();
        
        public bool Triggered<T>(InputPatterns pattern) where T : struct => Get<T>(pattern).Triggered;

        public bool Pressed<T>(InputPatterns pattern) where T : struct => Get<T>(pattern).IsPressed;

        public void EnableInput<T>(InputPatterns pattern) where T : struct => Get<T>(pattern).Enable();
        
        public void DisableInput<T>(InputPatterns pattern) where T : struct => Get<T>(pattern).Disable();

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