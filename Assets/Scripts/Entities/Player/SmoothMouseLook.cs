using Entities.Player.PlayerInput;
using UnityEngine;

namespace Entities.Player {
    public class SmoothMouseLook : MonoBehaviour {
        [SerializeField] private GameObject characterBody;
        [SerializeField] private Vector2 clampInDegrees = new Vector2(360, 180);
        [SerializeField] private bool lockCursor;
        [SerializeField] private float sensitivity = 0.5f;
        [SerializeField] private Vector2 smoothing = new Vector2(3, 3);

        private Vector2 MousePointerDelta => PlayerInputController.Instance.MouseDelta.ReadValue();
        private Vector2 targetDirection;
        private Vector2 targetCharacterDirection;
        private bool camLocked;
        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;

        private void Start() {
            targetDirection = transform.localRotation.eulerAngles;
            camLocked = false;

            if (characterBody)
                targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        }

        private void Update() {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            var targetOrientation = Quaternion.Euler(targetDirection);
            var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

            var mouseDelta = MousePointerDelta;
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing.x, sensitivity * smoothing.y));

            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            _mouseAbsolute += _smoothMouse;

            if (clampInDegrees.x < 360)
            {
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
            }

            if (clampInDegrees.y < 360)
            {
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
            }

            transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) *
                                      targetOrientation;

            if (characterBody)
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
                characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
            }
        }
    }
}
