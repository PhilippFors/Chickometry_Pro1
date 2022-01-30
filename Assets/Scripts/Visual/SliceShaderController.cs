using UnityEngine;

namespace Visual
{
    /// <summary>
    /// Controls the slice plane parameters on the mesh it is assigned to
    /// </summary>
    public class SliceShaderController : MonoBehaviour
    {
        public bool reverse;
        public bool isTransitioning;
        public GameObject plane;
        private MeshRenderer meshRenderer;
        // public BoxCollider volume;
        private int slicePlanePosID;
        private int slicePlaneDirID;
        
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            foreach (var mat in meshRenderer.materials) {
                mat.SetFloat("_TimeSpeed", Random.Range(0.5f, 1.6f));
                mat.SetFloat("_RandomSwitchEdge", Random.Range(0.2f, 0.7f));
                mat.SetVector("_SlicePlanePos", plane.transform.position);
                mat.SetVector("_SlicePlaneDir", plane.transform.forward);
                mat.SetFloat("_Reverse", reverse ? 1 : 0);
            }
        }

        void Update()
        {
            if (!isTransitioning) {
                return;
            }
            
            foreach (var mat in meshRenderer.materials) {
                mat.SetVector("_SlicePlanePos", plane.transform.position);
                mat.SetVector("_SlicePlaneDir", plane.transform.forward);
            }
        }

        private void GetMinMax()
        {
            // var pos = volume.transform.position;
            // var size = volume.size;
            // var scale = volume.transform.localScale;
            // var rot = volume.transform.rotation;
            // var s = Vector3.Scale(size, scale);
            // var verts = new[] {
            //     rot * (new Vector3(pos.x + s.x / 2, pos.y + s.y / 2, pos.z - s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x + s.x / 2, pos.y + s.y / 2, pos.z + s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x - s.x / 2, pos.y + s.y / 2, pos.z + s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x - s.x / 2, pos.y + s.y / 2, pos.z - s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x + s.x / 2, pos.y - s.y / 2, pos.z - s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x + s.x / 2, pos.y - s.y / 2, pos.z + s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x - s.x / 2, pos.y - s.y / 2, pos.z + s.z / 2) - pos) + pos,
            //     rot * (new Vector3(pos.x - s.x / 2, pos.y - s.y / 2, pos.z - s.z / 2) - pos) + pos,
            // };
            // var minPoint = new Vector3(FindMinX(verts), FindMinY(verts), FindMinZ(verts));
            // var maxPoint = new Vector3(FindMaxX(verts), FindMaxY(verts), FindMaxZ(verts));
            // meshRenderer.material.SetVector("_MinPoint", minPoint);
            // meshRenderer.material.SetVector("_MaxPoint", maxPoint);
        }

        private float FindMinX(Vector3[] verts)
        {
            float min = float.PositiveInfinity;
            for (int i = 0; i < verts.Length; i++) {
                if (verts[i].x < min) {
                    min = verts[i].x;
                }
            }

            return min;
        }

        private float FindMaxX(Vector3[] verts)
        {
            float max = float.NegativeInfinity;
            for (int i = 0; i < verts.Length; i++) {
                if (verts[i].x > max) {
                    max = verts[i].x;
                }
            }

            return max;
        }

        private float FindMinY(Vector3[] verts)
        {
            float min = float.PositiveInfinity;
            for (int i = 0; i < verts.Length; i++) {
                if (verts[i].y < min) {
                    min = verts[i].y;
                }
            }

            return min;
        }

        private float FindMaxY(Vector3[] verts)
        {
            float max = float.NegativeInfinity;
            for (int i = 0; i < verts.Length; i++) {
                if (verts[i].y > max) {
                    max = verts[i].y;
                }
            }

            return max;
        }

        private float FindMinZ(Vector3[] verts)
        {
            float min = float.PositiveInfinity;
            for (int i = 0; i < verts.Length; i++) {
                if (verts[i].z < min) {
                    min = verts[i].z;
                }
            }

            return min;
        }

        private float FindMaxZ(Vector3[] verts)
        {
            float max = float.NegativeInfinity;
            for (int i = 0; i < verts.Length; i++) {
                if (verts[i].z > max) {
                    max = verts[i].z;
                }
            }

            return max;
        }
    }
}