using System.Collections.Generic;
using UnityEngine;
using Utlities;

namespace RoomLoop.Portal
{
    public class PortalTeleporter : MonoBehaviour
    {
        public bool canTravel;
        [SerializeField] private Collider blocker;
        
        private List<IPortalTraveller> teleportQueue = new List<IPortalTraveller>();
        private Portal portal;
        private Transform receiver;
        
        private static readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        
        private void Awake()
        {
            portal = GetComponentInParent<Portal>();
            receiver = portal.pairPortal.GetComponentInChildren<PortalTeleporter>().transform;
        }

        void LateUpdate()
        {
            if (!canTravel) {
                blocker.enabled = true;
                return;
            }

            blocker.enabled = false;

            if (teleportQueue.Count > 0) {
                for (int i = 0; i < teleportQueue.Count; i++) {
                    var traveller = teleportQueue[i];
                    if (!traveller.CanTravel) {
                        continue;
                    }
                    
                    var m = receiver.transform.localToWorldMatrix * transform.worldToLocalMatrix * traveller.GetTransform().localToWorldMatrix;
                    
                    //position
                    Vector3 relativePos = portal.transform.InverseTransformPoint(traveller.GetTransform().position);
                    relativePos = halfTurn * relativePos;
                    var newPosition = receiver.transform.TransformPoint(relativePos);

                    var rb = traveller.GetComponent<Rigidbody>();
                    Vector3 relativeVel = transform.InverseTransformDirection(rb.velocity); 
                    relativeVel = halfTurn * relativeVel;
                    var newVelocity = receiver.TransformDirection(relativeVel);
                    
                    var portalToPlayer = traveller.GetTransform().position - transform.position;
                    var dot = Vector3.Dot(portalToPlayer, transform.forward);
                    var previousDot = Vector3.Dot(traveller.PreviousPortalOffset, transform.forward);

                    if (dot < 0f && previousDot > 0f) {
                        traveller.Teleport(transform, receiver.transform, newPosition, m.rotation, newVelocity);
                        teleportQueue.RemoveAt(i);
                        i--;
                    }
                    else {
                        traveller.PreviousPortalOffset = portalToPlayer;
                    }
                }
            }
        }

        public void EnterPortal(IPortalTraveller obj)
        {
            if (obj != null && !teleportQueue.Contains(obj)) {
                obj.PreviousPortalOffset = obj.GetTransform().position - transform.position;
                teleportQueue.Add(obj);
            }
        }

        public void ExitPortal(IPortalTraveller obj)
        {
            if (obj != null && teleportQueue.Contains(obj)) {
                teleportQueue.Remove(obj);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var obj = other.GetComponentInParent<IPortalTraveller>();
            EnterPortal(obj);
        }

        void OnTriggerExit(Collider other)
        {
            var obj = other.GetComponentInParent<IPortalTraveller>();
            ExitPortal(obj);
        }
    }
}