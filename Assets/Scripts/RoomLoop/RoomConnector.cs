using Sirenix.OdinInspector;
using UnityEngine;

public class RoomConnector : MonoBehaviour
{
    public Transform[] roomForwardAnchors = new Transform[2];
    public Transform[] roomBackwardsAnchors = new Transform[2];

    public Transform roomForward;
    public Transform roomBackward;
    
    [SerializeField] private LayerMask anchorMask;

    private readonly Transform[] toConnect = new Transform[2];
    
    [Button]
    public void Teleport(bool forward)
    {
        FindOpenAnchor();

        if (forward) {
            if (CheckForEnd(toConnect[0])) {
                return;
            }
            var diff = toConnect[0].position + (roomBackward.position - toConnect[1].position);
            roomBackward.position = diff;
            SwapRooms();
        }
        else {
            if (CheckForEnd(toConnect[1])) {
                return;
            }
            var diff = toConnect[1].position + (roomForward.position - toConnect[0].position);
            roomForward.position = diff;
            SwapRooms();
        }
    }

    private bool CheckForEnd(Transform anchor)
    {
        var hits = Physics.OverlapSphere(anchor.position, 0.5f, anchorMask, QueryTriggerInteraction.Collide);
        if (hits.Length > 0) {
            foreach (var hit in hits) {
                if (hit.GetComponent<RoomStopper>()) {
                    return true;
                }
            }
        }

        return false;
    }
    
    private void FindOpenAnchor()
    {
        for (int i = 0; i < roomForwardAnchors.Length; i++) {
            var hits = Physics.OverlapSphere(roomForwardAnchors[i].position, 0.5f, anchorMask,
                QueryTriggerInteraction.Collide);
            if (hits.Length > 0) {
                for (int j = 0; j < hits.Length; j++) {
                    foreach (var tr in roomBackwardsAnchors) {
                        if (tr != hits[j].transform) {
                            toConnect[1] = tr;
                            if (i == 0) {
                                toConnect[0] = roomForwardAnchors[1];
                            }
                            else {
                                toConnect[0] = roomForwardAnchors[0];
                            }

                            return;
                        }
                    }
                }
            }
        }
    }

    private void SwapRooms()
    {
        var temp = roomForward;
        roomForward = roomBackward;
        roomBackward = temp;

        var temp2 = roomForwardAnchors;
        roomForwardAnchors = roomBackwardsAnchors;
        roomBackwardsAnchors = temp2;
    }
}