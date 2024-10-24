﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject hitObj;
    public SpriteRenderer[] fullBodies;
    public HitEffect hitEffect;

    public float forceJump;
    public float multiplier;
    public float realSpeed;
    public float startSpeed;
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
    Coroutine shockerTrigger;
    Coroutine flameTrigger;
    Coroutine flameBurningTrigger;
    Coroutine blockCollision;
    protected Coroutine playerCollision;

    protected LayerMask layerOrigin;
    protected LayerMask layerBumping;

    public virtual void Start()
    {
        hitObj = content;
    }

    public void GetLayer()
    {
        lineIndex = EUtils.GetIndexLine(gameObject);
        layerOrigin = gameObject.layer;
        layerBumping = LayerMask.NameToLayer("Line_" + lineIndex);
    }

    public virtual void SetHp()
    {
        healthHandler.SetTotalHp(enemyInfo.hp);
    }

    public virtual void SetDamage()
    {
        name = enemyInfo.damage.ToString();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay") && !content.activeSelf)
        {
            content.SetActive(true);
            speed = realSpeed;
            col.enabled = false;
            col.enabled = true;
        }
        if (!content.activeSelf || enemyInfo.hp == 0) return;
        int subtractHp;
        if (collision.CompareTag("Bullet"))
        {
            subtractHp = int.Parse(collision.name);
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("MachineGun"))
        {
            subtractHp = int.Parse(collision.name);
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
            subtractHp = int.Parse(collision.name);
            sawTrigger = StartCoroutine(SawTriggerHandle(subtractHp));
        }
        if (collision.CompareTag("Shocker"))
        {
            subtractHp = int.Parse(collision.name);
            shockerTrigger = StartCoroutine(ShockerTriggerHandle(subtractHp));
        }
        if (collision.CompareTag("ShockerBooster"))
        {
            subtractHp = int.Parse(collision.attachedRigidbody.name);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("Flame"))
        {
            subtractHp = int.Parse(collision.name);
            if (flameBurningTrigger == null && UpgradeEvolutionController.instance.flames.Contains(UpgradeEvolutionController.FLAMEEVO.BURNING))
            {
                int damageBurning;
                ParController.instance.PlayFlameThrowerParticle(transform.position, transform, out damageBurning);
                flameBurningTrigger = StartCoroutine(FlameBurningTriggerHandle(damageBurning));
            }
            flameTrigger = StartCoroutine(FlameTriggerHandle(subtractHp));
        }
    }

    IEnumerator FlameBurningTriggerHandle(int subtractHp)
    {
        while (enemyInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeFlameBurningDamage);
        }
    }

    protected IEnumerator PlayerTriggerHandle(int subtractHp)
    {
        while (PlayerController.instance.player.hp > 0 && !isStunByWeapon)
        {
            PlayerController.instance.playerHandler.SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeBlockNPlayerDamage);
        }
    }

    IEnumerator BlockCollisionHandle(GameObject b, int subtractHp)
    {
        if (b == null) yield break;
        BlockHandler scB = BlockController.instance.GetScBlock(b).blockHandler;
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
    
    IEnumerator ShockerTriggerHandle(int subtractHp)
    {
        while (enemyInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeShockerDamage);
        }
    }

    IEnumerator FlameTriggerHandle(int subtractHp)
    {
        while (enemyInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
        }
    }

    public void StartBumpByWeapon()
    {
        isBumping = true;
        gameObject.layer = layerBumping;
        colObj.layer = layerBumping;
    }

    public void EndBumpByWeapon()
    {
        isBumping = false;
        gameObject.layer = layerOrigin;
        colObj.layer = layerOrigin;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Saw"))
        {
            if (sawTrigger != null) StopCoroutine(sawTrigger);
        }
        if (collision.CompareTag("Flame"))
        {
            if (flameTrigger != null) StopCoroutine(flameTrigger);
        }
        if (collision.CompareTag("Boom")) SubtractHp(int.Parse(collision.attachedRigidbody.name));
    }

    public void Stun(float time)
    {
        if (animator.GetBool("attack")) animator.SetBool("attack", false);
        if (stunByWeapon != null) StopCoroutine(stunByWeapon);
        stunByWeapon = StartCoroutine(StunByWeapon(time));
    }

    public IEnumerator StunByWeapon(float time)
    {
        isStunByWeapon = true;
        yield return new WaitForSeconds(time);
        isStunByWeapon = false;
        if (isAttack) animator.SetBool("attack", true);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.activeSelf || !colObj.gameObject.activeSelf || !collision.gameObject.activeSelf) return;
        if (collision.gameObject.CompareTag("Block")) blockCollision = StartCoroutine(BlockCollisionHandle(collision.rigidbody.gameObject, int.Parse(name)));
        if (collision.gameObject.CompareTag("Ground")) isCollisionWithGround = true;
    }

    public bool a;

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.gameObject.activeSelf && !colObj.gameObject.activeSelf || !collision.gameObject.activeSelf) return;
        if ((collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Car")) && collision.contacts[0].normal.x >= 0.99f) isCollisionWithCar = true;
        if (collision.contacts[0].normal.y >= 0.99f && isJump) isJump = false;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.contacts[0].normal.x >= 0.85f && !isBumping && collision.gameObject != frontalCollision)
            {
                frontalCollision = collision.gameObject;
            }
        }
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

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            if (blockCollision != null) StopCoroutine(blockCollision);
        }
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
            && !isStunByWeapon
            && collision.gameObject.CompareTag("Enemy")
            && collision.contacts[0].normal.x >= 0.95f
            && !isStunned
            && !isCollisionWithCar
            && !isShot)
        {
            jump = StartCoroutine(JumpStart(collision));
        }
    }

    void CheckBump(Collision2D collision)
    {
        if (isCollisionWithGround
            && collision.contacts[0].normal.y <= -0.85f)
        {
            if (isCollisionWithCar) StartCoroutine(CarController.instance.Bump(layerBumping, layerOrigin, colObj, collision.rigidbody.gameObject, collision.collider.gameObject, rb.gameObject, this, col.bounds.size.y - collision.collider.bounds.size.x));
            else if (isStunByWeapon)
            {
                if (GameController.instance.EBlockNearest(layerOrigin) == gameObject)
                {
                    StartCoroutine(CarController.instance.Bump(layerBumping, layerOrigin, colObj, collision.rigidbody.gameObject, collision.collider.gameObject, rb.gameObject, this, col.bounds.size.y - collision.collider.bounds.size.x));
                }
            }
        }
    }

    public virtual void SpawnbyTime() { }

    protected virtual void FixedUpdate()
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

        if (isCollisionWithCar
            || gameObject.layer == layerBumping
            || frontalCollision != null
            || isStunByWeapon)
        {
            speed = 0;
        }

        rb.velocity = new Vector2(-(speed + GameController.instance.backgroundSpeed) * multiplier, rb.velocity.y);
        animator.SetFloat("velocityY", rb.velocity.y);
        animator.SetFloat("walkSpeed", walkSpeed);
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
        if (enemyInfo.hp == 0) return;
        if (!healthBar.activeSelf) healthBar.SetActive(true);
        float hp = enemyInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        damage.ShowDamage(subtractHp.ToString(), hitObj);
        hitEffect.PlayHitEffect(fullBodies); 
        if (hp == 0)
        {
            ParController.instance.PlayZomDieParticle(hitObj.transform.position);
            DeathHandle();
        }
    }

    public Tween delayRevival;

    protected virtual void DeathHandle()
    {
        SetColNKinematicNRevival(false);
        StopCoroutines();
        UIHandler.instance.FlyGold(enemyInfo.transform.position, 2);
        SetDeathAni();
        healthBar.SetActive(false);
        GameController.instance.listEVisible.Remove(gameObject);

        delayRevival = DOVirtual.DelayedCall(1f, delegate
        {
            EnemyTowerController.instance.ERevival(enemyInfo.gameObject, this);
        });
    }

    public void SetDeathAni()
    {
        int deathRandomizer = Random.Range(0, 10);
        animator.SetInteger("deathRandomizer", deathRandomizer);
        animator.SetTrigger("death");
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
    }

    public void SetColNKinematicNRevival(bool isEnable)
    {
        colObj.SetActive(isEnable);
        rb.isKinematic = !isEnable;
    }

    public void Restart()
    {
        StopCoroutines();
        SetDefaultField();
    }

    protected virtual void StopCoroutines()
    {
        if (jump != null)
        {
            StopCoroutine(jump); 
        }
        if (sawTrigger != null)
        {
            StopCoroutine(sawTrigger); 
        }
        if (shockerTrigger != null)
        {
            StopCoroutine(shockerTrigger); 
        }
        if (flameTrigger != null)
        {
            StopCoroutine(flameTrigger);
        }
        if (stunnedDelay != null)
        {
            StopCoroutine(stunnedDelay); 
        }
        if (stunByWeapon != null)
        {
            StopCoroutine(stunByWeapon);
        }
        if (blockCollision != null)
        {
            StopCoroutine(blockCollision);
        }
        if (playerCollision != null)
        {
            StopCoroutine(playerCollision);
        }
        if (flameBurningTrigger != null)
        {
            StopCoroutine(flameBurningTrigger);
        }
    }

    public virtual void SetDefaultField()
    {
        animator.Rebind();
        content.transform.DOKill();
        speed = startSpeed;
        col.enabled = true;
        rb.isKinematic = false;
        isCollisionWithCar = false;
        isStunned = false;
        isJump = false;
        isStunByWeapon = false;
        isShot = false;
        isBumping = false;
        frontalCollision = null;       
        view.SetActive(true);
        colObj.SetActive(true);
        SetColNKinematicNRevival(true);
        healthBar.SetActive(false);
        healthHandler.SetDefaultInfo(enemyInfo);
    }

    IEnumerator SetFalseIsStunned(float time)
    {
        yield return new WaitForSeconds(time);
        isStunned = false;
    }
}
