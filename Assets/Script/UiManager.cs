using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(500, 10, 200, 200), "FPS: "+((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }
}
