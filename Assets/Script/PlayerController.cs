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

    private void Start()
    {
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
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");
                rb.velocity = new Vector3(horizontalInput * (currentVelocity), 0f,verticalInput * (currentVelocity) );

                break;
            default:break;
        }
    }
    void InputProcessor()
    {
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

                    SpawnBullet(objectHit);
                    state = CharacterSTATES.IDLE;
                    ReduceSize();
                }
                break;
            case CharacterSTATES.HIT:
                ReduceSize();
                state = CharacterSTATES.IDLE;
                break;
            case CharacterSTATES.DEAD:
                Destroy(this.gameObject);
                break;
            default: break;
        }
    }

    private void ReduceSize()
    {
        scale -= stepsForScale;
        if (scale < 0.1f)
        {
            scale = 0.1f;
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
        GUI.Label(new Rect(10, 10, 200, 30), $"Bullets: {bullets}");
        GUI.Label(new Rect(10, 30, 200, 30), $"Max Bullets: {maxBullets}");
        GUI.Label(new Rect(10, 50, 200, 30), $"HP: {health}");
        GUI.Label(new Rect(10, 70, 200, 30), $"Current velocity: {currentVelocity}");
    }
}
