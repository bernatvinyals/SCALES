using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : CharacterController
{

    PlayerController playerRef = null;

    public float coondownBetweenShots = 1f;
    private float initalWait = 0f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        infiniteBullets = true;
        initalWait = Random.Range(1f, 3f);
        //TODO
        //CHANGE THIS TO A MANAGER GIVING THE PLAYER REFERENCE
        playerRef = FindAnyObjectByType<PlayerController>();
        //TODO
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        CheckIfDead();
        StateMachine();
    }
    
    void StateMachine()
    {
        switch (state)
        {
            case CharacterSTATES.IDLE:
                //Animation change
                if (timer > initalWait && playerRef != null)
                {
                    state = CharacterSTATES.ATTACK;
                    timer = 0f;
                }

                break;
            case CharacterSTATES.MOVE:
                //Animation change

                break;
            case CharacterSTATES.ATTACK:
                if (timer >= coondownBetweenShots)
                {
                    if (playerRef != null)
                    {
                        SpawnBullet(playerRef.transform.position);
                        timer = 0f;
                        //Animation change
                    }
                    else
                    {
                        state = CharacterSTATES.IDLE;
                    }
                    
                }

                break;
            case CharacterSTATES.HIT:
                //Animation change
                Debug.Log("Hit");
                state = CharacterSTATES.IDLE;
                break;
            case CharacterSTATES.DEAD:
                //Animation change
                Destroy(this.gameObject);
                break;
            default: break;
        }
    }

}
