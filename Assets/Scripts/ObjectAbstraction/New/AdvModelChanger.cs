using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.PlayerInput;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.New
{
    [RequireComponent(typeof(MeshCollider))]
    public class AdvModelChanger : MonoBehaviour, IModelChanger
    {
        public bool Shootable => true;
        public bool IsAbstract => abstractLayer != 0;
        public bool SimpleToggle => simpleToggle;

        [SerializeField] private bool simpleToggle;
        [SerializeField] private bool hasSperarateMeshCol;
        [SerializeField, Min(0)] private int abstractLayer; //current abstraction layer
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float threshhold;
        [SerializeField, ReadOnly] private float absoluteMouseDelta;
        [SerializeField, ReadOnly] private float threshholdAbsolute;
        [SerializeField] private MeshFilter currentMeshFilter;
        [SerializeField] private MeshFilter nextMeshFilter;

        [SerializeField] private List<Mesh> meshes;

        [SerializeField, EnableIf("hasSperarateMeshCol")]
        private List<Mesh> colMeshes;

        [SerializeField] private Material dissolveShader;
        private Material currentMat;
        private Material nextMat;
        private float MouseDeltaX => PlayerInputController.Instance.MouseDelta.ReadValue().x;
        private bool isInThreshhold;
        private int ScreenWidth => Screen.width;
        private MeshCollider meshCollider;
        public float[] abstractionSections;
        private float oldAbsolute;
        private bool goesUp;
        private bool toNext;

        private void Awake()
        {
            meshCollider = GetComponent<MeshCollider>();
            
            var currentMeshRenderer = currentMeshFilter.GetComponent<MeshRenderer>();
            var nextMeshRenderer = nextMeshFilter.GetComponent<MeshRenderer>();
            currentMat = new Material(dissolveShader);
            nextMat = new Material(dissolveShader);
            currentMeshRenderer.sharedMaterial = currentMat;
            nextMeshRenderer.sharedMaterial = nextMat;
            nextMat.SetFloat("_CutoffValue", 1);
        }

        private void Start()
        {
            abstractionSections = new float[meshes.Count];
            currentMeshFilter.sharedMesh = meshes[abstractLayer];
            if (abstractLayer == 0) {
                nextMeshFilter.sharedMesh = meshes[abstractLayer + 1];
            }

            if (abstractLayer == meshes.Count - 1) {
                nextMeshFilter.sharedMesh = meshes[abstractLayer - 1];
            }
        }

        // Called every frame by the abstracto gun
        public void ToggleModels()
        {
            if (!simpleToggle) {
                if (!isInThreshhold) {
                    oldAbsolute = abstractionSections[abstractLayer];
                    abstractionSections[abstractLayer] += (MouseDeltaX / ScreenWidth) * sensitivity;
                    abstractionSections[abstractLayer] = Mathf.Clamp(abstractionSections[abstractLayer], -0.05f, 1.05f);

                    if (oldAbsolute < abstractionSections[abstractLayer]) {
                        if (goesUp) {
                            toNext = true;
                        }
                        else {
                            toNext = false;
                        }
                    }
                    else {
                        if (goesUp) {
                            toNext = false;
                        }
                        else {
                            toNext = true;
                        }
                    }

                    isInThreshhold = abstractionSections[abstractLayer] >= 1 || abstractionSections[abstractLayer] <= 0;

                    if (abstractionSections[abstractLayer] >= 1 && abstractLayer + 1 < abstractionSections.Length) {
                        abstractionSections[abstractLayer] = 0.99f;
                        abstractLayer++;
                        EnableLayer(abstractLayer);
                    }

                    if (abstractionSections[abstractLayer] <= 0 && abstractLayer > 0) {
                        abstractionSections[abstractLayer] = 0.01f;
                        abstractLayer--;
                        EnableLayer(abstractLayer);
                    }
                }

                if (isInThreshhold) {
                    oldAbsolute = threshholdAbsolute;
                    threshholdAbsolute += (MouseDeltaX / ScreenWidth) * sensitivity;
                    threshholdAbsolute = Mathf.Clamp(threshholdAbsolute, threshhold * -1, threshhold);

                    if (threshholdAbsolute <= threshhold * -1 || threshholdAbsolute >= threshhold) {
                        if (threshholdAbsolute < 0) {
                            if (abstractLayer > 0) {
                                nextMeshFilter.sharedMesh = meshes[abstractLayer - 1];
                            }

                            goesUp = false;
                        }

                        if (threshholdAbsolute > 0) {
                            if (abstractLayer + 1 < meshes.Count) {
                                nextMeshFilter.sharedMesh = meshes[abstractLayer + 1];
                            }

                            goesUp = true;
                        }

                        isInThreshhold = false;
                        threshholdAbsolute = 0;
                    }
                }
            }
            else {
                abstractLayer++;
                if (abstractLayer == meshes.Count) {
                    abstractLayer = 0;
                }

                StartCoroutine(EnableLayer(abstractLayer));
            }
        }

        private IEnumerator EnableLayer(int layer)
        {
            if (simpleToggle) {
                yield return StartCoroutine(Transition());
                
                currentMeshFilter.sharedMesh = nextMeshFilter.sharedMesh;
                                
                currentMat.SetFloat("_CutoffValue", 0);
                nextMat.SetFloat("_CutoffValue", 1);
                
                if (hasSperarateMeshCol) {
                    meshCollider.sharedMesh = meshes[layer];
                }

                if (layer + 1 >= meshes.Count) {
                    nextMeshFilter.sharedMesh = meshes[0];
                }
                else {
                    nextMeshFilter.sharedMesh = meshes[layer + 1];
                }

            }
            else {
                if (nextMeshFilter.sharedMesh) {
                    if (!toNext) {
                        nextMeshFilter.sharedMesh = meshes[layer];
                    }

                    currentMeshFilter.sharedMesh = nextMeshFilter.sharedMesh;

                    if (hasSperarateMeshCol) {
                        meshCollider.sharedMesh = colMeshes[layer];
                    }
                    else {
                        meshCollider.sharedMesh = nextMeshFilter.sharedMesh;
                    }
                }
            }
        }

        private IEnumerator Transition()
        {
            currentMat.DOFloat(1, "_CutoffValue", 0.5f);
            nextMat.DOFloat(0, "_CutoffValue", 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        private void OnValidate()
        {
            if (abstractLayer < meshes.Count) {
                meshCollider = GetComponent<MeshCollider>();
                currentMeshFilter.sharedMesh = meshes[abstractLayer];
                if (hasSperarateMeshCol) {
                    meshCollider.sharedMesh = colMeshes[abstractLayer];
                }
                else {
                    meshCollider.sharedMesh = meshes[abstractLayer];
                }
            }
        }
    }
}