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
        if (!content.activeSelf) return;
        base.OnTriggerExit2D(collision);
    }

    protected override void FixedUpdate()
    {
        if (isCollisionWithCar
            || amoutCollision >= 2
            || enemyInfo.transform.position.x <= targetX)
        {
            if (!animator.GetBool("attack"))
            {
                targetX = GetTargetX();
                animator.SetBool("attack", true);
            }
            isWalk = false;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            if (animator.GetBool("attack"))
            {
                animator.SetBool("attack", false);
            }
            isWalk = true;
            rb.velocity = new Vector2(speed * multiplier, rb.velocity.y);
        }
    }

    public override void Jump()
    {
        if (enemyInfo.transform.position.x <= targetX) return;
        base.Jump();
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
