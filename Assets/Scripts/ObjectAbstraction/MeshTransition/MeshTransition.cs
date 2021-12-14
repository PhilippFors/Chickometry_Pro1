using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.SocialPlatforms;

namespace ObjectAbstraction.MeshTransition
{
    [Serializable]
    public class VertexId
    {
        public VertexId(int id, Vector3 vertex)
        {
            Id = id;
            this.vertex = vertex;
        }

        public int Id;
        public Vector3 vertex;
    }

    [Serializable]
    public class VertexGroup
    {
        public VertexId closestHighPoly;
        public Vector3 mainLowPoly;

        public List<int> connectedVertices = new List<int>();
        public void Add(VertexId v) => connectedVertices.Add(v.Id);
        public bool IsConnected(VertexId id) => connectedVertices.Contains(id.Id);
    }

    public class MeshTransition : MonoBehaviour
    {
        [SerializeField] private bool lerpToLowPoly = true;
        [SerializeField] private Mesh highPoly;
        [SerializeField] private Mesh lowPoly;
        [SerializeField] private float transitionDuration;

        private bool doTransition;
        private float elapsedTime;
        private MeshFilter meshFilter;
        private Mesh deformationMesh;
        public VertexId[] originalVerticesHighPoly;
        public VertexId[] originalVerticesLowPoly;
        private VertexId[] displacedVertices;
        private int[] triangles;
        private int[] deformedTriangles;
        public List<VertexGroup> vertexGroups = new List<VertexGroup>();

        private void Start()
        {
            meshFilter = GetComponent<MeshFilter>();
            elapsedTime = transitionDuration;

            originalVerticesHighPoly = new VertexId[highPoly.vertices.Length];
            originalVerticesLowPoly = new VertexId[lowPoly.vertices.Length];
            triangles = new int[highPoly.triangles.Length];
            deformedTriangles = new int[highPoly.triangles.Length];
            
            for (int i = 0; i < highPoly.vertices.Length; i++) {
                originalVerticesHighPoly[i] = new VertexId(i, highPoly.vertices[i]);
                triangles[i] = highPoly.triangles[i];
            }

            for (int i = 0; i < lowPoly.vertices.Length; i++) {
                originalVerticesLowPoly[i] = new VertexId(i, lowPoly.vertices[i]);
            }

            displacedVertices = new VertexId[originalVerticesHighPoly.Length];
            for (int i = 0; i < displacedVertices.Length; i++) {
                var vert = originalVerticesHighPoly[i];
                displacedVertices[i] = new VertexId(vert.Id, vert.vertex);
            }

            deformationMesh = new Mesh();
            deformationMesh.SetVertices(GetAllVertices(originalVerticesHighPoly));
            deformationMesh.SetTriangles(triangles, 0);
            deformationMesh.SetIndices(highPoly.GetIndices(0), MeshTopology.Triangles, 0);
            deformationMesh.RecalculateBounds();
            deformationMesh.RecalculateNormals();
            deformationMesh.RecalculateTangents();
            meshFilter.mesh = deformationMesh;
        }

        void Update()
        {
            if (!doTransition) {
                return;
            }

            if (lerpToLowPoly) {
                if (elapsedTime < transitionDuration) {
                    for (int i = 0; i < vertexGroups.Count; i++) {
                        var index = vertexGroups[i].closestHighPoly.Id;

                        displacedVertices[index].vertex = Vector3.Lerp(displacedVertices[index].vertex,
                            vertexGroups[i].mainLowPoly, elapsedTime / transitionDuration);

                        // foreach (var par in vertexGroups[i].connectedVertices) {
                        //     displacedVertices[par].vertex = Vector3.Lerp(displacedVertices[par].vertex,
                        //         Vector3.zero, elapsedTime / transitionDuration);
                        // }
                    }

                    deformationMesh.vertices = GetAllVertices(displacedVertices);
                    deformationMesh.RecalculateNormals();
                    deformationMesh.RecalculateBounds();
                    elapsedTime += Time.deltaTime;
                }
                else {
                    doTransition = false;
                    lerpToLowPoly = false;
                }
            }
            else {
                if (elapsedTime < transitionDuration) {
                    for (int i = 0; i < displacedVertices.Length; i++) {
                        displacedVertices[i].vertex = Vector3.Lerp(displacedVertices[i].vertex,
                            originalVerticesHighPoly[i].vertex, elapsedTime / transitionDuration);
                    }

                    deformationMesh.vertices = GetAllVertices(displacedVertices);
                    deformationMesh.RecalculateNormals();
                    deformationMesh.RecalculateBounds();
                    elapsedTime += Time.deltaTime;
                }
                else {
                    doTransition = false;
                    lerpToLowPoly = true;
                }
            }
        }

