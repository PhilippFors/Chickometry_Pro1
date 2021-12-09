using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace RoomLoop
{
    public class RoomTeleporter : MonoBehaviour
    {
        public Room RoomForward => roomForward;
        public Room RoomBackward => roomBackward;

        [SerializeField] private Room roomForward;
        [SerializeField] private Room roomBackward;
        [SerializeField] private bool reverseDirection;
        [SerializeField] private LayerMask anchorMask;

        private readonly List<AnchorId> toConnect = new List<AnchorId>();

        public bool Teleport(bool forward)
        {
            FindOpenAnchor();

            if (forward) {
                if (reverseDirection) {
                    return MoveRoom(roomForward);
                }

                return MoveRoom(roomBackward);
            }

            if (reverseDirection) {
                return MoveRoom(roomBackward);
            }

            return MoveRoom(roomForward);
        }

        private bool MoveRoom(Room room)
        {
            AnchorId otherAnchor = null;
            AnchorId selfAnchor = null;

            for (int i = 0; i < toConnect.Count; i++) {
                if (room.HasAnchor(toConnect[i])) {
                    selfAnchor = toConnect[i];
                }
                else {
                    otherAnchor = toConnect[i];
                }
            }

            if (!otherAnchor || !selfAnchor || CheckForEnd(otherAnchor.transform)) {
                return false;
            }

            var diff = otherAnchor.transform.position + (room.transform.position - selfAnchor.transform.position);
            room.transform.position = diff;
            SwapRooms();
            return true;
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
            toConnect.Clear();

            roomBackward.Anchors.ForEach(x =>
            {
                if (x.CheckAchors() && !toConnect.Contains(x)) {
                    toConnect.Add(x);
                }
            });

            roomForward.Anchors.ForEach(x =>
            {
                if (x.CheckAchors() && !toConnect.Contains(x)) {
                    toConnect.Add(x);
                }
            });
        }

        private void SwapRooms()
        {
            var temp = roomForward;
            roomForward = roomBackward;
            roomBackward = temp;
        }
    }
}