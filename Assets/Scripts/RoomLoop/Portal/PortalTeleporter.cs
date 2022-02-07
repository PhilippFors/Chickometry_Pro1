using System.Collections.Generic;
using UnityEngine;
using Utlities;

namespace RoomLoop.Portal
{
    public class PortalTeleporter : MonoBehaviour
    {    
        private readonly List<IPortalTraveller> teleportQueue = new List<IPortalTraveller>();
        private Portal portal;
        private Transform Receiver =>  portal.PairPortalTeleporter.GetComponentInChildren<PortalTeleporter>().transform;
        private readonly Quaternion halfTurn = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        
        private void Awake()
        {
            portal = GetComponentInParent<Portal>();
        }

        void LateUpdate()
        {
            if (teleportQueue.Count > 0) {
                for (int i = 0; i < teleportQueue.Count; i++) {
                    var traveller = teleportQueue[i];
                    if (!traveller.CanTravel) {
                        continue;
                    }
                    
                    var receiverTransform = Receiver;
                    var m = receiverTransform.localToWorldMatrix * transform.localToWorldMatrix * traveller.GetTransform().localToWorldMatrix;
                    
                    Vector3 relativePos = transform.InverseTransformPoint(traveller.GetTransform().position);
                    relativePos = halfTurn * relativePos;
                    var newPosition = receiverTransform.TransformPoint(relativePos);

                    var rb = traveller.GetComponent<Rigidbody>();
                    Vector3 relativeVel = transform.InverseTransformDirection(rb.velocity); 
                    relativeVel = halfTurn * relativeVel;
                    var newVelocity = receiverTransform.TransformDirection(relativeVel);
                    
                    Quaternion relativeRot = Quaternion.Inverse(transform.rotation) * traveller.GetTransform().rotation;
                    relativeRot = halfTurn * relativeRot;
                    var newRot = receiverTransform.rotation * relativeRot;
                    
                    var portalToPlayer = traveller.GetTransform().position - transform.position;
                    var dot = Vector3.Dot(portalToPlayer, transform.forward);
                    var previousDot = Vector3.Dot(traveller.PreviousPortalOffset, transform.forward);

                    if (dot < 0f && previousDot > 0f) {
                        traveller.Teleport(transform, receiverTransform, newPosition, newRot, newVelocity);
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