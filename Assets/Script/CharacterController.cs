using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject prefabBullet;

    public enum CharacterSTATES
    {
        IDLE, MOVE, HIT, ATTACK, DEAD, _LAST
    }

    public int health = 100;
    public int maxHealth = 100;
    
    public float velocity = 10f;
    protected float currentVelocity = 10f;
    public float maxVelocity = 10f;

    public int bullets = 10;
    public int maxBullets = 10;
    public bool infiniteBullets = false;
    protected CharacterSTATES state = CharacterSTATES.IDLE;


    protected Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void HitDamage(int hp)
    {
        health -= hp;
        state = CharacterSTATES.HIT;
    }
 
    protected void SpawnBullet(Vector3 target)
    {
        if (bullets <= 0)
        {
            bullets = 0;
            return;
        }
        Vector3 worldPosition = target;
        Vector3 targetForBullet = gameObject.transform.position - worldPosition;
        targetForBullet = targetForBullet.normalized * -1;
        targetForBullet.z = 0.0f;
        GameObject bullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
        Bullet classBullet = bullet.GetComponent<Bullet>();
        classBullet.SetDirection(targetForBullet);
        classBullet.SetParent(this.gameObject);
        if (!infiniteBullets)
        {
            bullets -= 1;
        }

    }
    protected bool CheckIfDead()
    {
        if (health <= 0)
        {
            state = CharacterSTATES.DEAD;
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
}
