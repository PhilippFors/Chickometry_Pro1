using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.PlayerInput;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.New
{
    /// <summary>
    /// Advance version of the model changer with more settings and functionality
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class AdvModelChanger : MonoBehaviour, IModelChanger
    {
        public bool Shootable => true;
        public bool IsAbstract => abstractLayer != 0;
        public bool SimpleToggle => simpleToggle;

        [SerializeField] private bool simpleToggle = true;
        [SerializeField, Min(0)] private int abstractLayer; //current abstraction layer
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float threshhold = 0.15f;
        [SerializeField, ReadOnly] private float threshholdAbsolute;
        [SerializeField] private MeshFilter currentMeshFilter;
        [SerializeField] private MeshFilter nextMeshFilter;

        [SerializeField] private List<ModelSettings> models;
        
        [SerializeField] private Material dissolveShader;
        private Material currentMat;
        private Material nextMat;
        private float MouseDeltaX => PlayerInputController.Instance.MouseDelta.ReadValue().x;
        private bool isInThreshhold;
        private int ScreenWidth => Screen.width;
        private MeshCollider meshCollider;
        private float[] abstractionSections;
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
            abstractionSections = new float[models.Count];
            currentMeshFilter.sharedMesh = models[abstractLayer].mesh;
            if (abstractLayer == 0) {
                nextMeshFilter.sharedMesh = models[abstractLayer + 1].mesh;
            }

            if (abstractLayer == models.Count - 1) {
                nextMeshFilter.sharedMesh = models[abstractLayer - 1].mesh;
            }

            for (int i = 0; i < abstractLayer + 1; i++) {
                abstractionSections[i] = 1;
            }
        }

        // Called in Update by the abstracto gun
        public void ToggleModels()
        {
            if (!simpleToggle) {
                if (!isInThreshhold) {
                    oldAbsolute = abstractionSections[abstractLayer];
                    abstractionSections[abstractLayer] += (MouseDeltaX / ScreenWidth) * sensitivity;
                    abstractionSections[abstractLayer] = Mathf.Clamp(abstractionSections[abstractLayer], -0.05f, 1.05f);
                    
                    if (oldAbsolute < abstractionSections[abstractLayer]) {
                        toNext = goesUp;
                    }
                    else {
                        toNext = !goesUp;
                    }

                    isInThreshhold = abstractionSections[abstractLayer] >= 1 || abstractionSections[abstractLayer] <= 0;

                    if (abstractionSections[abstractLayer] >= 1 && abstractLayer + 1 < abstractionSections.Length) {
                        abstractionSections[abstractLayer] = 0.99f;
                        if (toNext) {
                            abstractLayer++;
                        }

                        EnableLayer(abstractLayer);
                    }

                    if (abstractionSections[abstractLayer] <= 0 && abstractLayer > 0) {
                        abstractionSections[abstractLayer] = 0.01f;
                        if (toNext) {
                            abstractLayer--;
                        }

                        EnableLayer(abstractLayer);
                    }
                    
                    Interp();
                }

                if (isInThreshhold) {
                    oldAbsolute = threshholdAbsolute;
                    threshholdAbsolute += (MouseDeltaX / ScreenWidth) * sensitivity;
                    threshholdAbsolute = Mathf.Clamp(threshholdAbsolute, threshhold * -1, threshhold);

                    if (threshholdAbsolute <= threshhold * -1 || threshholdAbsolute >= threshhold) {
                        if (threshholdAbsolute < 0) {
                            if (abstractLayer > 0) {
                                models[abstractLayer - 1].ApplyMesh(nextMeshFilter);
                                models[abstractLayer - 1].ApplyTexture(nextMeshFilter.GetComponent<MeshRenderer>());
                            }

                            goesUp = false;
                        }

                        if (threshholdAbsolute > 0) {
                            if (abstractLayer + 1 < models.Count) {
                                models[abstractLayer + 1].ApplyMesh(nextMeshFilter);
                                models[abstractLayer + 1].ApplyTexture(nextMeshFilter.GetComponent<MeshRenderer>());
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
                if (abstractLayer == models.Count) {
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
                
                models[layer].ApplyMeshCollider(meshCollider);
                models[layer].ApplyRigidbodySettings(GetComponent<Rigidbody>());
                models[layer].ApplyTexture(currentMeshFilter.GetComponent<MeshRenderer>());
                
                if (layer + 1 >= models.Count) {
                    models[0].ApplyMesh(nextMeshFilter);
                    models[0].ApplyTexture(nextMeshFilter.GetComponent<MeshRenderer>());
                }
                else {
                    models[layer + 1].ApplyMesh(nextMeshFilter);
                    models[layer + 1].ApplyTexture(nextMeshFilter.GetComponent<MeshRenderer>());
                }
            }
            else {
                if (nextMeshFilter.sharedMesh) {
                    if (!toNext) {
                        nextMeshFilter.sharedMesh = models[layer].mesh;
                    }

                    currentMeshFilter.sharedMesh = nextMeshFilter.sharedMesh;
                               
                    currentMat.SetFloat("_CutoffValue", 0);
                    nextMat.SetFloat("_CutoffValue", 1);

                    if (models[layer].hasSeperateColliderMesh) {
                        meshCollider.sharedMesh = models[layer].colliderMesh;
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

        private void Interp()
        {
            var current = 0f;
            var next = 0f;
            if (!goesUp) {
                current = Mathf.InverseLerp(1, 0,abstractionSections[abstractLayer]);
                next = abstractionSections[abstractLayer];
                currentMat.SetFloat("_CutoffValue", current);
                nextMat.SetFloat("_CutoffValue", next);
            }
            else {
                current = abstractionSections[abstractLayer];
                next = Mathf.InverseLerp(1, 0, abstractionSections[abstractLayer]);
            }

            currentMat.SetFloat("_CutoffValue", current);
            nextMat.SetFloat("_CutoffValue", next);
        }

        private void OnValidate()
        {
            if (abstractLayer < models.Count) {
                meshCollider = GetComponent<MeshCollider>();
                models[abstractLayer].ApplyMesh(currentMeshFilter);
                models[abstractLayer].ApplyMeshCollider(meshCollider);
                models[abstractLayer].ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
        }
    }
}