using Entities.Player.PlayerInput;
using UnityEngine;

public class LinearMouseInterpolation : MonoBehaviour
{
    [SerializeField] private float sensitivity = 1f;
    private Vector2 MouseDelta => PlayerInputController.Instance.MouseDelta.ReadValue();

    private float DeltaX => MouseDelta.x;
    private int width => Screen.width;
    public float absolute;
    private void Update()
    {
        Debug.Log(DeltaX);
        absolute += (DeltaX/width) * sensitivity;
        absolute = Mathf.Clamp(absolute, 0, 1);
    }
}
