using DG.Tweening;
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
    public GameObject content;
    public GameObject view;
    public GameObject frontalCollision;

    public float forceJump;
    public float multiplier;
    public float speed;
    public float timeStunned;
    public float timeJump;

    public bool isCollisionWithCar;
    public bool isCollisionWithGround;
    public bool isJump;
    public bool isStunByWeapon;
    public bool isStunned;
    public bool isBumping;
    public bool isAttack;
    public bool isShot;
    public int amoutCollision;
    public int lineIndex;

    public Coroutine stunByWeapon;
    Coroutine stunnedDelay;
    Coroutine jump;
    Coroutine sawTrigger;
    Coroutine flameTrigger;
    Coroutine blockCollision;
    Coroutine playerCollision;
    public LayerMask layerOrigin;
    public LayerMask layerBumping;

    public virtual void Start()
    {
        lineIndex = EUtils.GetIndexLine(gameObject);
        layerOrigin = gameObject.layer;
        layerBumping = LayerMask.NameToLayer("Line_" + lineIndex);
        healthHandler.SetTotalHp(enemyInfo.hp);
    }

    public virtual void SetDamage()
    {
        name = enemyInfo.damage.ToString();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay")) content.SetActive(true);
        if (!content.activeSelf) return;
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
            sawTrigger = StartCoroutine(SawTriggerHandle(subtractHp));
        }
        if (collision.CompareTag("Flame"))
        {
            subtractHp = int.Parse(collision.gameObject.name);
            flameTrigger = StartCoroutine(FlamewTriggerHandle(subtractHp));
        }
    }

    IEnumerator PlayerCollisionHandle(int subtractHp)
    {
        while (PlayerHandler.instance.playerInfo.hp > 0 && !isStunByWeapon)
        {
            PlayerHandler.instance.SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeBlockNPlayerDamage);
        }
    }

    IEnumerator BlockCollisionHandle(GameObject b, int subtractHp)
    {
        BlockHandler scB = BlockController.instance.GetScBlock(b).blockHandler;
        if (scB == null) yield break;
        while (scB.blockInfo.hp > 0 && !isStunByWeapon)
        {
            scB.SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeBlockNPlayerDamage);
        }
    }

    IEnumerator SawTriggerHandle(int subtractHp)
    {
        while (enemyInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeSawDamage);
        }
    }

    IEnumerator FlamewTriggerHandle(int subtractHp)
    {
        while (enemyInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (!content.activeSelf) return;
        if (collision.CompareTag("Saw"))
        {
            if (sawTrigger != null) StopCoroutine(sawTrigger);
        }
        if (collision.CompareTag("Flame"))
        {
            if (flameTrigger != null) StopCoroutine(flameTrigger);
        }
        if (collision.CompareTag("Boom")) SubtractHp(499);
    }

    public IEnumerator StunByWeapon(float time)
    {
        isStunByWeapon = true;
        yield return new WaitForSeconds(time);
        isStunByWeapon = false;
        if(isAttack) animator.SetBool("attack", true);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerCollision == null) StartCoroutine(PlayerCollisionHandle(int.Parse(name)));
        }
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car"))
        {
            isCollisionWithCar = true;
            if (blockCollision == null) StartCoroutine(BlockCollisionHandle(collision.rigidbody.gameObject, int.Parse(name)));
        }
        if (collision.gameObject.CompareTag("Ground")) isCollisionWithGround = true;
    }

    public bool a;

    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (!content.activeSelf) return;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerCollision != null) StopCoroutine(playerCollision);
        }
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car"))
        {
            isCollisionWithCar = false;
            if(blockCollision != null) StopCoroutine (blockCollision);
        }
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
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else if (isStunByWeapon)
        {
            rb.velocity = new Vector2(-GameController.instance.backgroundSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(speed * multiplier, rb.velocity.y);
        }
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", Mathf.Abs(!isStunByWeapon ? speed * multiplier : 0));
    }

    protected IEnumerator JumpStart(Collision2D collision)
    {
        isJump = true;
        Collider2D col = collision.collider;
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

    public Vector2 GetPositionTopBound(Collider2D col)
    {
        return new Vector2(col.bounds.max.x - col.bounds.extents.x, col.bounds.max.y);
    }

    void JumpEnd()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    void SubtractHp(float subtractHp)
    {
        if (!healthBar.activeSelf) healthBar.SetActive(true);
        float hp = enemyInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        damage.ShowDamage(subtractHp.ToString());
        if (hp == 0) DeathHandle();
    }

    protected virtual void DeathHandle()
    {
        SetColNKinematicNRevival(false);

        int deathRandomizer = Random.Range(0, 10);

        SetDeathAni(deathRandomizer);
        healthHandler.SetDefaultInfo(enemyInfo);
        healthBar.SetActive(false);

        GameController.instance.listEVisible.Remove(gameObject);
        if (enemyInfo.hp == 0) ParController.instance.PlayZomDieParticle(enemyInfo.transform.position);

        DOVirtual.DelayedCall(deathRandomizer == 0 ? 1 : 0, delegate
        {
            EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].ERevival(enemyInfo.gameObject, this);
        });
    }

    public void SetDeathAni(int deathRandomizer)
    {
        animator.SetInteger("deathRandomizer", deathRandomizer);
        animator.SetTrigger("death");
        SetLayerWeight(0);
    }

    public void SetColNKinematicNRevival(bool isEnable)
    {
        rb.isKinematic = !isEnable;
        col.enabled = isEnable;
    }

    public void SetActiveContentNView(bool isActive)
    {
        view.SetActive(isActive);
        content.SetActive(!isActive);
    }

    public void ResetBone()
    {
        enemyInfo.bone.ResetBone();
    }

    public void SetLayerWeight(int weight)
    {
        animator.SetLayerWeight(1, weight);
        animator.SetLayerWeight(2, weight);
    }

    private void OnDisable()
    {
        SetDefaultField();
    }

    public void SetDefaultField()
    {
        col.enabled = true;
        rb.isKinematic = false;
        isCollisionWithCar = false;
        isStunned = false;
        isJump = false;
        isStunByWeapon = false;
        isShot = false;
        frontalCollision = null;

        if (jump != null) StopCoroutine(jump);
        if (sawTrigger != null) StopCoroutine(sawTrigger);
        if (flameTrigger != null) StopCoroutine(flameTrigger);
        if (stunnedDelay != null) StopCoroutine(stunnedDelay);
        if (blockCollision != null) StopCoroutine(blockCollision);
        if (playerCollision != null) StopCoroutine(playerCollision);

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
