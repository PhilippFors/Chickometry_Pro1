using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ObjectAbstraction.ModelChanger;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Visual
{
    /// <summary>
    /// Used to generate a random amount of cubes
    /// </summary>
    public class CubeTransitionController : MonoBehaviour
    {
        public int cubeAmount;
        public float startScale = 1;
        public GameObject cubePrefab;
        public BoxCollider excludeBounds;
        [SerializeField] private List<MeshRenderer> cubes;

        private BoxCollider bounds;
        private Coroutine coroutine;
        private int AbsoluteScaleId => Shader.PropertyToID("_AbsoluteScale");
        private Transform parentModelChanger;

        private void Start()
        {
            // foreach (var rend in cubes) {
            //     rend.material.SetFloat(AbsoluteScaleId, startScale);
            // }

            parentModelChanger = GetComponentInParent<AdvModelChanger>().transform;
        }

        public void Init(bool toAbstract, Vector2 minMaxY)
        {
            transform.position = parentModelChanger.position + new Vector3(0, toAbstract ? minMaxY.x : minMaxY.y);
        }

        private void Update()
        {
            Shader.SetGlobalVector("_TransitionPosition", transform.position);
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


#if UNITY_EDITOR
        [Button]
        private void Generate()
        {
            if (cubes == null) {
                cubes = new List<MeshRenderer>();
            }

            KillChildren();
            bounds = GetComponent<BoxCollider>();

            for (int i = 0; i < cubeAmount; i++) {
                var pos = FindRandomInArea();
                if (excludeBounds) {
                    while (excludeBounds.bounds.Contains(pos)) {
                        pos = FindRandomInArea();
                    }
                }

                var obj = Instantiate(cubePrefab, pos, Quaternion.identity, transform).GetComponent<MeshRenderer>();
                cubes.Add(obj);
            }
        }

        private void KillChildren()
        {
            for (int i = 0; i < cubes.Count; i++) {
                if (!Application.isPlaying) {
                    DestroyImmediate(cubes[i].gameObject);
                }
                else {
                    Destroy(cubes[i].gameObject);
                }
            }

            cubes.Clear();
        }

        private Vector3 FindRandomInArea()
        {
            return new Vector3(
                Random.Range(
                    gameObject.transform.position.x - gameObject.transform.localScale.x * bounds.size.x * 0.5f,
                    gameObject.transform.position.x + gameObject.transform.localScale.x * bounds.size.x * 0.5f),
                Random.Range(
                    gameObject.transform.position.y - gameObject.transform.localScale.y * bounds.size.y * 0.5f,
                    gameObject.transform.position.y + gameObject.transform.localScale.y * bounds.size.y * 0.5f),
                Random.Range(
                    gameObject.transform.position.z - gameObject.transform.localScale.z * bounds.size.z * 0.5f,
                    gameObject.transform.position.z + gameObject.transform.localScale.z * bounds.size.z * 0.5f)
            );
        }
#endif
    }
}