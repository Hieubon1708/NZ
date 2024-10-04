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
        if (collision.CompareTag("Car"))
        {
            animator.SetInteger("attackRandomizer", Random.Range(0, 2));
            animator.SetBool("attack", true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Car")) animator.SetBool("attack", false);
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
