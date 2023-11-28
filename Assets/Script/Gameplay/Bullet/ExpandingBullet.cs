using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingBullet : Bullet
{
    public float expandingForce = 2.0f;
    protected override void STATE_FIXUPDATE_AIR()
    {
        return;
    }
    protected new void STATE_START()
    {
        
        this.gameObject.transform.position = new Vector3(
            this.gameObject.transform.position.x,
            0f,
            this.gameObject.transform.position.z);
    }
    protected override void STATE_AIR()
    {
        base.STATE_AIR();
        this.gameObject.transform.localScale = 
            new Vector3(
                this.gameObject.transform.localScale.x + (expandingForce * Time.deltaTime),
                Mathf.Clamp(this.gameObject.transform.localScale.x + (expandingForce * Time.deltaTime), 0f, 6f),
                this.gameObject.transform.localScale.z + (expandingForce * Time.deltaTime)
        );
        
        rb.velocity = Vector3.zero;
    }
}
