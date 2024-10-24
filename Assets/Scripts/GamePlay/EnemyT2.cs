﻿using DG.Tweening;
using UnityEngine;

// con bắn đạn ở dưới
public class EnemyT2 : EnemyHandler
{
    public float targetX;

    public override void Start()
    {
        base.Start();
        SetHp();
        targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1, GameController.instance.xPlus2);
    }

    protected override void FixedUpdate()
    {
        float walkSpeed = 0f;
        float speed = this.speed;

        if (isCollisionWithCar)
        {
            walkSpeed = -GameController.instance.backgroundSpeed * multiplier;
        }
        else
        {
            walkSpeed = -rb.velocity.x * multiplier;
        }

        if (isCollisionWithCar
            || gameObject.layer == layerBumping
            || frontalCollision != null
            || isStunByWeapon)
        {
            speed = 0;
        }

        if(GameController.instance.isLose && BlockController.instance.tempBlocks.Count == 0)
        {
            if (animator.GetBool("attack"))
            {
                isShot = false;
                animator.SetBool("attack", false);
            }
        }
        else
        {
            if (Mathf.Abs(enemyInfo.transform.position.x - PlayerController.instance.transform.position.x) < targetX)
            {
                walkSpeed = 0;
                if (!animator.GetBool("attack"))
                {
                    isShot = true;
                    animator.SetBool("attack", true);
                }
                if(isCollisionWithGround)
                {
                    speed = 0;
                }
            }
            else
            {
                if (animator.GetBool("attack"))
                {
                    isShot = false;
                    animator.SetBool("attack", false);
                    targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1, GameController.instance.xPlus2);
                }
            }
        }
        
        rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", walkSpeed);
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
        targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1, GameController.instance.xPlus2);
    }

    public override void SetDefaultField()
    {
        base.SetDefaultField();
        delayRevival.Kill();
        content.SetActive(false);
        gameObject.layer = layerOrigin;
        colObj.layer = layerOrigin;
    }
}
