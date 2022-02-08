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
            get => switchable;
            set => switchable = value;
        }

        public List<Mesh> Models => new List<Mesh> {normalMeshFilter.mesh, abstractMeshFilter.mesh};
        public MeshRenderer NormalRend => normalRend;
        public MeshRenderer AbstractRend => abstractRend;
        public bool IsAbstract => isAbstract;

        [SerializeField] private bool useSlicePlane;
        [SerializeField] private bool useSimpleTransition;
        [SerializeField, ShowIf("useSlicePlane")] private TransitionController plane;

        [SerializeField] private bool isAbstract;
        [SerializeField] public bool switchable = true;
        [SerializeField] private bool useNormSkinnedMesh;
        [SerializeField, ShowIf("useNormSkinnedMesh")] private SkinnedMeshRenderer normalSkinnedMesh;
        [SerializeField, HideIf("useNormSkinnedMesh")] private MeshFilter normalMeshFilter;

        [SerializeField] private bool useAbstrSkinnedMesh;
        [SerializeField, ShowIf("useAbstrSkinnedMesh")] private SkinnedMeshRenderer abstractSkinnedMesh;
        [SerializeField, HideIf("useAbstrSkinnedMesh")] private MeshFilter abstractMeshFilter;

        [SerializeField] private ModelSettings normalModel;
        [SerializeField] private ModelSettings abstractModel;

        private Mesh AbstrMesh => useAbstrSkinnedMesh ? abstractSkinnedMesh.sharedMesh : abstractMeshFilter.mesh;
        private Mesh NormalMesh => useNormSkinnedMesh ? normalSkinnedMesh.sharedMesh : normalMeshFilter.mesh;
        private MeshRenderer normalRend;
        private MeshRenderer abstractRend;
        private Rigidbody rb;
        private GameObject previousColliders;
        
        private void Awake()
        {
            originalAbstraction = IsAbstract;
            OriginalPosition = transform.position;
            OriginalRotation = transform.rotation;
            rb = GetComponent<Rigidbody>();
            if (!useNormSkinnedMesh) {
                normalRend = normalMeshFilter.GetComponent<MeshRenderer>();
            }

            if (!useAbstrSkinnedMesh) {
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
            if (isAbstract) {
                ApplySettings(normalModel, NormalMesh);
                yield return null;
                ApplySettings(abstractModel, AbstrMesh);
            }
            else {
                ApplySettings(abstractModel, AbstrMesh);
                yield return null;
                ApplySettings(normalModel, NormalMesh);
            }
        }

        public void ToggleModels()
        {
            if (isAbstract && switchable) {
                EnableNormalLayer(useSimpleTransition);
            }
            else {
                EnableAbstractLayer(useSimpleTransition);
            }

            isAbstract = !isAbstract;
        }

        private void EnableNormalLayer(bool instant = false)
        {
            ApplySettings(normalModel, NormalMesh);
            Transition(false, instant);
        }

        private void EnableAbstractLayer(bool instant = false)
        {
            ApplySettings(abstractModel, AbstrMesh);
            Transition(true, instant);
        }

        private void ApplySettings(ModelSettings modelSettings, Mesh mesh)
        {
           modelSettings.ApplyCollider(ref previousColliders);
           // modelSettings.ApplyMeshCollider(GetComponent<MeshCollider>(), mesh);
           modelSettings.ApplyRigidbodySettings(rb);
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
                abstractModel.ApplyCollider(ref previousColliders);
                abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
            else {
                normalModel.ApplyCollider(ref previousColliders);
                normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
        }
    }
}