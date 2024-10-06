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
        if (isCollisionWithCar
            || gameObject.layer == layerBumping
            || frontalCollision != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else if (isStunByWeapon)
        {
            rb.velocity = new Vector2(-GameController.instance.backgroundSpeed * multiplier, rb.velocity.y);
        }
        else if (Mathf.Abs(enemyInfo.transform.position.x - PlayerController.instance.transform.position.x) < targetX)
        {
            if (!animator.GetBool("attack"))
            {
                isShot = true;
                animator.SetBool("attack", true);
            }
            rb.velocity = new Vector2(-GameController.instance.backgroundSpeed * multiplier, rb.velocity.y);
        }
        else
        {
            if (animator.GetBool("attack"))
            {
                isShot = false;
                animator.SetBool("attack", false);
                targetX = EUtils.RandomXDistanceByCar(xPlus1, xPlus2);
            }
            rb.velocity = new Vector2(speed * multiplier, rb.velocity.y);
        }
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", Mathf.Abs(!isStunByWeapon || !(Mathf.Abs(enemyInfo.transform.position.x - PlayerController.instance.transform.position.x) < targetX) ? speed * multiplier : 0));
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
        targetX = EUtils.RandomXDistanceByCar(xPlus1, xPlus2);
    }
}
