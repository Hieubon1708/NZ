using UnityEngine;

public class EnemyT1 : EnemyHandler
{
    public override void Start()
    {
        base.Start();
        SetDamage();
    }

    public override void SetDamage()
    {
        base.SetDamage();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Car") && !isStunByWeapon)
        {
            animator.SetInteger("attackRandomizer", Random.Range(0, 2));
            animator.SetBool("attack", true);
            isAttack = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameController.instance.isLose && collision.CompareTag("Car") && !isStunByWeapon)
        {
            if (BlockController.instance.tempBlocks.Count != 0)
            {
                if (enemyInfo.transform.position.y > PlayerController.instance.transform.position.y - 0.1f)
                {
                    if (animator.GetBool("attack"))
                    {
                        animator.SetBool("attack", false);
                        isAttack = false;
                    }
                }
                else 
                {
                    if (!animator.GetBool("attack"))
                    {
                        animator.SetInteger("attackRandomizer", Random.Range(0, 2));
                        animator.SetBool("attack", true);
                        isAttack = true;
                    }
                }
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Car") && !isStunByWeapon)
        {
            animator.SetBool("attack", false);
            isAttack = false;
        }
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
