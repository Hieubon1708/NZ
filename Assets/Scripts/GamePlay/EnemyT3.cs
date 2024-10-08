﻿using System.Collections;
using UnityEngine;

// con bắn đạn, bay ở trên
public class EnemyT3 : EnemyHandler
{
    public Vector2 targetPos;
    float yRandomAfterLevingCave;
    bool isLevingCave;
    Coroutine levingCave;
    public float xPlus1, xPlus2;
    public float yPlus1, yPlus2;
    float targetX;

    public override void Start()
    {
        base.Start();

        yRandomAfterLevingCave = EUtils.RandomYDistanceByCar(yPlus1, yPlus2);
        targetX = CarController.instance.transform.position.x + xPlus2;
        if (transform.position.y >= CarController.instance.transform.position.y + yPlus1) isLevingCave = true;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Tower")) levingCave = StartCoroutine(LevingCave());
    }

    protected override void FixedUpdate()
    {
        if (transform.position.x <= targetX)
        {
            if (!animator.GetBool("attack")) animator.SetBool("attack", true);
            if (!isLevingCave) return;
            if (rb.velocity != Vector2.zero)
            {
                rb.velocity = Vector2.zero;
                targetPos = RandomTarget();
            }
            if (Vector2.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position, targetPos, 0.1f);
                //Debug.DrawLine(transform.position, targetPos, Color.red, 2);
            }
            else
            {
                targetPos = RandomTarget();
            }
        }
        else
        {
            rb.velocity = new Vector2(speed * multiplier, rb.velocity.y);
        }
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
        if (levingCave != null) StopCoroutine(levingCave);
    }

    Vector2 RandomTarget()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector2 dir = new Vector2(x, y);

        return EUtils.ClampXYDistanceByCar(transform.position, dir, xPlus1, xPlus2, yPlus1, yPlus2);
    }

    IEnumerator LevingCave()
    {
        rb.velocity = new Vector2(rb.velocity.x, 3.5f);
        if (transform.position.x <= targetX) yRandomAfterLevingCave = EUtils.RandomXDistanceByCar(yPlus1, yPlus2 / 2);
        yield return new WaitWhile(() => transform.position.y <= yRandomAfterLevingCave);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        isLevingCave = true;
    }
}
