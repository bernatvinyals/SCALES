using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Interactuble
{

    private void OnTriggerEnter(Collider other)
    {
        GameplayObject a = other.gameObject.transform.parent.GetComponent<GameplayObject>();
        bool b = a != null;
        if (b)
        {
            if((a.flags & (GameplayFlags.INSTACED_FROM_PLAYER)) != 0)
            {
                //If its form a player
                b = true;
            }else { b = false; }
        }
        if (other.gameObject.tag == "Player" || b)
        {
            this.invokeInteractedOnce(); 
        }
    }
}
