using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTransition : MonoBehaviour
{
    public GameObject newPlayerPoint = null;

    private void OnTriggerEnter(Collider other)
    {
        if (newPlayerPoint != null)
        {
            if(other.gameObject.tag == "Player")
            {
                PlayerController player = other.gameObject.transform.parent.GetComponent<PlayerController>();
                player?.SetTransitionPoint(newPlayerPoint.transform.position, true);
                
            }
        }
    }
}
