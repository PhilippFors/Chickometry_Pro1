using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Visual
{
    public class CubeController : MonoBehaviour
    {
        public float startScale = 1;
        private MeshRenderer rend;
        private int RandomSeedId => Shader.PropertyToID("_RandomSeed");
        private int AbsoluteScaleId => Shader.PropertyToID("_AbsoluteScale");
        private int RandomRangeId => Shader.PropertyToID("_RandomRange");
        private void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            
            var seed = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
            rend.material.SetVector(RandomSeedId, seed);
            rend.material.SetFloat(AbsoluteScaleId, startScale);
        }

        private void Update()
        {
            rend.material.SetVector("_SelfPosition", transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 100, 250, 0.1f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}