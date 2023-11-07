using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : GameplayObject
{
    public GameObject prefabBullet;

    public enum CharacterSTATES
    {
        IDLE, MOVE, HIT, ATTACK, DEAD, DISABLED, _LAST
    }

    public int health = 10;
    public int maxHealth = 10;
    
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

    public delegate void IsDead();
    public event IsDead characterIsDead;
    void Start()
    {
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
    public void HitDamage(int hp)
    {
        health -= hp;
        state = CharacterSTATES.HIT;
    }
 
    protected bool SpawnBullet(Vector3 target)
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
        GameObject bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
        Bullet classBullet = bullet.GetComponent<Bullet>();
        classBullet.SetDirection(targetForBullet);
        classBullet.SetParent(this.gameObject);
        classBullet.SetDamage(damagePoints);
        if (!infiniteBullets)
        {
            bullets -= 1;
        }
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
    }
    public virtual void ReducceBullets(int quantity = 1)
    {
        bullets -= quantity;
        if (bullets <= 0)
        {
            bullets = 0;
        }
    }
}
