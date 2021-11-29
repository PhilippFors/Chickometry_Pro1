using Entities.Player.PlayerInput;
using UnityEngine;

namespace Interaction.Interactables
{
    public class MouseBasedRotator : BaseInteractable
    {
        [SerializeField] private float sensitity = 200;
        private float MouseDeltaX => PlayerInputController.Instance.MouseDelta.ReadValue().x;
        private float Width => Screen.width;

        public override void OnInteract()
        {
            var delta = MouseDeltaX / Width * sensitity;
            var currentRot = transform.rotation.eulerAngles;
            currentRot += new Vector3(0, delta, 0);
            transform.rotation = Quaternion.Euler(currentRot);
        }
    }
}