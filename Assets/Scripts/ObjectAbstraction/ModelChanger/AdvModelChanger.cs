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
    public partial class AdvModelChanger : MonoBehaviour, IModelChanger, IResettableItem
    {
        public bool Shootable
        {
            get => shootable;
            set => shootable = value;
        }

        public List<Mesh> Models => new List<Mesh> {normalMeshFilter.mesh, abstractMeshFilter.mesh};
        public MeshRenderer NormalRend => normalRend;
        public MeshRenderer AbstractRend => abstractRend;
        public bool IsAbstract => isAbstract;

        [SerializeField] private bool useSlicePlane;
        [SerializeField] private bool useSimpleTransition;

        [SerializeField, ShowIf("useSlicePlane")]
        private TransitionController plane;

        [SerializeField] private bool isAbstract;
        [SerializeField] public bool shootable = true;

        [SerializeField] private bool useNormalSkinnedMeshRenderer;

        [SerializeField, ShowIf("useNormalSkinnedMeshRenderer")]
        private SkinnedMeshRenderer normalSkinnedMeshRenderer;

        [SerializeField, HideIf("useNormalSkinnedMeshRenderer")]
        private MeshFilter normalMeshFilter;

        [SerializeField] private bool useAbstractSkinnedMeshRenderer;

        [SerializeField, ShowIf("useAbstractSkinnedMeshRenderer")]
        private SkinnedMeshRenderer abstractSkinnedMeshRenderer;

        [SerializeField, HideIf("useAbstractSkinnedMeshRenderer")]
        private MeshFilter abstractMeshFilter;

        [SerializeField] private ModelSettings normalModel;
        [SerializeField] private ModelSettings abstractModel;

        private MeshRenderer normalRend;
        private MeshRenderer abstractRend;

        private GameObject previousColliders;

        private void Awake()
        {
            originalAbstraction = IsAbstract;
            OriginalPosition = transform.position;
            OriginalRotation = transform.rotation;

            if (!useNormalSkinnedMeshRenderer) {
                normalRend = normalMeshFilter.GetComponent<MeshRenderer>();
            }

            if (!useAbstractSkinnedMeshRenderer) {
                abstractRend = abstractMeshFilter.GetComponent<MeshRenderer>();
            }
        }

        private void Start()
        {
            if (useSlicePlane) {
                plane.Init(isAbstract);
            }

            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            var abstrMesh = useAbstractSkinnedMeshRenderer
                ? abstractSkinnedMeshRenderer.sharedMesh
                : abstractMeshFilter.mesh;
            var normalMesh = useNormalSkinnedMeshRenderer
                ? normalSkinnedMeshRenderer.sharedMesh
                : normalMeshFilter.mesh;
            
            if (isAbstract) {
                normalModel.ApplyCollider(ref previousColliders);
                normalModel.ApplyMeshCollider(GetComponent<MeshCollider>(), normalMesh);
                normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
                yield return null;
                abstractModel.ApplyCollider(ref previousColliders);
                abstractModel.ApplyMeshCollider(GetComponent<MeshCollider>(), abstrMesh);
                abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
            else {
                abstractModel.ApplyCollider(ref previousColliders);
                abstractModel.ApplyMeshCollider(GetComponent<MeshCollider>(), abstrMesh);
                abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
                yield return null;
                normalModel.ApplyCollider(ref previousColliders);
                normalModel.ApplyMeshCollider(GetComponent<MeshCollider>(), normalMesh);
                normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
        }

        public void ToggleModels()
        {
            if (isAbstract && shootable) {
                EnableNormalLayer(useSimpleTransition);
            }
            else {
                EnableAbstractLayer(useSimpleTransition);
            }

            isAbstract = !isAbstract;
        }

        private void EnableNormalLayer(bool instant = false)
        {
            var normalMesh = useNormalSkinnedMeshRenderer
                ? normalSkinnedMeshRenderer.sharedMesh
                : normalMeshFilter.mesh;
            normalModel.ApplyCollider(ref previousColliders);
            normalModel.ApplyMeshCollider(GetComponent<MeshCollider>(), normalMesh);
            normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            Transition(false, instant);
        }

        private void EnableAbstractLayer(bool instant = false)
        {
            var abstrMesh = useAbstractSkinnedMeshRenderer
                ? abstractSkinnedMeshRenderer.sharedMesh
                : abstractMeshFilter.mesh;
            abstractModel.ApplyCollider(ref previousColliders);
            abstractModel.ApplyMeshCollider(GetComponent<MeshCollider>(), abstrMesh);
            abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            Transition(true, instant);
        }

        private void Transition(bool toAbstract, bool instant = false)
        {
            if (useSlicePlane) {
                if (instant) {
                    plane.StartTransition(toAbstract, 0f);
                    return;
                }

                plane.StartTransition(toAbstract);
            }
            else {
                if (toAbstract) {
                    if (instant) {
                        normalRend.enabled = false;
                        abstractRend.enabled = true;
                        return;
                    }
                    
                    MaterialTransitions(normalRend.materials, 1);
                    MaterialTransitions(abstractRend.materials, 0);
                }
                else {
                    if (instant) {
                        normalRend.enabled = true;
                        abstractRend.enabled = false;
                        return;
                    }

                    MaterialTransitions(normalRend.materials, 0);
                    MaterialTransitions(abstractRend.materials, 1);
                }
            }
        }

        private void MaterialTransitions(Material[] mats, float endValue)
        {
            foreach (var mat in mats) {
                mat.DOFloat(endValue, "_CutoffValue", 0.5f);
            }
        }

        private void OnValidate()
        {
            if (isAbstract) {
                // abstractModel.ApplyMeshCollider(GetComponent<MeshCollider>(), abstractMeshFilter);
                abstractModel.ApplyCollider(ref previousColliders);
                abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
            else {
                // normalModel.ApplyMeshCollider(GetComponent<MeshCollider>(), normalMeshFilter);
                normalModel.ApplyCollider(ref previousColliders);
                normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
        }
    }
}