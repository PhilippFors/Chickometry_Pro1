using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoomLoop.Portal
{
    public class PortalTeleporter : MonoBehaviour
    {
        private List<PortalTraveller> teleportQueue = new List<PortalTraveller>();
        private Portal portal;
        private Transform receiver;

        private void Awake()
        {
            portal = GetComponent<Portal>();
            receiver = portal.pairPortal.transform;
        }

        void LateUpdate()
        {
            if (teleportQueue.Count > 0) {
                for (int i = 0; i < teleportQueue.Count; i++) {
                    var traveller = teleportQueue[i];
                    var m = receiver.transform.localToWorldMatrix *
                            transform.worldToLocalMatrix *
                            traveller.transform.localToWorldMatrix;

                    var portalToPlayer = traveller.transform.position - transform.position;
                    var dot = Vector3.Dot(portalToPlayer, transform.forward);
                    var previousDot = Vector3.Dot(traveller.previousPortalOffset, transform.forward);

                    if (dot < 0f && previousDot > 0f) {
                        traveller.Teleport(transform, receiver.transform, m.GetColumn(3), m.rotation);
                        teleportQueue.RemoveAt(i);
                        i--;
                    }
                    else {
                        traveller.previousPortalOffset = portalToPlayer;
                    }
                }
            }
        }

        public void EnterPortal(PortalTraveller obj)
        {
            if (obj && !teleportQueue.Contains(obj)) {
                obj.previousPortalOffset = obj.transform.position - transform.position;
                teleportQueue.Add(obj);
            }
        }

        public void ExitPortal(PortalTraveller obj)
        {
            if (obj && teleportQueue.Contains(obj)) {
                teleportQueue.Remove(obj);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var obj = other.GetComponentInParent<PortalTraveller>();
            EnterPortal(obj);
        }

        void OnTriggerExit(Collider other)
        {
            var obj = other.GetComponentInParent<PortalTraveller>();
            ExitPortal(obj);
        }
    }
}