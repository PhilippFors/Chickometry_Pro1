using System.Collections;
using Entities.Player;
using Entities.Player.PlayerInput;
using ObjectAbstraction.ModelChanger;
using UnityEngine;
using Utlities;

namespace ObjectAbstraction
{
    /// <summary>
    /// Shooting at a viable object toggles its abstraction level.
    /// </summary>
    public class AbstractoGun : MonoBehaviour
    {
        [SerializeField] private float coolDown;
        [SerializeField] private LayerMask hitMask;
        [SerializeField] bool canShoot = true;
        [SerializeField] private Animation anim;

        private bool FireTriggered => InputController.Instance.Triggered(InputPatterns.LeftClick);
        private bool FirePressed => InputController.Instance.IsPressed(InputPatterns.LeftClick);
        private Transform mainCam;
        private SmoothMouseLook mouseLook;

        private void Start()
        {
            mouseLook = ServiceLocator.Get<SmoothMouseLook>();
            mainCam = Camera.main.transform;
            InputController.Instance.Get(InputPatterns.RightClick).Canceled += ctx => mouseLook.ResetTargeDirection();
        }

        private void Update()
        {
            Fire();
        }

        private void Fire()
        {
            if (FireTriggered || FirePressed) {
                if (Physics.Raycast(mainCam.position, mainCam.forward, out var hit, Mathf.Infinity, hitMask,
                    QueryTriggerInteraction.Ignore)) {
                    var modelChanger = hit.transform.GetComponentInParent<IModelChanger>();
                    
                    if (modelChanger != null) {
                        if (canShoot) {
                            anim.Play();
                            Shoot(modelChanger, hit.transform.GetComponent<Rigidbody>());
                        }
                    }
                }
            }
        }

        private void Shoot(IModelChanger modelChanger, Rigidbody rb)
        {
            modelChanger.ToggleModels();
            if (rb) {
                rb.velocity = Vector3.zero;
            }

            canShoot = false;
            StartCoroutine(CoolDown());
        }

        private IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(coolDown);
            canShoot = true;
        }
    }
}