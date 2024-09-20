using System.Collections;
using UnityEngine;

public class EnemyT1 : EnemyHandler
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay")) content.SetActive(true);
        if (!content.activeSelf) yield break;
        if (collision.CompareTag("Car")) animator.SetBool("attack", true);
        StartCoroutine(base.OnTriggerEnter2D(collision));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (isCollisionWithCar
            || amoutCollision == 2
            && listNormals[0].y == 0
            && listNormals[1].y == 0
            || isStunned && collision.contacts[0].normal.x >= 0.99f) isWalk = false;
        else isWalk = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
    }
}
