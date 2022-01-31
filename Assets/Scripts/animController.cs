using System.Collections;
using System.Collections.Generic;
using Entities.Player.PlayerInput;
using UnityEngine;

public class animController : MonoBehaviour
{
    private bool pressed => InputController.Instance.Triggered(InputPatterns.Interact);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pressed) {
            GetComponent<Animation>().Play();
        }   
    }
}
