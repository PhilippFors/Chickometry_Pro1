using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Checkpoints;
using UnityEngine;

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
                StartCoroutine(EnableAbstractLayer());
            }
            else {
                StartCoroutine(EnableNormalLayer());
            }
        }

        public void ToggleModels()
        {
            if (isAbstract) {
                StartCoroutine(EnableNormalLayer());
            }
            else {
                StartCoroutine(EnableAbstractLayer());
            }

            isAbstract = !isAbstract;
        }

        private IEnumerator EnableNormalLayer()
        {
            yield return StartCoroutine(Transition(false));
            normalModel.ApplyMeshCollider(meshCollider, normalMeshFilter);
            normalModel.ApplyCollider(ref previousColliders);
            normalModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
        }

        private IEnumerator EnableAbstractLayer()
        {
            yield return StartCoroutine(Transition(true));
            abstractModel.ApplyMeshCollider(meshCollider, abstractMeshFilter);
            abstractModel.ApplyCollider(ref previousColliders);
            abstractModel.ApplyRigidbodySettings(GetComponent<Rigidbody>());
        }

        private IEnumerator Transition(bool toAbstract)
        {
            if (toAbstract) {
                MaterialTransitions(normalMat.materials, 1);
                MaterialTransitions(abstractMat.materials, 0);
            }
            else {
                MaterialTransitions(normalMat.materials, 0);
                MaterialTransitions(abstractMat.materials, 1);
            }

            yield return new WaitForSeconds(0.5f);
        }

        public void MaterialTransitions(Material[] mats, float endValue)
        {
            foreach (var mat in mats) {
                mat.DOFloat(endValue, "_CutoffValue", 0.5f);
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
    }
}