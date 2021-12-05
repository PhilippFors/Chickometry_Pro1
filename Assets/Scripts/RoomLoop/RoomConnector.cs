using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace RoomLoop
{
    public class RoomConnector : MonoBehaviour
    {
        public bool reverseDirection;
        public Room roomForward;
        public Room roomBackward;

        [SerializeField] private LayerMask anchorMask;

        private AnchorId[] toConnect;

        [Button]
        public bool Teleport(bool forward)
        {
            FindOpenAnchor();

            if (forward) {
                if (reverseDirection) {
                    return MoveForwardRoom();
                }

                return MoveBackwardRoom();
            }

            if (reverseDirection) {
                return MoveBackwardRoom();
            }

            return MoveForwardRoom();
        }

        private bool MoveBackwardRoom()
        {
            AnchorId otherAnchor = null;

            AnchorId selfAnchor = null;
            for (int i = 0;
                i < toConnect.Length;
                i++) {
                if (roomBackward.HasAnchor(toConnect[i])) {
                    selfAnchor = toConnect[i];
                }
                else {
                    otherAnchor = toConnect[i];
                }
            }

            if (CheckForEnd(otherAnchor.transform)) {
                return false;
            }

            var diff = otherAnchor.transform.position +
                       (roomBackward.transform.position - selfAnchor.transform.position);
            roomBackward.transform.position = diff;
            SwapRooms();
            return true;
        }

        private bool MoveForwardRoom()
        {
            AnchorId otherAnchor = null;
            AnchorId selfAnchor = null;

            for (int i = 0; i < toConnect.Length; i++) {
                if (roomForward.HasAnchor(toConnect[i])) {
                    selfAnchor = toConnect[i];
                }
                else {
                    otherAnchor = toConnect[i];
                }
            }

            if (CheckForEnd(otherAnchor.transform)) {
                return false;
            }

            var diff = otherAnchor.transform.position +
                       (roomForward.transform.position - selfAnchor.transform.position);
            roomForward.transform.position = diff;
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
            var openAnchors = new List<AnchorId>();

            roomBackward.anchors.ForEach(x =>
            {
                if (x.CheckAchors() && !openAnchors.Contains(x)) {
                    openAnchors.Add(x);
                }
            });

            roomForward.anchors.ForEach(x =>
            {
                if (x.CheckAchors() && !openAnchors.Contains(x)) {
                    openAnchors.Add(x);
                }
            });

            toConnect = openAnchors.ToArray();
        }

        private void SwapRooms()
        {
            var temp = roomForward;
            roomForward = roomBackward;
            roomBackward = temp;
        }
    }
}