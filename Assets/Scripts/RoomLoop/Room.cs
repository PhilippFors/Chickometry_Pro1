using System.Linq;
using UnityEngine;

namespace RoomLoop
{
    public class Room : MonoBehaviour
    {
        public AnchorId[] Anchors => anchors;
        [SerializeField] private AnchorId[] anchors = new AnchorId[2];

        public bool HasAnchor(AnchorId id) => anchors.Contains(id);
    }
}