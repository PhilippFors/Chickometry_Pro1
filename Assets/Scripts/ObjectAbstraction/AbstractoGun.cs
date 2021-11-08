using System.Collections;
using Entities.Player;
using Entities.Player.PlayerInput;
using ObjectAbstraction.New;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Shooting at a viable object toggles its abstraction level.
    /// </summary>
    public class AbstractoGun : MonoBehaviour
    {
        [SerializeField] private SmoothMouseLook mouseLook;
        [SerializeField] private float coolDown;
        [SerializeField] private LayerMask hitMask;
        [SerializeField] bool canShoot = true;
        [SerializeField] private Animation anim;

        private bool FireTriggered => PlayerInputController.Instance.LeftMouseButton.Triggered;
        private bool AltFirePressed => PlayerInputController.Instance.RightMouseButton.IsPressed;

        private Transform mainCam;
        private AdvModelChanger altFireCache;

        private void Start()
        {
            mainCam = Camera.main.transform;
            PlayerInputController.Instance.RightMouseButton.Canceled += ctx => mouseLook.ResetTargeDirection();
        }

        private void Update()
        {
            AltFire();

            Fire();
        }

        private void AltFire()
        {
            if (AltFirePressed) {
                if (Physics.Raycast(mainCam.position, mainCam.forward, out var hit, Mathf.Infinity, hitMask,
                    QueryTriggerInteraction.Ignore)) {
                    var modelChanger = hit.transform.GetComponentInParent<AdvModelChanger>();
                    if (modelChanger) {
                        if (!altFireCache) {
                            altFireCache = modelChanger;
                        }
                    }
                }

                if (altFireCache) {
                    mouseLook.enableLook = false;

                    // TODO: Better lock on, probably needs a mouselook refactor
                    mouseLook.transform.LookAt(altFireCache.transform);
                    var temp = new Vector3(altFireCache.transform.position.x,
                        mouseLook.CharacterBody.position.y,
                        altFireCache.transform.position.z);
                    mouseLook.CharacterBody.LookAt(temp);

                    altFireCache.ToggleModels();
                }
            }
            else {
                altFireCache = null;
                mouseLook.enableLook = true;
            }
        }

        private void Fire()
        {
            if (FireTriggered && canShoot && !AltFirePressed) {
                anim.Play();

                if (Physics.Raycast(mainCam.position, mainCam.forward, out var hit, Mathf.Infinity, hitMask,
                    QueryTriggerInteraction.Ignore)) {
                    var modelChanger = hit.transform.GetComponentInParent<IModelChanger>();

                    if (modelChanger != null && !(modelChanger is AdvModelChanger) && modelChanger.Shootable) {
                        modelChanger.ToggleModels();
                        var rb = hit.transform.GetComponentInParent<Rigidbody>();
                        if (rb) {
                            rb.velocity = Vector3.zero;
                        }
                    }

                    canShoot = false;
                    StartCoroutine(CoolDown());
                }
            }
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(coolDown);
            canShoot = true;
        }
    }
}