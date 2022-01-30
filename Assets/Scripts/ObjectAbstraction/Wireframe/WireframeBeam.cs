using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.Wireframe
{
    /// <summary>
    /// Looks for objects in its path and extends a trigger to a certain distance.
    /// </summary>
    public class WireframeBeam : MonoBehaviour
    {
        public bool isEnabled = true;

        [SerializeField] private float length = 0.2f;
        [SerializeField] private float maxLength = 10;
        [SerializeField] private float growSpeed = 5;
        [SerializeField] private BoxCollider col;
        [SerializeField] private Transform geo;
        [SerializeField] private ParticleSystem[] particleSystems;
        [SerializeField] private LayerMask mask;
        [SerializeField] private List<WireframeBeamReflector> excludeReflectors;
        [SerializeField] private List<GameObject> ignore;

        private WireframeBeamReflector currentReflector;
        private float minLength = 0f;

        private void Start()
        {
            if (!isEnabled) {
                DisableBeam();
            }
        }

        private void Update()
        {
            ChangeLength(length);

            if (length > minLength) {
                EnableParticles();
            }
            else {
                StopParticles();
            }
            
            if (!isEnabled) {
                length = minLength;
                return;
            }

            var hits = Physics.RaycastAll(transform.position, transform.forward, maxLength, mask,
                QueryTriggerInteraction.Ignore);

            ProcessHits(hits);
        }

        private void ProcessHits(RaycastHit[] hits)
        {
            if (hits.Length > 0) {
                Array.Sort(hits, (hit1, hit2) => hit1.distance < hit2.distance ? 0 : 1);
                WireframeBeamReflector hitReflector = null;
                var maxDist = maxLength;

                for (int i = 0; i < hits.Length; i++) {
                    if (ignore.Contains(hits[i].transform.gameObject)) {
                        continue;
                    }

                    var wIdentifier = hits[i].transform.GetComponent<WireframeIdentifier>();
                    var wReflector = hits[i].transform.GetComponentInChildren<WireframeBeamReflector>();

                    if (wReflector && !excludeReflectors.Contains(wReflector)) {
                        hitReflector = wReflector;
                        currentReflector = wReflector;
                        maxDist = hits[i].distance;
                        break;
                    }

                    if (!wIdentifier) {
                        maxDist = hits[i].distance;
                        break;
                    }
                }

                length = maxDist;

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

        private void EnableParticles()
        {
            foreach (var p in particleSystems) {
                p.Play();
            }
        }

        private void StopParticles()
        {
            foreach (var p in particleSystems) {
                p.Stop();
            }
        }

        private void ChangeLength(float value)
        {
            var v = Mathf.Lerp(col.size.z, value, growSpeed * Time.deltaTime);
            col.size = new Vector3(col.size.x, col.size.y, v);
            col.center = new Vector3(0, 0, v / 2f);
            geo.transform.localScale = new Vector3(v, geo.transform.localScale.y, geo.transform.localScale.z);
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