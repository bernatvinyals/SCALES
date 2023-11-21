using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HP : MonoBehaviour
{

    float negative = 1f;
    float angle = 0f;
    Animator animator = null;
    public bool flipV = false;
    private void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        angle = Random.value * 14;
    }
    private void Update()
    {
        angle += Time.deltaTime * 2f * negative;
        if (angle > 14f)
        {
            negative = -1f;
        }
        else if (angle < -14f)
        {
            negative = 1f;
        }
        this.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,1f * angle));
    }
    public void LateUpdate()
    {
        //Using LATE UPDATE in order to alter the animation values.
        if (flipV)
        {
            this.transform.localScale = new Vector2(
                this.transform.localScale.x,
                this.transform.localScale.y * -1
                );
        }
    }
    public void Show()
    {
        animator.SetBool("hide", false);
    }
    public void Hide()
    {
        animator.SetBool("hide", true);
    }
}
