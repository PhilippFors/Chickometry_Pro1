using System.Collections.Generic;
using Entities.Player.PlayerInput;
using UnityEngine;

namespace ObjectAbstraction
{
    /// <summary>
    /// Throws greneades that change the abstract levels of surrounding objects.
    /// Has max amount of active grenades.
    /// </summary>
    public class AbstractoGrenadeThrower : MonoBehaviour
    {
        [SerializeField] private int maxGrenades = 2;
        [SerializeField] private float throwForce = 4;
        [SerializeField] private bool permanentChange;    
        [SerializeField] private GameObject grenadePrefab;
        [SerializeField] private float grenadeTimer = 3;
        private List<GameObject> activeGrenadeList = new List<GameObject>();
        private int activeGrenades;
        private bool GrenadeThrowTriggerd => PlayerInputController.Instance.RightMouseButton.Triggered;
        private Transform mainCam;

        private void Awake()
        {
            mainCam = Camera.main.transform;
        }

        private void Update()
        {
            if (GrenadeThrowTriggerd && activeGrenades < maxGrenades) {
                var spawnPos = new Vector3(mainCam.position.x, mainCam.position.y - 0.5f, mainCam.position.z + 0.5f);
                var obj = Instantiate(grenadePrefab, spawnPos, Quaternion.identity);
                var rb = obj.GetComponent<Rigidbody>();
                rb.AddForce(mainCam.forward * throwForce, ForceMode.Impulse);
                var rad = obj.GetComponent<AbstractoRadius>();
                rad.Init(this, permanentChange, grenadeTimer);
                activeGrenadeList.Add(obj);
                activeGrenades++;
            }
        }

        public void Remove(GameObject obj)
        {
            activeGrenadeList.Remove(obj);
            activeGrenades--;
        }
    }
}