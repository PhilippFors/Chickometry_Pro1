using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.Wireframe
{
    public class WireframeBeam : MonoBehaviour
    {
        public bool isEnabled = true;

        [SerializeField] private float length = 0.2f;
        [SerializeField] private float maxLength = 10;
        [SerializeField] private float growSpeed = 5;
        [SerializeField] private BoxCollider col;
        [SerializeField] private LayerMask mask;
        [SerializeField] private List<WireframeBeamReflector> excludeReflectors;
        [SerializeField] private List<GameObject> ignore;

        private WireframeBeamReflector currentReflector;
        private float minLength = 0.2f;

        private void Start()
        {
            if (!isEnabled) {
                DisableBeam();
            }
        }

        private void Update()
        {
            ChangeLength(length);

            if (!isEnabled) {
                return;
            }

            var hits = Physics.RaycastAll(transform.position, transform.forward, maxLength, mask, QueryTriggerInteraction.Ignore);

            ProcessHits(hits);
        }

        private void ProcessHits(RaycastHit[] hits)
        {
            if (hits.Length > 0) {
                Array.Sort(hits, (hit1, hit2) => hit1.distance < hit2.distance ? 0 : 1);
                WireframeBeamReflector hitReflector = null;
                
                for (int i = 0; i < hits.Length; i++) {
                    if (ignore.Contains(hits[i].transform.gameObject)) {
                        continue;
                    }
                    var wIdentifier = hits[i].transform.GetComponent<WireframeIdentifier>();
                    var wReflector = hits[i].transform.GetComponent<WireframeBeamReflector>();

                    if (wReflector && !excludeReflectors.Contains(wReflector)) {
                        hitReflector = wReflector;
                        currentReflector = wReflector;
                        length = hits[i].distance;
                        break;
                    }

                    if (!wIdentifier) {
                        length = hits[i].distance;
                        break;
                    }
                }

                if (!hitReflector) {
                    DisableReflector();
                }
            }
            else {
                DisableReflector();

                length = maxLength;
            }
            
            EnableReflector();
        }

        private void DisableReflector()
        {
            if (currentReflector) {
                currentReflector.DisableBeams();
                currentReflector = null;
            }
        }

        private void EnableReflector()
        {
            if (currentReflector) {
                currentReflector.EnableBeams();
            }
        }
        
        private void ChangeLength(float value)
        {
            var v = Mathf.Lerp(col.size.z, value, growSpeed * Time.deltaTime);
            col.size = new Vector3(col.size.x, col.size.y, v);
            col.center = new Vector3(0, 0, v / 2f);
        }

        private void OnValidate()
        {
            ChangeLength(length);
        }

        [Button]
        public void EnableBeam()
        {
            isEnabled = true;
        }

        [Button]
        public void DisableBeam()
        {
            isEnabled = false;
            length = minLength;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 0, 0.8f, 0.2f);
            Gizmos.matrix = transform.localToWorldMatrix;
            if (col) {
                Gizmos.DrawWireCube(col.center, col.size);
                Gizmos.DrawCube(col.center, col.size);
            }
        }
    }
}