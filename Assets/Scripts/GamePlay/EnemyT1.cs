using DG.Tweening;
using UnityEngine;

public class EnemyT1 : EnemyHandler
{
    public override void Start()
    {
        base.Start();
        SetDamage();
        SetHp();
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        base.OnCollisionStay2D(collision);
        if (!collision.collider.gameObject.activeSelf || !colObj.activeSelf || !content.activeSelf || blockCollision != null) return;
        if (collision.gameObject.CompareTag("Block"))
        {
            blockCollision = StartCoroutine(BlockCollisionHandle(collision.rigidbody.gameObject, int.Parse(name)));
        }
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
        if (collision.CompareTag("Player")) playerCollision = StartCoroutine(PlayerTriggerHandle(int.Parse(name)));
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
            else
            {
                animator.SetBool("attack", false);
                isAttack = false;
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
        if (collision.CompareTag("Player"))
        {
            if (playerCollision != null) StopCoroutine(playerCollision);
        }
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
