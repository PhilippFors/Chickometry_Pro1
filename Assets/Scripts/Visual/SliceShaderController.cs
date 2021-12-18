using UnityEngine;

namespace Visual
{
    public class SliceShaderController : MonoBehaviour
    {
        public bool reverse;
        public GameObject plane;
        public MeshRenderer meshRenderer;
        public BoxCollider volume;

        // Update is called once per frame
        void Update()
        {
            GetMinMax();
            meshRenderer.material.SetVector("_PlanePos", plane.transform.position);
            meshRenderer.material.SetVector("_PlaneDir", plane.transform.forward);
            meshRenderer.material.SetFloat("_Reverse", reverse ? 1 : 0);
        }

        private void GetMinMax()
        {
            var pos = volume.transform.position;
            var size = volume.size;
            var scale = volume.transform.localScale;
            var rot = volume.transform.rotation;
            var s = Vector3.Scale(size, scale);
            var verts = new[] {
                rot * (new Vector3(pos.x + s.x / 2, pos.y + s.y / 2, pos.z - s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x + s.x / 2, pos.y + s.y / 2, pos.z + s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x - s.x / 2, pos.y + s.y / 2, pos.z + s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x - s.x / 2, pos.y + s.y / 2, pos.z - s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x + s.x / 2, pos.y - s.y / 2, pos.z - s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x + s.x / 2, pos.y - s.y / 2, pos.z + s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x - s.x / 2, pos.y - s.y / 2, pos.z + s.z / 2) - pos) + pos,
                rot * (new Vector3(pos.x - s.x / 2, pos.y - s.y / 2, pos.z - s.z / 2) - pos) + pos,
            };
            var minPoint = new Vector3(FindMinX(verts), FindMinY(verts), FindMinZ(verts));
            var maxPoint = new Vector3(FindMaxX(verts), FindMaxY(verts), FindMaxZ(verts));
            meshRenderer.material.SetVector("_MinPoint", minPoint);
            meshRenderer.material.SetVector("_MaxPoint", maxPoint);
            
            // var right = rot * (new Vector3(pos.x + s.x / 2, pos.y, pos.z) - pos) + pos;
            // var up = rot * (new Vector3(pos.x, pos.y + s.y / 2, pos.z) - pos) + pos;
            // var forward = rot * (new Vector3(pos.x, pos.y, pos.z + s.z / 2) - pos) + pos;
            //
            // Matrix4x4 m = new Matrix4x4();
            // m.SetRow(0, volume.transform.forward);
            // m.SetRow(1, volume.transform.up);
            // m.SetRow(2, volume.transform.right);
            //
            // meshRenderer.material.SetVector("_Position", pos);
            // meshRenderer.material.SetVector("_Size", s);
            // meshRenderer.material.SetMatrix("_Directions", m);
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