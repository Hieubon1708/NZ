using DG.Tweening;
using UnityEngine;

// con bắn đạn ở dưới
public class EnemyT2 : EnemyHandler
{
    public float targetX;
    public EnemyAttackEventT2 attackEventT2;

    public override void Start()
    {
        base.Start();
        SetHp();
        targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1 + 3, GameController.instance.xPlus2 - 0.5f);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("ColDisplay"))
        {
            Debug.DrawLine(transform.position, new Vector2(CarController.instance.transform.position.x + targetX, transform.position.y), Color.red, 1);
        }
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
            walkSpeed = (Mathf.Abs(rb.velocity.x) - GameController.instance.backgroundSpeed) * multiplier;
        }

        if (isCollisionWithCar
            || gameObject.layer == layerBumping
            || frontalCollision != null
            || isStunByWeapon)
        {
            speed = 0;
        }

        if (GameController.instance.isLose && BlockController.instance.tempBlocks.Count == 0)
        {
            if (animator.GetBool("attack"))
            {
                isShot = false;
                animator.SetBool("attack", false);
            }
        }
        else
        {
            if (Mathf.Abs(enemyInfo.transform.position.x - CarController.instance.transform.position.x) <= targetX)
            {
                walkSpeed = 0;
                if (!animator.GetBool("attack"))
                {
                    isShot = true;
                    animator.SetBool("attack", true);
                }
                if (isCollisionWithGround)
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
                    targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1 + 3, GameController.instance.xPlus2 - 0.5f);
                }
            }
        }

        rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", walkSpeed);
    }

    public override void Restart()
    {
        attackEventT2.SetActiveBullet(false);
        base.Restart();
    }

    public override void DeathHandle()
    {
        base.DeathHandle();
        targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1 + 3, GameController.instance.xPlus2 - 0.5f);
    }

    public override void SetDefaultField()
    {
        base.SetDefaultField();
        delayRevival.Kill();
        content.SetActive(false);
        /*gameObject.layer = layerOrigin;
        colObj.layer = layerOrigin;*/
    }
}
