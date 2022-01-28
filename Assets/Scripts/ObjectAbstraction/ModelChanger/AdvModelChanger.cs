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
        public MeshRenderer NormalRend => normalRend;
        public MeshRenderer AbstractRend => abstractRend;
        public bool IsAbstract => isAbstract;

        [SerializeField] private bool useSlicePlane;
        [SerializeField] private bool useSimpleTransition;
        [SerializeField, ShowIf("useSlicePlane")] private TransitionController plane;
        [SerializeField] private bool isAbstract;
        [SerializeField] public bool shootable = true;
        [SerializeField] private MeshFilter normalMeshFilter;
        [SerializeField] private MeshFilter abstractMeshFilter;
        [SerializeField] private ModelSettings normalModel;
        [SerializeField] private ModelSettings abstractModel;

        private MeshRenderer normalRend;
        private MeshRenderer abstractRend;
        private MeshCollider meshCollider;
        private GameObject previousColliders;

        private void Awake()
        {
            originalAbstraction = IsAbstract;
            OriginalPosition = transform.position;
            OriginalRotation = transform.rotation;

            meshCollider = GetComponent<MeshCollider>();
            normalRend = normalMeshFilter.GetComponent<MeshRenderer>();
            abstractRend = abstractMeshFilter.GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            if (useSlicePlane) {
                plane.Init(isAbstract);
                return;
            }

            if (isAbstract) {
                EnableAbstractLayer(useSimpleTransition);
            }
            else {
                EnableNormalLayer(useSimpleTransition);
            }
        }

        public void ToggleModels()
        {
            if (isAbstract) {
                EnableNormalLayer(useSimpleTransition);
            }
            else {
                EnableAbstractLayer(useSimpleTransition);
            }

            isAbstract = !isAbstract;
        }

        private void EnableNormalLayer(bool instant = false)
        {
            normalModel.ApplyMeshCollider(meshCollider, normalMeshFilter);
            normalModel.ApplyCollider(ref previousColliders);
            normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            Transition(false, instant);
        }

        private void EnableAbstractLayer(bool instant = false)
        {
            abstractModel.ApplyMeshCollider(meshCollider, abstractMeshFilter);
            abstractModel.ApplyCollider(ref previousColliders);
            abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            Transition(true, instant);
        }

        private void Transition(bool toAbstract, bool instant = false)
        {
            if (useSlicePlane) {
                if (instant) {
                    plane.StartTransition(toAbstract, 0.1f);
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
            meshCollider = GetComponent<MeshCollider>();
            if (isAbstract) {
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
            Gizmos.matrix = transform.localToWorldMatrix;

        }
    }
}