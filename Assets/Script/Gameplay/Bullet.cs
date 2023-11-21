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
    float maxTimeAliveOnGround = 200f;

    public float pickupCooldown = 1f;
    private float timer = 0;
    private float timerGrabCooldown = 0f;
    protected BulletStates state = BulletStates.Air;



    int damage = 10;
    protected GameObject parent = null;
    protected bool isFromPlayer = false;

    public Collider damageColider = null;
    public Collider grabColider = null;
    private void Start()
    {
        maxTimeAliveOnGround = 200f;
        rb = GetComponent<Rigidbody>();
        if (grabColider != null)
        {
            grabColider.isTrigger = true;
        }

    }
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
                rb.velocity = direction * velocity;
                break;
            default:
                break;
        }
        
    }
    private void Update()
    {
        
        switch (state)
        {
            case BulletStates.Air:
                timer += Time.deltaTime;
                if (timer > maxTimeAliveOnAir)
                {
                    state = BulletStates.Expired;
                }
                break;

            case BulletStates.Hit:
                if (isFromPlayer)
                {
                    state = BulletStates.AfterHit;
                }
                else
                {
                    state = BulletStates.Expired;
                }
                break;

            case BulletStates.AfterHit:
                timer = 0f;
                timerGrabCooldown = 0f;
                velocity = 0f;
                damageColider.isTrigger = false;
                
                direction = Vector3.zero;
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                state = BulletStates.PickupCooldown;
                /*gameObject.transform.position = new Vector3(
                    Random.Range(
                    gameObject.transform.position.x - 1.0f,
                    gameObject.transform.position.x + 1.0f),
                    gameObject.transform.position.y,
                    Random.Range(
                    gameObject.transform.position.z - 1.0f,
                    gameObject.transform.position.z + 1.0f)                
                ); Random spawn for grabs   */
                break;
            case BulletStates.PickupCooldown:
                timerGrabCooldown += Time.deltaTime;
                if (timerGrabCooldown > pickupCooldown)
                {
                    grabColider.gameObject.SetActive(true);
                    state = BulletStates.Ground;
                }
                break;
            case BulletStates.Ground:
                timer += Time.deltaTime;
                if (timer > maxTimeAliveOnGround)
                {
                    state = BulletStates.Expired;
                }
                break;

            case BulletStates.Expired:
                if (parent!= null)
                {
                    parent.GetComponent<CharacterController>().AddBullets();
                }
                Destroy(this.gameObject);
                break;
            
            default:
                break;
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
