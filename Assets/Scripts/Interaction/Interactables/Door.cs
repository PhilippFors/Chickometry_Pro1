using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction.Interactables;
using DG.Tweening;
public class Door : MonoBehaviour
{
    private Vector3 startingPos;
    private Vector3 endPos;
    public Vector3 movingVector;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        endPos = startingPos + movingVector;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
          
    }

    public void OnInteract()
    {
        rb.useGravity = false;
        transform.DOMove(endPos, 1);
     
      
    }

    public void OffInteract()
    {

        // transform.DOMove(startingPos, 1);
        rb.useGravity = true;
    }



}
