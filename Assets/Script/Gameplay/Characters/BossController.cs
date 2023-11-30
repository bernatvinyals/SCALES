using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    public List<GameObject> typeBullets = new List<GameObject>();
    protected override void LateStart()
    {
        prefabBullet = typeBullets[0];
        characterRecivedDamage += () =>
        {
            if (health < 140)
            {
                prefabBullet = typeBullets[1];
            }
            else if (health < 60)
            {
                prefabBullet = typeBullets[2];
            }
        };
    }
}
