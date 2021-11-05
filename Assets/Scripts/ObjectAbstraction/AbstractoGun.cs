using System.Collections;
using Entities.Player.PlayerInput;
using ObjectAbstraction.Prototype;
using UnityEngine;

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

        private bool FireTriggered => PlayerInputController.Instance.LeftMouseButton.Triggered;
        private Transform mainCam;

        private void Start()
        {
            mainCam = Camera.main.transform;
        }

        private void Update()
        {
            if (FireTriggered && canShoot) {
                anim.Play();

                if (Physics.Raycast(mainCam.position, mainCam.forward, out var hit, Mathf.Infinity, hitMask,
                    QueryTriggerInteraction.Ignore)) {
                    var modelChanger = hit.transform.GetComponentInParent<IModelChanger>();

                    if (modelChanger != null && modelChanger.Shootable) {
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