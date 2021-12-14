using UnityEngine;

namespace RoomLoop.Portal
{
    public interface IPortalTraveller
    {
        public Vector3 PreviousPortalOffset { get; set; }
        public bool CanTravel { get; }
        public void Teleport(Transform inPortal, Transform outPortal, Vector3 pos, Quaternion rot, Vector3 velocity);
    }
}
