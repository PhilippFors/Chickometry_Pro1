using Entities.Player.PlayerInput;
using UnityEngine;
using Utlities.Locators;

namespace Entities.Player
{
    public class SmoothMouseLook : MonoBehaviourService
    {
        public bool enableLook;
        public Transform CharacterBody => characterBody.transform;

        [SerializeField] private GameObject characterBody;
        [SerializeField] private Vector2 clampInDegrees = new Vector2(360, 180);
        [SerializeField] private bool lockCursor;
        [SerializeField] private float sensitivity = 0.5f;
        [SerializeField] private Vector2 smoothing = new Vector2(3, 3);

        private Vector2 MousePointerDelta => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta);
        private Vector2 targetDirection;
        private Vector2 targetCharacterDirection;
        private Vector2 mouseAbsolute;
        private Vector2 smoothMouse;
        private Quaternion yRotation;

        private void Start()
        {
            targetDirection = transform.localRotation.eulerAngles;

            if (characterBody) {
                targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
            }

            if (lockCursor) {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void ResetTargeDirection() // why
        {
            targetDirection = transform.localRotation.eulerAngles;
            if (characterBody) {
                targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
            }

            mouseAbsolute = Vector2.zero;
        }

        public void ForceLookAt(Quaternion rot)
        {
            var delta = Quaternion.Angle(rot, yRotation);
            mouseAbsolute.x += delta;
            var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
            yRotation = Quaternion.AngleAxis(mouseAbsolute.x, Vector3.up);
            characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
        }

        private void Update()
        {
            if (!enableLook) {
                return;
            }

            var targetOrientation = Quaternion.Euler(targetDirection);
            var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

            var mouseDelta = MousePointerDelta;
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing.x, sensitivity * smoothing.y));

            smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            mouseAbsolute += smoothMouse;

            if (clampInDegrees.x < 360) {
                mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
            }

            if (clampInDegrees.y < 360) {
                mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
            }

            transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right) *
                                      targetOrientation;

            if (characterBody) {
                yRotation = Quaternion.AngleAxis(mouseAbsolute.x, Vector3.up);
                characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
            }
        }
    }
}