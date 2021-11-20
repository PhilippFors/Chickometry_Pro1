using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OnTrigger : MonoBehaviour
{
    private bool contested = false;

   public Door door;
   private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Gudrun" )
        {
            door.OnInteract();
           // contested = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gudrun" )
        {
            door.OffInteract();
           // contested = false;
        }
    }
}
