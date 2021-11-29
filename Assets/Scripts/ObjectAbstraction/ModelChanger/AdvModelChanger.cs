using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.PlayerInput;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ObjectAbstraction.ModelChanger
{
    /// <summary>
    /// Advance version of the model changer with more settings and functionality
    /// </summary>
    [RequireComponent(typeof(MeshCollider))]
    public class AdvModelChanger : MonoBehaviour, IModelChanger
    {
        public bool Shootable {
            get => shootable;
            set => shootable = value;
        }
        public bool IsAbstract => abstractLayer != 0;
        public bool SimpleToggle => simpleToggle;
        public List<ModelSettings> Models => models;
        
        [SerializeField] private bool simpleToggle = true;
        [SerializeField, Min(0)] private int abstractLayer; //current abstraction layer
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float threshhold = 0.15f;
        [SerializeField, ReadOnly] private float threshholdAbsolute;
        [SerializeField] private MeshFilter currentMeshFilter;
        [SerializeField] private MeshFilter nextMeshFilter;
        [SerializeField] private List<ModelSettings> models;
        [SerializeField] private Material usedMaterial;

        private Material currentInstanceMat;
        private Material nextInstanceMat;
        private MeshCollider meshCollider;
        private float MouseDeltaX => InputController.Instance.GetValue<Vector2>(InputPatterns.MouseDelta).x;
        private float oldAbsolute;
        private float[] abstractionSections;
        private bool goesUp;
        private bool toNext;
        private bool isInThreshhold;
        private bool shootable;
        private int ScreenWidth => Screen.width;

        private void Awake()
        {
            meshCollider = GetComponent<MeshCollider>();
            var currentMeshRenderer = currentMeshFilter.GetComponent<MeshRenderer>();
            var nextMeshRenderer = nextMeshFilter.GetComponent<MeshRenderer>();

            currentInstanceMat = new Material(usedMaterial);
            nextInstanceMat = new Material(usedMaterial);
            currentMeshRenderer.sharedMaterial = currentInstanceMat;
            nextMeshRenderer.sharedMaterial = nextInstanceMat;
            nextInstanceMat.SetFloat("_CutoffValue", 1);
        }

        private void Start()
        {
            abstractionSections = new float[models.Count];

            models[abstractLayer].ApplyMesh(currentMeshFilter);
            models[abstractLayer].ApplyTexture(currentMeshFilter.GetComponent<MeshRenderer>());
            models[abstractLayer].ApplyMeshCollider(meshCollider);
            models[abstractLayer].ApplyRigidbodySettings(GetComponent<Rigidbody>());

            if (simpleToggle) {
                if (abstractLayer + 1 == models.Count) {
                    models[0].ApplyMesh(nextMeshFilter);
                    models[0].ApplyTexture(nextMeshFilter.GetComponent<MeshRenderer>());
                }
                else {
                    models[abstractLayer + 1].ApplyMesh(nextMeshFilter);
                    models[abstractLayer + 1].ApplyTexture(nextMeshFilter.GetComponent<MeshRenderer>());
                }
            }
            else {
                if (abstractLayer == 0) {
                    models[abstractLayer + 1].ApplyMesh(nextMeshFilter);
                }

                if (abstractLayer == models.Count - 1) {
                    models[abstractLayer - 1].ApplyMesh(nextMeshFilter);
                }

                for (int i = 0; i < abstractLayer + 1; i++) {
                    abstractionSections[i] = 1;
                }
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

                    if (abstractionSections[abstractLayer] <= 1 && abstractionSections[abstractLayer] >= 0) {
                        Interp();
                    }

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

                currentInstanceMat.SetFloat("_CutoffValue", 0);
                nextInstanceMat.SetFloat("_CutoffValue", 1);

                models[layer].ApplyMeshCollider(meshCollider);
                models[layer].ApplyRigidbodySettings(GetComponent<Rigidbody>());
                models[layer].ApplyTexture(currentMeshFilter.GetComponent<MeshRenderer>());

                if (layer + 1 == models.Count) {
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
                        models[layer].ApplyMesh(nextMeshFilter);
                    }

                    currentMeshFilter.sharedMesh = nextMeshFilter.sharedMesh;

                    currentInstanceMat.SetFloat("_CutoffValue", 0);
                    nextInstanceMat.SetFloat("_CutoffValue", 1);

                    models[layer].ApplyMeshCollider(meshCollider);
                    models[layer].ApplyRigidbodySettings(GetComponent<Rigidbody>());
                    models[layer].ApplyTexture(currentMeshFilter.GetComponent<MeshRenderer>());
                }
            }
        }

        private IEnumerator Transition()
        {
            currentInstanceMat.DOFloat(1, "_CutoffValue", 0.5f);
            nextInstanceMat.DOFloat(0, "_CutoffValue", 0.5f);
            yield return new WaitForSeconds(0.5f);
        }

        private void Interp()
        {
            float current;
            float next;

            if (!goesUp) {
                current = 1 - abstractionSections[abstractLayer];
                next = abstractionSections[abstractLayer];
            }
            else {
                current = abstractionSections[abstractLayer];
                next = 1 - abstractionSections[abstractLayer];
            }

            currentInstanceMat.SetFloat("_CutoffValue", current);
            nextInstanceMat.SetFloat("_CutoffValue", next);
        }

        private void OnValidate()
        {
            var currentRend = currentMeshFilter.GetComponent<MeshRenderer>();
            if (!currentRend.sharedMaterial) {
                currentRend.sharedMaterial = new Material(usedMaterial);
            }

            if (abstractLayer < models.Count) {
                meshCollider = GetComponent<MeshCollider>();
                models[abstractLayer].ApplyMesh(currentMeshFilter);
                models[abstractLayer].ApplyTexture(currentMeshFilter.GetComponent<MeshRenderer>());
                models[abstractLayer].ApplyMeshCollider(meshCollider);
                models[abstractLayer].ApplyRigidbodySettings(GetComponent<Rigidbody>());
            }
        }
    }
}