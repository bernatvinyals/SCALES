using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    enum BulletStates{
        Air, Hit, AfterHit, Ground, Expired, _Last
    }

    Vector3 direction = Vector3.zero;
    
    Rigidbody rb;
    float velocity = 50f;
    public bool activated = true;
    public float maxTimeAlive = 5f;
    private float timer = 0;
    BulletStates state = BulletStates.Air;

    int damage = 10;
    GameObject parent = null;
    bool isFromPlayer = false;

    public Collider damageColider = null;
    public Collider grabColider = null;
    private void Start()
    {
        state = BulletStates.Air;
        rb = GetComponent<Rigidbody>();
        if (grabColider != null)
        {
            grabColider.isTrigger = true;
        }

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
                if (timer > maxTimeAlive)
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
                velocity = 0;
                damageColider.isTrigger = false;
                grabColider.gameObject.SetActive(true);
                direction = Vector3.zero;
                rb.velocity = Vector3.zero;
                rb.useGravity = true;
                state = BulletStates.Ground;
                break;
            case BulletStates.Ground:

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
        if (!activated)
        {
            return;
        }
        if (parent == null)
        {
            return;
        }
        if (collision.gameObject.tag != parent.gameObject.tag)
        {
            CharacterController cc = collision.gameObject.GetComponentInParent<CharacterController>();
            if (cc == null) { return; }
            if (state == BulletStates.Air)
            {
                cc.HitDamage(damage);
                state = BulletStates.Hit;
                return;
            }
        }
        else
        {
            CharacterController cc = collision.gameObject.GetComponentInParent<CharacterController>();
            if (state == BulletStates.Ground)
            {
                cc.AddBullets();
                state = BulletStates.Expired;
            }
        }
        
    }
}
