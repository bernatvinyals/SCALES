using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : GameplayObject
{
    public enum BulletStates{
        Air, Hit, AfterHit,PickupCooldown, Ground, Expired, _Last
    }

    protected Vector3 direction = Vector3.zero;
    
    protected Rigidbody rb;
    public float velocity = 50f;
    public bool activated = true;
    public float maxTimeAliveOnAir = 5f;
    public float maxTimeAliveOnGround = 0f;

    public float pickupCooldown = 1f;
    private float timer = 0;
    private float timerGrabCooldown = 0f;
    protected BulletStates state = BulletStates.Air;



    int damage = 10;
    protected GameObject parent = null;
    protected bool isFromPlayer = false;
    public bool WaitForTimeAlive = false; //If this is true the bullet won't desapear on
                                          //player hit but when maxTimeAliveOnAir is 0f


    public Collider damageColider = null;
    public Collider grabColider = null;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (grabColider != null)
        {
            grabColider.isTrigger = true;
        }
        STATE_START();
    }
    protected void STATE_START() { }
    public void SetState(BulletStates in_state)
    {
        state = in_state;
    }
    public void SetDirection(Vector3 vect)
    {
        direction = vect;
    }
    public void SetParent(GameObject go)
    {
        parent = go; 
        if (parent != null)
        {
            if (parent.GetComponent<PlayerController>())
            {
                isFromPlayer = true;
            }
        }
    }
    public void SetDamage(int in_damage)
    {
        damage = in_damage;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case BulletStates.Air:
                STATE_FIXUPDATE_AIR();
                break;

            case BulletStates.Hit:
                STATE_FIXUPDATE_HIT();
                break;

            case BulletStates.AfterHit:
                STATE_FIXUPDATE_AFTERHIT();

                break;
            case BulletStates.PickupCooldown:
                STATE_FIXUPDATE_PICKUPCOOLDOWN();
                break;
            case BulletStates.Ground:
                STATE_FIXUPDATE_GROUND();
                break;

            case BulletStates.Expired:
                STATE_FIXUPDATE_EXPIRED();
                break;
            default:
                break;
        }
        
    }

    protected virtual void STATE_FIXUPDATE_EXPIRED() { }

    protected virtual void STATE_FIXUPDATE_GROUND() { }

    protected virtual void STATE_FIXUPDATE_PICKUPCOOLDOWN() { }

    protected virtual void STATE_FIXUPDATE_AFTERHIT() { }

    protected virtual void STATE_FIXUPDATE_HIT() { }

    protected virtual void STATE_FIXUPDATE_AIR()
    {
        rb.velocity = direction * velocity;
    }

    private void Update()
    {
        
        switch (state)
        {
            case BulletStates.Air:
                STATE_AIR();
                break;

            case BulletStates.Hit:
                STATE_HIT();
                break;

            case BulletStates.AfterHit:
                STATE_AFTERHIT();

                break;
            case BulletStates.PickupCooldown:
                STATE_PICKUPCOOLDOWN();
                break;
            case BulletStates.Ground:
                STATE_GROUND();
                break;

            case BulletStates.Expired:
                STATE_EXPIRED();
                break;

            default:
                break;
        }
    }

    protected virtual void STATE_EXPIRED()
    {
        if (parent != null)
        {
            parent.GetComponent<CharacterController>().AddBullets();
        }
        if (WaitForTimeAlive)
        {
            if (timer > maxTimeAliveOnAir)
            {
                Destroy(this.gameObject);
            }
                    
            state = BulletStates.Air;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void STATE_GROUND()
    {
        timer += Time.deltaTime;
        if (timer > maxTimeAliveOnGround)
        {
            state = BulletStates.Expired;
        }
    }

    protected virtual void STATE_PICKUPCOOLDOWN()
    {
        timerGrabCooldown += Time.deltaTime;
        if (timerGrabCooldown > pickupCooldown)
        {
            grabColider.gameObject.SetActive(true);
            state = BulletStates.Ground;
        }
    }

    protected virtual void STATE_AFTERHIT()
    {
        timer = 0f;
        timerGrabCooldown = 0f;
        velocity = 0f;
        damageColider.isTrigger = false;

        direction = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.useGravity = true;
        state = BulletStates.PickupCooldown;

        gameObject.transform.position = new Vector3(
            Random.Range(
            gameObject.transform.position.x - 1.0f,
            gameObject.transform.position.x + 1.0f),
            gameObject.transform.position.y,
            Random.Range(
            gameObject.transform.position.z - 1.0f,
            gameObject.transform.position.z + 1.0f)
        );/* Random spawn for grabs   */
    }

    protected virtual void STATE_HIT()
    {
        if (isFromPlayer)
        {
            state = BulletStates.AfterHit;
        }
        else
        {
            state = BulletStates.Expired;
        }
    }

    protected virtual void STATE_AIR()
    {
        timer += Time.deltaTime;
        if (timer > maxTimeAliveOnAir)
        {
            state = BulletStates.Expired;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!activated || parent == null)
        {
            return;
        }
        
        GameplayObject go = collision.gameObject.GetComponentInParent<GameplayObject>();
        if (go == null)
        { //If it isn't a gameplay object

            if (collision.gameObject.tag == "Killplane")
            {//Check if its kill plane
                state = BulletStates.Expired;
            }
            return;
        }
        if ((go.flags & (GameplayFlags.IGNORE_BULLETS)) != 0)
        {// If it is a gameplay object and has a flag to ignore bullets
            return;
        }

        CharacterController cc = go.gameObject.GetComponentInParent<CharacterController>();
        if (cc == null) {
            //If it's not a Character
            if ((go.flags & (GameplayFlags.GROUND_BULLETS)) != 0)
            { //Check if has a flag to bounce bullets
                if (parent.gameObject.tag == "Player")
                {//In case the parent of the bullet is the player
                    state = BulletStates.AfterHit;
                }
                else
                {//If it's anything else
                    state = BulletStates.Expired;

                }
            }
            return;
        }
        if (cc.GetState() == CharacterController.CharacterSTATES.DISABLED)
        {
            return;
        }
        if (collision.gameObject.tag != parent.gameObject.tag)
        {//If tag is different means it's an enemy, in the prespective of the player or the reverse
            if (state == BulletStates.Air)
            {
                cc.HitDamage(damage);
                state = BulletStates.Hit;
                return;
            } 
        }
        else
        {//If the bullet parent triggers on the bullet
            if (state == BulletStates.Ground)
            {
                cc.AddBullets();
                state = BulletStates.Expired;
            }         
        }

    }
}
