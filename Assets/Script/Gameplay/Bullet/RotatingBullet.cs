using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBullet : Bullet
{
    public float angle = 0;

    protected override void STATE_FIXUPDATE_AIR()
    {
        if (parent == null)
        {
            state = BulletStates.AfterHit; return;
        }
        float a = 0;
        float distanceBetweenParent = Vector3.Distance(this.transform.position, parent.transform.position);
        distanceBetweenParent = distanceBetweenParent / 10f;
        distanceBetweenParent = Mathf.Abs(distanceBetweenParent);
        a = Mathf.Lerp(10f, 0.2f, distanceBetweenParent);
        angle += a * Time.fixedDeltaTime;

        var lastPos = this.transform.position;
        //angle = 180 * Mathf.PI / 180;
        this.transform.position = RotatePointAroundPivot(this.transform.position, parent.transform.position, new Vector3(0, angle, 0));
        this.transform.Rotate(Vector3.up * angle, Space.Self);
        this.transform.position += this.transform.forward * 2.3f * Time.fixedDeltaTime;
        Debug.DrawLine(lastPos, transform.position, Color.red, 1f);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot , Vector3 angles){
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles)* dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
