using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ObjectAbstraction.ModelChanger;
using ObjectPooling;
using UnityEngine;

namespace Visual
{
    /// <summary>
    /// Controls the transition between abstraction levels
    /// </summary>
    public class TransitionController : MonoBehaviour
    {
        [SerializeField] private Transform plane;
        [SerializeField] private VolumeCubeGenerator cubeGenerator;
        [SerializeField] private float cubeScale = 1;
        [SerializeField] private float transitionDuration = 1;
        [SerializeField] private float maxYPosition;
        [SerializeField] private float minYPosition;
        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private float maxDistance = 3.5f;
        [SerializeField] private float cubeStartScale;
        [SerializeField] private int batchAmount = 50;
        [Header("Gizmos"), SerializeField] private bool drawGizmos;
        [SerializeField] private float gizmoSize = 1f;
        [SerializeField] private float gizmoTransparency = 0.2f;
        
        private Coroutine coroutine;
        private Transform parentModelChanger;
        private Dictionary<int, MeshRenderer> currentRenderers = new Dictionary<int, MeshRenderer>();
        private bool isTransitioning;
        private int batchCount;
        private bool updateRunning;
        private Vector3 ToAbstractPos => parentModelChanger.position + (-plane.transform.forward * minYPosition);
        private Vector3 ToNormalPos => parentModelChanger.position + (-plane.transform.forward * maxYPosition);

        private void OnValidate()
        {
            if (!parentModelChanger) {
                var p = GetComponentInParent<AdvModelChanger>();
                if (p) {
                    parentModelChanger = p.transform;
                }
            }
        }

        private void Start()
        {
            parentModelChanger = GetComponentInParent<AdvModelChanger>().transform;
        }

        public void Init(bool toAbstract)
        {
            transform.position = toAbstract ? ToAbstractPos : ToNormalPos;
        }

        private void Update()
        {
            if (isTransitioning) {
                if (!updateRunning) {
                    UpdateCubeSpawns().Forget();
                }

                foreach (var pair in currentRenderers) {
                    pair.Value.material.SetVector("_TransitionPosition", plane.position);
                }
            }
        }

        private async UniTaskVoid UpdateCubeSpawns()
        {
            updateRunning = true;
            var maxDist = maxDistance + 0.5f;
            var positions = cubeGenerator.CubePositions;
            for (int i = 0; i < positions.Count; i++) {
                batchCount++;
                
                var pos = cubeGenerator.transform.TransformPoint(positions[i]);
                pos = parentModelChanger.worldToLocalMatrix.MultiplyPoint3x4(pos);
                var planePos = parentModelChanger.worldToLocalMatrix.MultiplyPoint3x4(plane.position);
                var dist = Mathf.Abs(pos.y - planePos.y);

                if (dist > maxDist && currentRenderers.ContainsKey(i)) {
                    var cube = currentRenderers[i];
                    cube.transform.parent = null;
                    currentRenderers.Remove(i);
                    CubePool.Instance.ReleaseObject(cube.GetComponent<CubeController>());
                    continue;
                }

                if (dist < maxDist && !currentRenderers.ContainsKey(i)) {
                    var cube = CubePool.Instance.GetObject();
                    cube.transform.parent = cubeGenerator.transform;
                    cube.transform.localPosition = positions[i];
                    cube.transform.localRotation = plane.localRotation;
                    cube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);
                    var renderer = cube.GetComponent<MeshRenderer>();
                    currentRenderers.Add(i, renderer);
                    SetCubeMaterial(renderer);
                }

                if (batchCount >= batchAmount) {
                    UniTask.Yield();
                    batchCount = 0;
                }
            }

            updateRunning = false;
        }

        private void SetCubeMaterial(MeshRenderer r)
        {
            r.material.SetVector("_TransitionPosition", plane.position);
            r.material.SetFloat("_AbsoluteScale", cubeStartScale);
            r.material.SetFloat("_MaxDistance", maxDistance);
            r.material.SetFloat("_MinDistance", minDistance);
            r.material.SetMatrix("_WorldToLocal", parentModelChanger.worldToLocalMatrix);
        }

        public void StartTransition(bool toAbstract, float duration = -1)
        {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                isTransitioning = false;
            }

            coroutine = StartCoroutine(MovePlane(toAbstract, duration > 0 ? duration : transitionDuration));
        }

        private IEnumerator MovePlane(bool isAbstract, float duration)
        {
            isTransitioning = true;
            var dir = plane.forward * Mathf.Abs(minYPosition - maxYPosition);
            Tween t;
            if (isAbstract) {
                t = transform.DOMove(ToAbstractPos, duration);
            }
            else {
                t = transform.DOMove(ToNormalPos, duration);
            }

            yield return t.WaitForCompletion();

            foreach (var pair in currentRenderers) {
                var cube = pair.Value;
                cube.transform.parent = null;
                CubePool.Instance.ReleaseObject(cube.GetComponent<CubeController>());
            }

            currentRenderers.Clear();
            isTransitioning = false;
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos || cubeGenerator.CubePositions.Count == 0) {
                return;
            }

            Gizmos.matrix = cubeGenerator.transform.localToWorldMatrix;
            foreach (var r in cubeGenerator.CubePositions) {
                Gizmos.color = new Color(0, 100, 250, gizmoTransparency);
                Gizmos.DrawCube(r, new Vector3(cubeScale, cubeScale, cubeScale));
            }

            Gizmos.matrix = parentModelChanger.localToWorldMatrix;
            Gizmos.color = new Color(100, 0, 100, 0.2f);
            Gizmos.DrawCube(new Vector3(0, maxYPosition, 0), new Vector3(gizmoSize, 0.08f, gizmoSize));
            Gizmos.DrawCube(new Vector3(0, minYPosition, 0), new Vector3(gizmoSize, 0.08f, gizmoSize));
        }
    }
}