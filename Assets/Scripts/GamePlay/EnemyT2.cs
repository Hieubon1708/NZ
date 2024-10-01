using UnityEngine;

// con bắn đạn ở dưới
public class EnemyT2 : EnemyHandler
{
    public float xMin;
    public float xMax;
    public float targetX;

    public override void Start()
    {
        base.Start();
        targetX = GetTargetX();
    }

    protected override void FixedUpdate()
    {
        if (isCollisionWithCar
            || gameObject.layer == layerBumping
            || frontalCollision != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else if (Mathf.Abs(enemyInfo.transform.position.x - PlayerController.instance.transform.position.x) < targetX)
        {
            if (!animator.GetBool("attack"))
            {
                isShot = true;
                animator.SetBool("attack", true);
            }
            rb.velocity = new Vector2(-GameController.instance.backgroundSpeed, rb.velocity.y);
        }
        else
        {
            if (animator.GetBool("attack"))
            {
                isShot = false;
                animator.SetBool("attack", false);
                targetX = GetTargetX();
            }
            rb.velocity = new Vector2(speed * multiplier, rb.velocity.y);
        }
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
        targetX = GetTargetX();
    }

    float GetTargetX()
    {
        return Random.Range(xMin, xMax);
    }
}
