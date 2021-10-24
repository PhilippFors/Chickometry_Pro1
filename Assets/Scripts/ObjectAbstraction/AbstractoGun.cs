using System.Collections;
using Entities.Player.PlayerInput;
using ObjectAbstraction;
using UnityEngine;

public class AbstractoGun : MonoBehaviour
{
    [SerializeField] private float coolDown;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] bool canShoot = true;
    private bool FireTriggerd => PlayerInputController.Instance.LeftMouseButton.Triggered;
   
    private Transform mainCam;

    private void Start()
    {
        mainCam = Camera.main.transform;
    }

    private void Update()
    {
        if (FireTriggerd && canShoot) {
            if (Physics.Raycast(mainCam.position, mainCam.forward, out var hit, Mathf.Infinity, hitMask)) {
                var switcher = hit.transform.GetComponentInParent<ModelSwitcher>();

                if (switcher && switcher.Shootable) {
                    switcher.ToggleModels();
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