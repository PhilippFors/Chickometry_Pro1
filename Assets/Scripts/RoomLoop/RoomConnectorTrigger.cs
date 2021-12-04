using Entities.Player;
using UnityEngine;

public class RoomConnectorTrigger : MonoBehaviour
{
    public bool isEnabled = true;
    public bool forwardDoor;
    [SerializeField] private RoomConnector roomConnector;
    [SerializeField] private RoomConnectorTrigger opposite;
    [SerializeField] private Transform room;
    private bool hasPassed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isEnabled) {
            isEnabled = true;
            return;
        }

        var player = other.GetComponentInParent<PlayerMovement>();
        if (player) {
            if (forwardDoor && roomConnector.roomBackward != room) {
                // if (!hasPassed) {
                roomConnector.Teleport(forwardDoor);
                hasPassed = true;
                opposite.isEnabled = false;
                // }
            }
            else if (!forwardDoor && roomConnector.roomForward != room) {
                // if (!hasPassed) {
                roomConnector.Teleport(forwardDoor);
                hasPassed = true;
                opposite.isEnabled = false;
                // }
            }
        }
    }
}