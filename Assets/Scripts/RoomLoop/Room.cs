using UnityEngine;

namespace RoomLoop
{
    public class Room : MonoBehaviour
    {
        public AnchorId[] Anchors => anchors;
        [SerializeField] private AnchorId[] anchors = new AnchorId[2];

        public bool HasAnchor(AnchorId id)
        {
            foreach (var i in anchors) {
                if (i == id) {
                    return true;
                }
            }

            return false;
        }
    }
}