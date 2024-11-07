using DG.Tweening;
using UnityEngine;

// con nhện to 
public class EnemyT4 : EnemyHandler
{
    public GameObject body;
    public bool isDeadByTower;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player")) playerCollision = StartCoroutine(PlayerTriggerHandle(int.Parse(name)));
    }

    public override void Start() { }

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

        if (isCollisionWithCar || !colObj.activeSelf)
        {
            speed = 0;
        }

        rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", walkSpeed);
    }

    public override void SetDefaultField() { }

    public override void DeathHandle()
    {
        UIHandler.instance.daily.CheckDaily(Daily.DailyType.DestroyEnemy);
        SetColNKinematicNRevival(false);
        healthBar.SetActive(false);
        StopCoroutines();
        if (!isDeadByTower) UIHandler.instance.FlyGold(enemyInfo.transform.position, 2);
        SetDeathAni();
        GameController.instance.listEVisible.Remove(gameObject);
    }

    public override void SpawnbyTime()
    {
        isDeadByTower = false;
        if (healthHandler.startHp == 0)
        {
            SetDamage();
            SetHp();
            hitObj = content;
        }
        float xTower = EnemyTowerController.instance.GetTower().col.transform.position.x;
        float xCol = GameController.instance.colDisplayPos.x;
        float x = xCol > xTower ? xTower : xCol;
        float y = CarController.instance.spawnY[0].transform.position.y;
        transform.position = new Vector2(x, y);
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
        if (collision.gameObject.CompareTag("Block")) blockCollision = StartCoroutine(BlockCollisionHandle(collision.rigidbody.gameObject, int.Parse(name)));
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car"))
        {
            animator.SetBool("attack", false);
            isAttack = false;
        }
    }
}
