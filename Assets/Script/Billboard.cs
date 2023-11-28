using UnityEngine;

public class Billboard : MonoBehaviour
{
    void OnEnable()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            enabled = false;
        }
    }

    private void Start()
    {
        transform.forward = Camera.main.transform.forward;
        
    }

    void LateUpdate()
    {

    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }
}