using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Checkpoints;
using Sirenix.OdinInspector;
using UnityEngine;
using Visual;

namespace ObjectAbstraction.ModelChanger
{
    /// <summary>
    /// Advanced version of the model changer with more settings and functionality
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public partial class AdvModelChanger : MonoBehaviour, IModelChanger, IResettableItem
    {
        public bool Shootable
        {
            get => shootable;
            set => shootable = value;
        }

        public List<Mesh> Models => new List<Mesh> {normalMeshFilter.mesh, abstractMeshFilter.mesh};
        public MeshRenderer NormalRend => normalMat;
        public MeshRenderer AbstractRend => abstractMat;
        public bool IsAbstract => isAbstract;

        [SerializeField] private bool useSlicePlane;
        [SerializeField] private bool useSimpleTransition;
        [SerializeField] private float transitionDuration = 0.5f;
        [SerializeField, ShowIf("useSlicePlane")] private CubeTransitionController plane;
        [SerializeField, ShowIf("useSlicePlane")] private float maxYPosition;
        [SerializeField, ShowIf("useSlicePlane")] private float minYPosition;
        [SerializeField] private bool isAbstract;
        [SerializeField] public bool shootable = true;
        [SerializeField] private MeshFilter normalMeshFilter;
        [SerializeField] private MeshFilter abstractMeshFilter;
        [SerializeField] private ModelSettings normalModel;
        [SerializeField] private ModelSettings abstractModel;

        private MeshRenderer normalMat;
        private MeshRenderer abstractMat;
        private MeshCollider meshCollider;
        private GameObject previousColliders;

        private void Awake()
        {
            originalAbstraction = IsAbstract;
            OriginalPosition = transform.position;
            OriginalRotation = transform.rotation;

            meshCollider = GetComponent<MeshCollider>();
            normalMat = normalMeshFilter.GetComponent<MeshRenderer>();
            abstractMat = abstractMeshFilter.GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            if (isAbstract) {
                StartCoroutine(EnableAbstractLayer(true));
            }
            else {
                StartCoroutine(EnableNormalLayer(true));
            }
        }

        public void ToggleModels()
        {
            if (isAbstract) {
                StartCoroutine(EnableNormalLayer(useSimpleTransition));
            }
            else {
                StartCoroutine(EnableAbstractLayer(useSimpleTransition));
            }

            isAbstract = !isAbstract;
        }

        private IEnumerator EnableNormalLayer(bool instant = false)
        {
            yield return StartCoroutine(Transition(false, instant));
            normalModel.ApplyMeshCollider(meshCollider, normalMeshFilter);
            normalModel.ApplyCollider(ref previousColliders);
            normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
        }

        private IEnumerator EnableAbstractLayer(bool instant = false)
        {
            yield return StartCoroutine(Transition(true, instant));
            abstractModel.ApplyMeshCollider(meshCollider, abstractMeshFilter);
            abstractModel.ApplyCollider(ref previousColliders);
            abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
        }

        private IEnumerator Transition(bool toAbstract, bool instant = false)
        {
            if (useSlicePlane) {
                if (toAbstract) {
                    if (instant) {
                        plane.transform.DOMove(transform.position + new Vector3(0, minYPosition, 0), 0.1f);
                        yield break;
                    }
                    
                    plane.Enable();
                    yield return new WaitForSeconds(0.3f);
                    plane.transform.DOMove(transform.position + new Vector3(0, minYPosition, 0), transitionDuration).onComplete += () => plane.Disable();
                    plane.GetComponentInChildren<ParticleSystem>().Play();
                }
                else {
                    if (instant) {
                        plane.transform.DOMove(transform.position + new Vector3(0, maxYPosition, 0), 0.1f);
                        yield break;
                    }
                    
                    plane.Enable();
                    yield return new WaitForSeconds(0.3f);
                    plane.transform.DOMove(transform.position + new Vector3(0, maxYPosition, 0), transitionDuration).onComplete += () => plane.Disable();
                    plane.GetComponentInChildren<ParticleSystem>().Play();
                }
            }
            else {
                if (toAbstract) {
                    if (instant) {
                        normalMat.enabled = false;
                        abstractMat.enabled = true;
                        yield break;
                    }

                    MaterialTransitions(normalMat.materials, 1);
                    MaterialTransitions(abstractMat.materials, 0);
                }
                else {
                    if (instant) {
                        normalMat.enabled = true;
                        abstractMat.enabled = false;
                        yield break;
                    }

                    MaterialTransitions(normalMat.materials, 0);
                    MaterialTransitions(abstractMat.materials, 1);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        public void MaterialTransitions(Material[] mats, float endValue)
        {
            foreach (var mat in mats) {
                mat.DOFloat(endValue, "_CutoffValue", transitionDuration);
            }
        }

        private void OnValidate()
        {
            meshCollider = GetComponent<MeshCollider>();
            if (isAbstract) {
                EnableAbstractLayer();
                abstractModel.ApplyMeshCollider(meshCollider, abstractMeshFilter);
                abstractModel.ApplyCollider(ref previousColliders);
                abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
            else {
                normalModel.ApplyMeshCollider(meshCollider, normalMeshFilter);
                normalModel.ApplyCollider(ref previousColliders);
                normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(100, 0, 100, 0.2f);
            Gizmos.DrawCube(transform.position + new Vector3(0, maxYPosition, 0), new Vector3(2, 0.05f, 2));
            Gizmos.DrawCube(transform.position + new Vector3(0, minYPosition, 0), new Vector3(2, 0.05f, 2));
        }
    }
}