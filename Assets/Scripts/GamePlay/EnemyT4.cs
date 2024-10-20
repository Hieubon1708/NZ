using DG.Tweening;
using UnityEngine;

// con nhện to 
public class EnemyT4 : EnemyHandler
{
    public GameObject body;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player")) playerCollision = StartCoroutine(PlayerTriggerHandle(int.Parse(name)));
    }

    public override void Start() { }

    public override void SetHp()
    {
        base.SetHp();
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

        if (isCollisionWithCar || !colObj.activeSelf)
        {
            speed = 0;
        }

        rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", walkSpeed);
    }

    protected override void DeathHandle()
    {
        SetColNKinematicNRevival(false);
        healthBar.SetActive(false);
        StopCoroutines();
        UIHandler.instance.FlyGold(enemyInfo.transform.position, 2);
        SetDeathAni();
        GameController.instance.listEVisible.Remove(gameObject);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void SpawnbyTime()
    {
        if (healthHandler.startHp == 0)
        {
            SetDamage();
            SetHp();
            hitObj = content;
        }
        float x = GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1;
        float y = CarController.instance.spawnY[0].transform.position.y;
        transform.position = new Vector2(x, y);
    }

    protected override void StopCoroutines()
    {
        base.StopCoroutines();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car"))
        {
            animator.SetInteger("attackRandomizer", Random.Range(0, 2));
            animator.SetBool("attack", true);
            isAttack = true;
        }
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
    }

    public override void SetDefaultField()
    {
        base.SetDefaultField();
    }
}
