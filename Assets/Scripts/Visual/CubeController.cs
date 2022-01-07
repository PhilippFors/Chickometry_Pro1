using UnityEngine;

namespace Visual
{
    public class CubeController : MonoBehaviour
    {
        public float startScale = 1;
        private MeshRenderer rend;
        private int AbsoluteScaleId => Shader.PropertyToID("_AbsoluteScale");
        private void Awake()
        {
            rend = GetComponent<MeshRenderer>();
            
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