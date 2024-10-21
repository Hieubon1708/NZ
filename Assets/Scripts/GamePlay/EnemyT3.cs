﻿using System.Collections;
using UnityEngine;

// con bắn đạn, bay ở trên
public class EnemyT3 : EnemyHandler
{
    public Vector2 targetPos;
    private Vector2 velocity = Vector2.zero;
    float yRandomAfterLevingCave;
    public bool isLevingCave;
    Coroutine levingCave;
    public float targetX;

    public override void Start()
    {
        hitObj = content;
        SetHp();
        yRandomAfterLevingCave = EUtils.RandomYDistanceByCar(GameController.instance.yPlus1, GameController.instance.yPlus2);
        targetX = CarController.instance.transform.position.x + GameController.instance.xPlus2;
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Tower") && EnemyTowerController.instance.GetTower().col == collision.gameObject && collision.gameObject.activeSelf) levingCave = StartCoroutine(LevingCave());
    }

    protected override void FixedUpdate()
    {
        float walkSpeed = 1f;
        float speed = this.speed;

        if (GameController.instance.isLose && BlockController.instance.tempBlocks.Count == 0)
        {
            if (animator.GetBool("attack"))
            {
                animator.SetBool("attack", false);
                if (levingCave != null) StopCoroutine(levingCave);
            }
            rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
        }
        else
        {
            if (Mathf.Abs(enemyInfo.transform.position.x - PlayerController.instance.transform.position.x) < targetX)
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
                    if (a) Debug.LogWarning(targetPos.ToString());

                    Vector2 targetPosition = Vector2.SmoothDamp(rb.position, targetPos, ref velocity, 0.3f);
                    rb.MovePosition(targetPosition);
                }
                else
                {
                    if (a) Debug.LogWarning("a");

                    targetPos = RandomTarget();
                }
            }
            else
            {
                rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
            }
        }

        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", walkSpeed + Mathf.Clamp(velocity.y, -0.5f, 0.5f));
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
        isLevingCave = false;
    }

    Vector2 RandomTarget()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector2 dir = new Vector2(x, y);

        return EUtils.ClampXYDistanceByCar(transform.position, dir, GameController.instance.xPlus1, GameController.instance.xPlus2, GameController.instance.yPlus1, GameController.instance.yPlus2);
    }

    IEnumerator LevingCave()
    {
        yield return new WaitUntil(() => Mathf.Abs(EnemyTowerController.instance.GetTower().col.transform.position.x - enemyInfo.transform.position.x) > 1.75f);
        rb.velocity = new Vector2(rb.velocity.x, 3.5f);
        if (transform.position.x <= targetX) yRandomAfterLevingCave = EUtils.RandomXDistanceByCar(GameController.instance.yPlus1, GameController.instance.yPlus2 / 2);
        yield return new WaitWhile(() => transform.position.y <= yRandomAfterLevingCave);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        isLevingCave = true;
    }

    protected override void StopCoroutines()
    {
        base.StopCoroutines();
        if (levingCave != null)
        {
            StopCoroutine(levingCave);
        }
    }
}
