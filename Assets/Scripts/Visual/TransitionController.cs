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
        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private float maxDistance = 3.5f;
        [SerializeField] private float cubeStartScale;
        [SerializeField] private bool drawGizmos;

        private BoxCollider bounds;
        private Coroutine coroutine;
        private Transform parentModelChanger;
        private Dictionary<int, MeshRenderer> currentRenderers = new Dictionary<int, MeshRenderer>();
        private bool isTransitioning;
        public int batchAmount = 50;
        private int batchCount;
        private bool updateRunning;
        private void OnValidate()
        {
            // if (cubes) {
            //     currentRenderers = cubes.GetComponentsInChildren<MeshRenderer>();
            // }
        }

        private void Awake()
        {
            // currentRenderers = cubes.GetComponentsInChildren<MeshRenderer>();
            // foreach (var r in currentRenderers) {
                // r.material.SetFloat("_AbsoluteScale", cubeStartScale);
                // r.material.SetFloat("_MaxDistance", maxDistance);
            // }
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
            for(int i = 0; i < positions.Count; i++) {
                batchCount++;
                var pos = cubeGenerator.transform.TransformPoint(positions[i]);
                var dist = Mathf.Abs(pos.y - plane.position.y);
                
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
                    cube.transform.rotation = plane.rotation;
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
            r.material.SetVector("_SelfPosition", r.transform.position);
            r.material.SetFloat("_AbsoluteScale", cubeStartScale);
            r.material.SetFloat("_MaxDistance", maxDistance);
            r.material.SetFloat("_MinDistance", minDistance);
        }

        public void StartTransition(bool toAbstract, float startTime, float transitionDuration, Vector2 minMaxY)
        {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                isTransitioning = false;
            }

            coroutine = StartCoroutine(MovePlane(toAbstract, startTime, transitionDuration, minMaxY));
        }

        private IEnumerator MovePlane(bool toAbstract, float startTime, float transitionDuration, Vector2 minMaxY)
        {
            isTransitioning = true;
            var dir = plane.forward * Mathf.Abs(minMaxY.x - minMaxY.y);
            Tween t;
            if (toAbstract) {
                t = transform.DOMove(parentModelChanger.position + dir, transitionDuration);
            }
            else {
                t = transform.DOMove(parentModelChanger.position - dir, transitionDuration);
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
                Gizmos.color = new Color(0, 100, 250, 0.05f);
                Gizmos.DrawCube(r, new Vector3(cubeScale, cubeScale, cubeScale));
            }
        }
    }
}