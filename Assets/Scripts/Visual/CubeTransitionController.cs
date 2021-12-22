using System.Collections.Generic;
using DG.Tweening;
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
        private BoxCollider bounds;
        [SerializeField] private List<MeshRenderer> cubes;
        
        private void Start()
        {
            foreach (var rend in cubes) {
                var seed = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
                rend.material.SetVector("_RandomSeed", seed);
                rend.material.SetFloat("_AbsoluteScale", startScale);
                rend.material.SetVector("_RandomRange", range);
                // rend.gameObject.SetActive(false);
            }
        }

        public void Enable()
        {
            foreach (var rend in cubes) {
                // rend.gameObject.SetActive(true);
                rend.material.DOFloat(1, "_AbsoluteScale", 0.5f);
            }
        }

        public void Disable()
        {
            foreach (var rend in cubes) {
                rend.material.DOFloat(0, "_AbsoluteScale", 0.5f);
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