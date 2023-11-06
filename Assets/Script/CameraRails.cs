using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRails : MonoBehaviour
{
    PlayerController playerRef = null;
    void Start()
    {
        playerRef = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (playerRef != null)
        {
            this.gameObject.transform.position = new Vector3(playerRef.transform.position.x, playerRef.transform.position.y + 12.16f, playerRef.transform.position.z + -15.6f);
        }
    }
}
