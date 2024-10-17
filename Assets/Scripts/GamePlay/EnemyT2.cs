using UnityEngine;

// con bắn đạn ở dưới
public class EnemyT2 : EnemyHandler
{
    public float targetX;
    public float xPlus1, xPlus2;

    public override void Start()
    {
        base.Start();
        SetDamage();
        targetX = EUtils.RandomXDistanceByCar(xPlus1, xPlus2);
    }

    public override void SetDamage()
    {
        base.SetDamage();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
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
            walkSpeed = -(rb.velocity.x - GameController.instance.backgroundSpeed) * multiplier;
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
                    targetX = EUtils.RandomXDistanceByCar(xPlus1, xPlus2);
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
        targetX = EUtils.RandomXDistanceByCar(xPlus1, xPlus2);
    }
}
