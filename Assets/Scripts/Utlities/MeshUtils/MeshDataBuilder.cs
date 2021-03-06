// Based on: https://catlikecoding.com/unity/tutorials/advanced-rendering/flat-and-wireframe-shading/

using System.Collections.Generic;
using ObjectAbstraction.ModelChanger;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Utlities.MeshUtils
{
    /// <summary>
    /// Any vertices are doubled so each triangle has it's own vertex.
    /// Generate Barycentric coordinates that can be used by a shader to generate a wireframe look.
    /// </summary>
    [DefaultExecutionOrder(-51)]
    public class MeshDataBuilder : MonoBehaviour
    {
        private static HashSet<Mesh> meshes = new HashSet<Mesh>();
        private AdvModelChanger modelChanger;
        private void Awake()
        {
            GenerateMeshData();
        }
        
        /// <summary>
        /// We will assign a color to each Vertex in a Triangle on the object's mesh
        /// </summary>
        public void GenerateMeshData()
        {
            modelChanger = GetComponentInParent<AdvModelChanger>();
            foreach (var model in modelChanger.Models) {
                if (model && !meshes.Contains(model)) {
                    meshes.Add(model);
                    SplitMesh(model);
                    SetVertexColors(model);
                }
            }
        }

        /// <summary>
        /// For this approach, we need to make sure there are not shared vertices
        /// on the mesh, that's why we use this method to split the mesh. 
        /// This will increase the number of vertices, so less optimized.
        /// </summary>
        /// <param name="mesh"></param>
        void SplitMesh(Mesh mesh)
        {
            int[] triangles = mesh.triangles;
            Vector3[] verts = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uvs = mesh.uv;

            Vector3[] newVerts;
            Vector3[] newNormals;
            Vector2[] newUvs;

            int n = triangles.Length;
            newVerts = new Vector3[n];
            newNormals = new Vector3[n];
            newUvs = new Vector2[n];

            for (int i = 0; i < n; i++) {
                newVerts[i] = verts[triangles[i]];
                newNormals[i] = normals[triangles[i]];
                if (uvs.Length > 0) {
                    newUvs[i] = uvs[triangles[i]];
                }

                triangles[i] = i;
            }

            mesh.vertices = newVerts;
            mesh.normals = newNormals;
            mesh.uv = newUvs;
            mesh.triangles = triangles;
        }

        /// <summary>
        /// We paint the vertex color
        /// </summary>
        /// <param name="mesh"></param>
        void SetVertexColors(Mesh mesh)
        {
            Color[] colorCoords = new[] {
                new Color(1, 0, 0),
                new Color(0, 1, 0),
                new Color(0, 0, 1),
            };

            Color32[] vertexColors = new Color32[mesh.vertices.Length];

            for (int i = 0; i < vertexColors.Length; i += 3) {
                vertexColors[i] = colorCoords[0];
                vertexColors[i + 1] = colorCoords[1];
                vertexColors[i + 2] = colorCoords[2];
            }

            mesh.colors32 = vertexColors;
        }
    }
}