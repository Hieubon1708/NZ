using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHandler : MonoBehaviour
{
    public Enemy enemyInfo;
    public GameObject healthBar;
    public HealthHandler healthHandler;
    public Damage damage;
    public Rigidbody2D rb;
    public SortingGroup sortingGroup;
    public GameObject colObj;
    public CapsuleCollider2D col;
    public Animator animator;
    bool isTriggerSaw;
    bool isTriggerFlame;
    public float forceJump;
    public float multiplier;
    public float speed;
    public float timeStunned;
    public float timeJump;
    public bool isCollisionWithCar;
    public bool isCollisionWithGround;
    public bool isJump;
    public bool isWalk;
    public bool isStunned;
    public bool isBumping;
    public bool isShot;
    public int amoutCollision;
    public int lineIndex;
    public GameObject view;
    public GameObject frontalCollision;

    Coroutine stunnedDelay;
    Coroutine jump;
    protected LayerMask layerOrigin;
    protected LayerMask layerBumping;

    public virtual void Start()
    {
        lineIndex = EUtils.GetIndexLine(gameObject);
        layerOrigin = gameObject.layer;
        layerBumping = LayerMask.NameToLayer("Line_" + lineIndex);
    }

    void OnEnable()
    {
        healthHandler.SetTotalHp(enemyInfo.hp);
    }

    protected IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay")) view.SetActive(true);
        if (!view.activeSelf) yield break;
        int subtractHp;
        if (collision.CompareTag("Bullet"))
        {
            subtractHp = 70;
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("MachineGun"))
        {
            subtractHp = int.Parse(collision.gameObject.name);
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("SawBooster"))
        {
            subtractHp = int.Parse(collision.attachedRigidbody.name);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("Saw"))
        {
            subtractHp = int.Parse(collision.gameObject.name);
            isTriggerSaw = true;
            while (isTriggerSaw && enemyInfo.hp > 0)
            {
                SubtractHp(subtractHp);
                yield return new WaitForSeconds(GameController.instance.timeSawDamage);
            }
        }
        if (collision.CompareTag("Flame"))
        {
            subtractHp = int.Parse(collision.gameObject.name);
            isTriggerFlame = true;
            while (isTriggerFlame && enemyInfo.hp > 0)
            {
                SubtractHp(subtractHp);
                yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
            }
        }
        if (collision.CompareTag("Car") && name != "0") animator.SetBool("attack", true);
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (!view.activeSelf) return;
        if (collision.CompareTag("Saw")) isTriggerSaw = false;
        if (collision.CompareTag("Flame")) isTriggerFlame = false;
        if (collision.CompareTag("Boom")) SubtractHp(499);
        if (collision.CompareTag("Car") && name != "0") animator.SetBool("attack", false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car")) isCollisionWithCar = true;
        if (collision.gameObject.CompareTag("Ground")) isCollisionWithGround = true;
    }

    public bool a;

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (!view.activeSelf) return;
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car")) isCollisionWithCar = true;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.contacts[0].normal.x >= 0.99f && !isBumping) frontalCollision = collision.gameObject;
        }
        if (collision.contacts[0].normal.y >= 0.99f && isJump) isJump = false;
        if (collision.gameObject.CompareTag("Enemy") && !isStunned)
        {
            if (collision.contacts[0].normal.y < 0f || collision.contacts[0].normal.x < -0.99f)
            {
                if (isJump)
                {
                    if (jump != null) StopCoroutine(jump);
                    JumpEnd();
                }

                isStunned = true;
                timeStunned = Random.Range(0.45f, 0.75f);

                if (stunnedDelay != null) StopCoroutine(stunnedDelay);
                stunnedDelay = StartCoroutine(SetFalseIsStunned(timeStunned));
            }
        }
        if (frontalCollision != null)
        {
            if (frontalCollision.layer == layerBumping)
            {
                gameObject.layer = layerBumping;
                colObj.layer = layerBumping;
            }
            if (frontalCollision.layer == layerOrigin)
            {
                gameObject.layer = layerOrigin;
                colObj.layer = layerOrigin;
            }
        }
        CheckJump(collision);
        CheckBump(collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car")) isCollisionWithCar = false;
        if (collision.gameObject.CompareTag("Ground")) isCollisionWithGround = false;
        if (collision.gameObject == frontalCollision)
        {
            gameObject.layer = layerOrigin;
            colObj.layer = layerOrigin;
            frontalCollision = null;
        }
    }

    public void CheckJump(Collision2D collision)
    {
        if (!isJump
            && collision.gameObject.CompareTag("Enemy")
            && collision.contacts[0].normal.x >= 0.99f
            && !isStunned
            && !isCollisionWithCar
            && !isShot)
        {
            jump = StartCoroutine(JumpStart(collision));
        }
    }

    void CheckBump(Collision2D collision)
    {
        if (isCollisionWithCar
            && isCollisionWithGround
            && collision.contacts[0].normal.y <= -0.85f)
        {
            StartCoroutine(CarController.instance.Bump(layerBumping, layerOrigin, colObj, collision.rigidbody.gameObject, rb.gameObject, this, col.bounds.size.y - collision.collider.bounds.size.x));
        }
    }

    protected virtual void FixedUpdate()
    {
        if (isCollisionWithCar
            || gameObject.layer == layerBumping
            || frontalCollision != null)
        {
            isWalk = false;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            isWalk = true;
            rb.velocity = new Vector2(speed * multiplier, rb.velocity.y);
        }
    }

    protected IEnumerator JumpStart(Collision2D collision)
    {
        isJump = true;
        Collider2D col = collision.collider;
        animator.SetFloat("velocityY", 3);
        rb.velocity = new Vector2(rb.velocity.x, forceJump);
        float maxY = rb.position.y + col.bounds.size.y;
        Vector2 target = GetPositionTopBound(col);
        while (rb.position.y <= maxY && rb.position.y <= target.y)
        {
            target = GetPositionTopBound(col);
            yield return new WaitForFixedUpdate();
        }
        JumpEnd();
    }

    Vector2 GetPositionTopBound(Collider2D col)
    {
        return new Vector2(col.bounds.max.x - col.bounds.extents.x, col.bounds.max.y);
    }

    void JumpEnd()
    {
        animator.SetFloat("velocityY", 0);
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    void SubtractHp(float subtractHp)
    {
        if (!view.activeSelf) return;
        if (!healthBar.activeSelf) healthBar.SetActive(true);
        float hp = enemyInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        damage.ShowDamage(subtractHp.ToString());
        if (hp == 0) DeathHandle();
    }

    protected virtual void DeathHandle()
    {
        view.SetActive(false);
        healthHandler.SetDefaultInfo(enemyInfo);
        enemyInfo.gameObject.SetActive(false);
        healthBar.SetActive(false);
        SetDefaultField();
        GameController.instance.listEVisible.Remove(gameObject);
        ParController.instance.PlayZomDieParticle(enemyInfo.transform.position);
        EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].ERevival(enemyInfo.gameObject);
    }

    private void OnDisable()
    {
        SetDefaultField();
    }

    public void SetDefaultField()
    {
        isCollisionWithCar = false;
        isCollisionWithGround = true;
        isStunned = false;
        isTriggerFlame = false;
        isTriggerSaw = false;
        isWalk = false;
        isJump = false;
        frontalCollision = null;
        if (stunnedDelay != null) StopCoroutine(stunnedDelay);
        if (jump != null) StopCoroutine(jump);
        gameObject.layer = layerOrigin;
        colObj.layer = layerOrigin;
        colObj.SetActive(true);
    }
    IEnumerator SetFalseIsStunned(float time)
    {
        yield return new WaitForSeconds(time);
        isStunned = false;
    }
}
