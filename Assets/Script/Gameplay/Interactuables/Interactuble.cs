using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactuble : MonoBehaviour
{
    public delegate void interacted();
    public event interacted Interacted;
    protected bool hasInteracted = false;
    public void invokeInteracted()
    {
        Interacted?.Invoke();
        hasInteracted = true;
    }
    public void invokeInteractedOnce()
    {
        if (!hasInteracted)
        {
            Interacted?.Invoke();
            hasInteracted = true;
        }
    }
}
