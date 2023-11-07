using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRails : MonoBehaviour
{
    public enum CameraState
    {
        FOLLOW_PLAYER, TRANSITIONING, IDLE
    }
    
    PlayerController playerRef = null;
    public CameraState state = CameraState.FOLLOW_PLAYER;
    public GameObject cameraPoint = null;
    public float timeBetween = 1f;
    private float timer = 0f;
    void Start()
    {
        playerRef = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case CameraState.FOLLOW_PLAYER:
                FollowPlayer();
                break; 

            case CameraState.TRANSITIONING:
                timer += Time.deltaTime;
                GoToPosition();
                if (timer >= timeBetween)
                {
                    timer = 0f;
                    state = CameraState.FOLLOW_PLAYER;
                }
                break;

            case CameraState.IDLE:
                break;

            default:
                break;
        }
       
    }

    void FollowPlayer()
    {
        if (playerRef != null)
        {
            this.gameObject.transform.position = new Vector3(playerRef.transform.position.x, playerRef.transform.position.y + 6.46f, playerRef.transform.position.z + -10f);
        }
    }
    void GoToPosition()
    {

        this.gameObject.transform.position = new Vector3(
            Mathf.Lerp(this.gameObject.transform.position.x, cameraPoint.transform.position.x, 0.5f),
            Mathf.Lerp(this.gameObject.transform.position.y, cameraPoint.transform.position.y, 0.5f),
            Mathf.Lerp(this.gameObject.transform.position.z, cameraPoint.transform.position.z, 0.5f)
            );
    }
    public void SetNewPosition(GameObject newPoint, bool changeState = false)
    {
        cameraPoint = newPoint;
        if (changeState)
        {
            state = CameraState.TRANSITIONING;
        }
    }
}
