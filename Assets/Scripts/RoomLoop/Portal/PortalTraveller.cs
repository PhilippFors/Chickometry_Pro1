using UnityEngine;

namespace RoomLoop.Portal
{
    public class PortalTraveller : MonoBehaviour
    {
        public Vector3 previousPortalOffset;
        
        public virtual void Teleport(Transform inPortal, Transform outPortal, Vector3 pos, Quaternion rot)
        {
        }
    }
}
