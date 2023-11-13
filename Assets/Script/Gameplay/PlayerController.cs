using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : CharacterController
{

    public float scale = 1;
    private float maxScale = 1;
    private float stepsForScale = 0.1f;
    // Start is called before the first frame update

    bool isCameraControlled = false;
    Vector3 transitionPoint = Vector3.zero;
    private void Start()
    {
        if (colisions == null)
        {
            colisions = GetComponentInChildren<Collider>();
        }
        rb = GetComponent<Rigidbody>();
        maxScale = transform.localScale.x;
        stepsForScale = 0.5f / maxBullets;
    }
    // Update is called once per frame
    void Update()
    {
        currentVelocity = Mathf.Lerp(velocity, maxVelocity, (scale*-1)+1);

        InputProcessor();
        CheckIfDead();
        StateMachine();
        float newScale = Mathf.Lerp(0.2f, maxScale, scale);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case CharacterSTATES.IDLE:
                rb.velocity = Vector3.zero;
                break;
            case CharacterSTATES.MOVE:
                if (!isCameraControlled)
                {//If the player isn't controlled by the camera
                    float horizontalInput = Input.GetAxis("Horizontal");
                    float verticalInput = Input.GetAxis("Vertical");
                    rb.velocity = new Vector3(horizontalInput * (currentVelocity), 0f, verticalInput * (currentVelocity));
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    rb.useGravity = false;
                    colisions.gameObject.SetActive(false);
                    float step = currentVelocity * Time.deltaTime;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, transitionPoint, step);
                    if (Vector3.Distance(this.transform.position, transitionPoint) <= 0.5f)
                    {
                        isCameraControlled = false;
                        colisions.gameObject.SetActive(true);
                        rb.useGravity = true;

                    }
                }

                break;
            default:break;
        }
    }
    void InputProcessor()
    {
        if (isCameraControlled)
        {
            return;
        }
        if (state == CharacterSTATES.IDLE || state == CharacterSTATES.MOVE)
        {
            if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
            {
                state = CharacterSTATES.MOVE;
            }
            else
            {
                state = CharacterSTATES.IDLE;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                state = CharacterSTATES.ATTACK;
            }
        }
       
    }
    void StateMachine()
    {
        switch (state)
        {
            case CharacterSTATES.IDLE:
                //Animation change
                break;
            case CharacterSTATES.MOVE:
                //Animation change

                break;
            case CharacterSTATES.ATTACK:
                //Animation change
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 objectHit = hit.point;

                    //SpawnBullet(Camera.main.ScreenToWorldPoint(Input.mousePosition)); Deprecated only for 2d

                    if (SpawnBullet(objectHit))
                    {
                        ReduceSize();
                    }
                    state = CharacterSTATES.IDLE;
                    
                }
                break;
            case CharacterSTATES.HIT:
                //Animation change
                state = CharacterSTATES.IDLE;
                break;
            case CharacterSTATES.DEAD:
                Destroy(this.gameObject);
                break;
            default: break;
        }
    }
    public void SetTransitionPoint(Vector3 point, bool changeState = false)
    {
        transitionPoint = point;
        isCameraControlled = changeState;
        if (changeState ) { state = CharacterSTATES.MOVE; }
    }
    private void ReduceSize()
    {
        scale -= stepsForScale;
        if (scale < 0.1f)
        {
            scale = 0.1f;
        }
    }

    public override void HitDamage(int hp)
    {
        state = CharacterSTATES.HIT;
        if (bullets <= 0)
        {
            health -= hp;
        }
        else
        {
            /*for (int i = 0; i < hp; i++)
            {
            } Old iterator spawn bullets for each damage point    */
                SpawnBullet(Vector3.zero,false,Bullet.BulletStates.AfterHit);//Spawn bullet on the ground
            ReducceBullets(); //Reducce the amount of bullets
            ReduceSize();
        }
    }

    public void AugmentSize()
    {
        scale += stepsForScale;
        if (scale > maxScale)
        {
            scale = maxScale;
        }
    }
    public override void AddBullets(int quantity = 1) 
    {
        bullets += quantity;
        AugmentSize();
        if (bullets >= maxBullets)
        {
            bullets = maxBullets;
        }
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(5, 5, 130, 200), "");
        GUI.Label(new Rect(10, 10, 200, 30), $"Bullets: {bullets}");
        GUI.Label(new Rect(10, 30, 200, 30), $"Max Bullets: {maxBullets}");
        GUI.Label(new Rect(10, 50, 200, 30), $"HP: {health}");
        GUI.Label(new Rect(10, 70, 200, 30), $"Current velocity: {currentVelocity}");
    }
}
