using System.Collections;
using UnityEngine;

// con bắn đạn ở dưới
public class EnemyT2 : EnemyHandler
{
    public float xMin;
    public float xMax;
    public float targetX;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetX = GetTargetX();
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay")) content.SetActive(true);
        if (!content.activeSelf) yield break;
        StartCoroutine(base.OnTriggerEnter2D(collision));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (isJump || isCollisionWithCar) isWalk = false;
        if (enemyInfo.transform.position.x <= targetX
            && isCollisionWithGround)
        {
            rb.velocity = Vector2.zero;
            if (isWalk)
            {
                isWalk = false;
                targetX = GetTargetX();
            }
        }
        else if(enemyInfo.transform.position.x > targetX
            || !isCollisionWithGround) isWalk = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(enemyInfo.transform.position.x <= targetX)
        {
            if(!animator.GetBool("attack"))
            {
                animator.SetBool("attack", true);
            }
        }
        else if (animator.GetBool("attack"))
        {
            animator.SetBool("attack", false);
        }
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
    }

    float GetTargetX()
    {
        return Random.Range(xMin, xMax);
    }
}
