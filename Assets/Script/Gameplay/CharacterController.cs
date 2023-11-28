using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterController;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

public class CharacterController : GameplayObject
{

    public GameObject prefabBullet;

    public enum CharacterSTATES
    {
        IDLE, MOVE, HIT, ATTACK, DEAD, DISABLED, _LAST
    }

    public int health = 10;
    public int maxHealth = 10;
    [NonSerialized]
    public float uiMaxHp = 10f; //Normalized hp between 10

    public float velocity = 10f;
    protected float currentVelocity = 10f;
    public float maxVelocity = 10f;

    public int bullets = 10;
    public int maxBullets = 10;
    public bool infiniteBullets = false;
    protected CharacterSTATES state = CharacterSTATES.IDLE;

    public int damagePoints = 1;

    protected Rigidbody rb;
    public Collider colisions;

    Animator animator;


    //EVENT Character shoots
    public delegate void HasShoot();
    public event HasShoot hasShoot;



    //EVENT Character gets hurt
    public delegate void IsHurt();
    public event IsHurt characterRecivedDamage;

    //EVENT Character dies
    public delegate void IsDead();
    public event IsDead characterIsDead;
    void Start()
    {
        SetMaxHP(maxHealth);
        if (colisions == null)
        {
            colisions = GetComponentInChildren<Collider>();
        }
        rb = GetComponent<Rigidbody>();
    }
    public void SetState(CharacterSTATES in_state)
    {
        this.state = in_state;
        if (state == CharacterSTATES.DEAD)
        {
            characterIsDead?.Invoke();
        }
    }
    public CharacterSTATES GetState()
    {
        return this.state;
    }
    public virtual void HitDamage(int hp)
    {
        health -= hp;
        state = CharacterSTATES.HIT;
        BroadcastHurt();
    }
    public virtual void SetMaxHP(int hp)
    {
        health = hp;
        maxHealth = hp;
        BroadcastHurt(); //Calling Hit so anything regarding hp
                                          // is going to get notified
    }

    protected bool SpawnBullet(Vector3 target, bool wasteBullets = true, Bullet.BulletStates state = Bullet.BulletStates.Air)
    {
        if (bullets <= 0)
        {
            bullets = 0;
            return false;
        }
        Vector3 worldPosition = target;
        worldPosition.y = this.gameObject.transform.position.y;
        Vector3 targetForBullet = gameObject.transform.position - worldPosition;
        targetForBullet = targetForBullet.normalized * -1;
        GameObject bullet = Instantiate(prefabBullet, 
            transform.position, Quaternion.identity);
        Bullet classBullet = bullet.GetComponent<Bullet>();
        classBullet.SetDirection(targetForBullet);
        classBullet.SetParent(this.gameObject);
        classBullet.SetDamage(damagePoints);
        classBullet.SetState(state);
        if (!infiniteBullets && wasteBullets)
        {
            bullets -= 1;
        }

        BroadcastShoot();
        return true;

    }
    protected bool CheckIfDead()
    {
        if (health <= 0)
        {
            SetState(CharacterSTATES.DEAD);
        }
        return health <= 0;
    }
    public virtual void AddBullets(int quantity = 1) 
    {
        bullets += quantity;
        if (bullets >= maxBullets)
        {
            bullets = maxBullets;
        }
        BroadcastShoot();
    }
    public virtual void ReducceBullets(int quantity = 1)
    {
        bullets -= quantity;
        if (bullets <= 0)
        {
            bullets = 0;
        }
    }
    protected void BroadcastHurt()
    {
        uiMaxHp = ((float)(health) / (float)(maxHealth)); 
        characterRecivedDamage?.Invoke();
    }
    protected void BroadcastShoot()
    {
        hasShoot?.Invoke();
    }
}
