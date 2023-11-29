using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool frameConsistent = false;
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
        if (frameConsistent) {
            transform.forward = Camera.main.transform.forward;
        }
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