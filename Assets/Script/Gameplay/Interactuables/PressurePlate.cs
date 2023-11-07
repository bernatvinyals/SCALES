using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactuble
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.invokeInteractedOnce(); 
        }
    }
}
