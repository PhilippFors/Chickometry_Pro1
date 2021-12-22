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
        public Vector2 range;
        public GameObject cubePrefab;
        public BoxCollider excludeBounds;
        [SerializeField] private List<MeshRenderer> cubes;

        private BoxCollider bounds;
        private Coroutine coroutine;
        private int RandomSeedId => Shader.PropertyToID("_RandomSeed");
        private int AbsoluteScaleId => Shader.PropertyToID("_AbsoluteScale");
        private int RandomRangeId => Shader.PropertyToID("_RandomRange");
        private Transform parentModelChanger;

        private void Start()
        {
            foreach (var rend in cubes) {
                var seed = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
                rend.material.SetVector(RandomSeedId, seed);
                rend.material.SetFloat(AbsoluteScaleId, startScale);
                rend.material.SetVector(RandomRangeId, range);
            }

            parentModelChanger = GetComponentInParent<AdvModelChanger>().transform;
        }

        public void Init(bool toAbstract, Vector2 minMaxY)
        {
            transform.position = parentModelChanger.position + new Vector3(0, toAbstract ? minMaxY.x : minMaxY.y);
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
            Enable(startTime);

            Tween t;
            if (toAbstract) {
                t = transform.DOMove(parentModelChanger.position + new Vector3(0, minMaxY.x, 0), transitionDuration);
            }
            else {
                t = transform.DOMove(parentModelChanger.position + new Vector3(0, minMaxY.y, 0), transitionDuration);
            }

            yield return t.WaitForCompletion();

            Disable(startTime);
        }

        private void Enable(float startTime = 0.2f)
        {
            foreach (var rend in cubes) {
                rend.material.DOFloat(1, AbsoluteScaleId, startTime);
            }
        }

        private void Disable(float startTime = 0.2f)
        {
            foreach (var rend in cubes) {
                rend.material.DOFloat(0, AbsoluteScaleId, startTime);
            }
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