        [Button]
        public void StartTransition()
        {
            elapsedTime = 0;
            doTransition = true;
        }

        [Button]
        private void ComputeClosestVertices()
        {
            vertexGroups = new List<VertexGroup>();

            var tempHighPoly = new List<VertexId>();
            for (int i = 0; i < originalVerticesHighPoly.Length; i++) {
                tempHighPoly.Add(new VertexId(originalVerticesHighPoly[i].Id, originalVerticesHighPoly[i].vertex));
            }
            
            for (int i = 0; i < originalVerticesLowPoly.Length; i++) {
                var vertGroup = new VertexGroup();
                var closestDistance = float.PositiveInfinity;
                VertexId closestVertex = null;
                var lowPolyId = originalVerticesLowPoly[i];
                vertGroup.mainLowPoly = lowPolyId.vertex;

                int closestIndex = 0;
                for (int j = 0; j < tempHighPoly.Count; j++) {
                    var highPolyId = tempHighPoly[j];
                    var dist = Vector3.Distance(LocalToWorld(highPolyId.vertex),LocalToWorld(lowPolyId.vertex));
                    if (dist < closestDistance) {
                        closestVertex = new VertexId(highPolyId.Id, highPolyId.vertex);
                        closestIndex = j;
                    }
                }

                tempHighPoly.RemoveAt(closestIndex);
                vertGroup.closestHighPoly = closestVertex;
                vertexGroups.Add(vertGroup);
            }

            tempHighPoly = new List<VertexId>();
            for (int i = 0; i < originalVerticesHighPoly.Length; i++) {
                tempHighPoly.Add(new VertexId(originalVerticesHighPoly[i].Id, originalVerticesHighPoly[i].vertex));
            }

            var maxAmount = Mathf.RoundToInt(tempHighPoly.Count / vertexGroups.Count);
            for (int i = 0; i < vertexGroups.Count; i++) {
                var highPolyId = vertexGroups[i].closestHighPoly;

                for (int z = 0; z < maxAmount; z++) {
                    var closestDistance = float.PositiveInfinity;
                    VertexId closestVertex = null;
                    int closestIndex = 0;
                    for (int j = 0; j < tempHighPoly.Count; j++) {
                        var otherHighPolyId = tempHighPoly[j];
                        if (otherHighPolyId.Id != highPolyId.Id) {
                            var dist = Vector3.Distance(LocalToWorld(highPolyId.vertex), LocalToWorld(otherHighPolyId.vertex));
                            if (dist < closestDistance) {
                                closestVertex = new VertexId(otherHighPolyId.Id, otherHighPolyId.vertex);
                                closestIndex = j;
                            }
                        }
                    }

                    tempHighPoly.RemoveAt(closestIndex);
                    vertexGroups[i].Add(closestVertex);
                }
            }

            if (tempHighPoly.Count > 0) {
                int t = 1;
                foreach (var v in tempHighPoly) {
                    vertexGroups[vertexGroups.Count - t].Add(new VertexId(v.Id, v.vertex));
                    t++;
                }
            }
        }

        private Vector3 LocalToWorld(Vector3 v)
        {
            return transform.localToWorldMatrix * v;
        }
        private Vector3[] GetAllVertices(VertexId[] vertices)
        {
            var verts = new Vector3[vertices.Length];
            for (int i = 0; i < verts.Length; i++) {
                verts[i] = vertices[i].vertex;
            }

            return verts;
        }
    }
}