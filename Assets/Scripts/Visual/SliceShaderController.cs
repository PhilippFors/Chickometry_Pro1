using UnityEngine;

namespace Visual
{
    /// <summary>
    /// Controls the slice plane parameters on the mesh it is assigned to
    /// </summary>
    public class SliceShaderController : MonoBehaviour
    {
        [SerializeField] private bool reverse;
        [SerializeField] private bool isSkinnedMesh;
        [SerializeField] private GameObject plane;

        private MeshRenderer meshRenderer;
        private SkinnedMeshRenderer skinnedMeshRenderer;
        private Vector3 oldPlanePos;
        private int slicePlanePos;
        private int slicePlaneDir;
        private void Awake()
        {
            slicePlanePos = Shader.PropertyToID("_SlicePlanePos");
            slicePlaneDir = Shader.PropertyToID("_SlicePlaneDir");
            oldPlanePos = plane.transform.position;
            if (isSkinnedMesh) {
                skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
                ApplySettings(skinnedMeshRenderer.materials);
            }
            else {
                meshRenderer = GetComponent<MeshRenderer>();
                ApplySettings(meshRenderer.materials);
            }
        }

        void Update()
        {
            var diff = oldPlanePos - plane.transform.position;
            if (diff.magnitude > 0.001) {
                if (isSkinnedMesh) {
                    ApplyPos(skinnedMeshRenderer.materials);
                }
                else {
                    ApplyPos(meshRenderer.materials);
                }

                oldPlanePos = plane.transform.position;
            }
        }

        private void ApplySettings(Material[] materials)
        {
            foreach (var mat in materials) {
                mat.SetFloat("_TimeSpeed", Random.Range(0.5f, 1.6f));
                mat.SetFloat("_RandomSwitchEdge", Random.Range(0.2f, 0.7f));
                mat.SetFloat("_BigGlitchesSpawnSpeed", Random.Range(0.1f, 1f));
                mat.SetFloat("_SmallGlitchesSpawnSpeed", Random.Range(0.7f, 2f));
                mat.SetFloat("_RandomSwitchEdge", Random.Range(0.2f, 0.7f));
                mat.SetVector(slicePlanePos, plane.transform.position);
                mat.SetVector(slicePlaneDir, plane.transform.forward);
                mat.SetFloat("_Reverse", reverse ? 1 : 0);
            }
        }

        private void ApplyPos(Material[] materials)
        {
            foreach (var mat in materials) {
                mat.SetVector(slicePlanePos, plane.transform.position);
                mat.SetVector(slicePlaneDir, plane.transform.forward);
            }
        }
    }
}