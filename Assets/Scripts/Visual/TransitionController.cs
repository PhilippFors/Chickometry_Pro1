using System;
using System.Collections;
using DG.Tweening;
using ObjectAbstraction.ModelChanger;
using UnityEngine;

namespace Visual
{
    /// <summary>
    /// Controls the transition between abstraction levels
    /// </summary>
    public class TransitionController : MonoBehaviour
    {
        [SerializeField] private Transform plane;
        [SerializeField] private GameObject cubes;
        [SerializeField] private float cubeStartScale;
        [SerializeField] private bool drawGizmos;
        
        private BoxCollider bounds;
        private Coroutine coroutine;
        private Transform parentModelChanger;
        private MeshRenderer[] renderers;

        private void OnValidate()
        {
            if (cubes) {
                renderers = cubes.GetComponentsInChildren<MeshRenderer>();
            }
        }

        private void Awake()
        {
            renderers = cubes.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderers) {
                r.material.SetFloat("_AbsoluteScale", cubeStartScale);
            }
        }

        private void Start()
        {
            parentModelChanger = GetComponentInParent<AdvModelChanger>().transform;
        }

        public void Init(bool toAbstract, Vector2 minMaxY)
        {
            transform.position = parentModelChanger.position + new Vector3(0, toAbstract ? minMaxY.x : minMaxY.y);
        }

        private void Update()
        {
            foreach (var r in renderers) {
                r.material.SetVector("_TransitionPosition", plane.position);
                r.material.SetVector("_SelfPosition", r.transform.position);
            }
        }

        public void StartTransition(bool toAbstract, float startTime, float transitionDuration, Vector2 minMaxY)
        {
            if (coroutine != null) {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(MovePlane(toAbstract, startTime, transitionDuration, minMaxY));
        }

        private IEnumerator MovePlane(bool toAbstract, float startTime, float transitionDuration, Vector2 minMaxY)
        {
            Tween t;
            if (toAbstract) {
                t = transform.DOMove(parentModelChanger.position + new Vector3(0, minMaxY.x, 0), transitionDuration);
            }
            else {
                t = transform.DOMove(parentModelChanger.position + new Vector3(0, minMaxY.y, 0), transitionDuration);
            }

            yield return t.WaitForCompletion();
        }
        
        private void OnDrawGizmos()
        {
            if (!drawGizmos) {
                return;
            }
            
            foreach (var r in renderers) {
                Gizmos.color = new Color(0, 100, 250, 0.05f);
                Gizmos.DrawCube(r.transform.position, r.transform.localScale);
            }
        }
    }
}