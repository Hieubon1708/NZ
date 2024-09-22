﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public int amoutCollision;
    public int lineIndex;
    public GameObject content;
    public GameObject frontalCollision;

    public List<GameObject> listCollisions = new List<GameObject>();
    public List<Vector2> listNormals = new List<Vector2>();
    public ContactPoint2D[] listContacts = new ContactPoint2D[10];

    Coroutine stunnedDelay;
    Coroutine jump;
    LayerMask layerOrigin;
    LayerMask layerBumping;

    public void Start()
    {
        lineIndex = EUtils.GetIndexLine(gameObject);
        layerOrigin = gameObject.layer;
        layerBumping = LayerMask.NameToLayer("Line_" + lineIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && a)
        {
            //StartCoroutine(CarController.instance.Bump(lineIndex, gameObject, nameOrigin));
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.SetFloat("velocityY", 3);
            //Jump();
        }
    }
    public bool b;

    protected virtual void OnEnable()
    {
        healthHandler.SetTotalHp(enemyInfo.hp);
    }

    protected virtual IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        int subtractHp;
        if (collision.CompareTag("Bullet"))
        {
            subtractHp = 70;
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("MachineGun"))
        {
            subtractHp = int.Parse(collision.attachedRigidbody.name);
            collision.gameObject.SetActive(false);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("Saw"))
        {
            subtractHp = GameController.instance.listDamages[collision.attachedRigidbody.gameObject];
            isTriggerSaw = true;
            while (isTriggerSaw && enemyInfo.hp > 0)
            {
                SubtractHp(subtractHp);
                yield return new WaitForSeconds(GameController.instance.timeSawDamage);
            }
        }
        if (collision.CompareTag("Flame"))
        {
            subtractHp = GameController.instance.listDamages[collision.attachedRigidbody.gameObject];
            isTriggerFlame = true;
            while (isTriggerFlame && enemyInfo.hp > 0)
            {
                SubtractHp(subtractHp);
                yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Saw")) isTriggerSaw = false;
        if (collision.CompareTag("Flame")) isTriggerFlame = false;
        if (collision.CompareTag("Boom"))
        {
            SubtractHp(499);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block")) isCollisionWithCar = true;
        if (collision.gameObject.CompareTag("Ground")) isCollisionWithGround = true;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.contacts[0].normal.y >= 0.99f && isJump)
            {
                if (jump != null) StopCoroutine(jump);
                JumpEnd();
            }
        }
    }

    public GameObject d;
    public bool a;

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block")) isCollisionWithCar = true;
        if (collision.gameObject.CompareTag("Enemy") && collision.contacts[0].normal.x >= 0.99f) frontalCollision = collision.gameObject;

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
                timeStunned = Random.Range(0.25f, 0.45f);

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

        GetContacts();
        CheckJump(collision);
        CheckBump(collision);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block")) isCollisionWithCar = false;
        if (collision.gameObject.CompareTag("Ground")) isCollisionWithGround = false;
        if (collision.gameObject == frontalCollision)
        {
            gameObject.layer = layerOrigin;
            colObj.layer = layerOrigin;
            frontalCollision = null;
        }
    }

    void CheckJump(Collision2D collision)
    {
        if (!isJump
            && collision.contacts[0].normal.x >= 0.99f
            && Mathf.Abs(collision.contacts[0].rigidbody.transform.position.y - transform.position.y) <= 0.01f
            && !isStunned
            && !isCollisionWithCar)
        {
            jump = StartCoroutine(JumpStart());
        }
    }

    public IEnumerator Bump(GameObject droping)
    {
        gameObject.layer = layerBumping;
        colObj.layer = layerBumping;
        CarController.instance.isBump[lineIndex - 1] = true;
        yield return new WaitWhile(() => Mathf.Abs(droping.transform.position.y - transform.position.y) >= 0.5f);
        CarController.instance.isBump[lineIndex - 1] = false;
        gameObject.layer = layerOrigin;
        colObj.layer = layerOrigin;
    }

    void GetContacts()
    {
        amoutCollision = 0;
        int length = rb.GetContacts(listContacts);
        listCollisions.Clear();

        for (int i = 0; i < length; i++)
        {
            if ((listContacts[i].normal.x >= 0.99f || listContacts[i].normal.x <= -0.99f || listContacts[i].normal.y < 0)
                && !listCollisions.Contains(listContacts[i].rigidbody.gameObject)
                && listContacts[i].collider.gameObject.CompareTag("Enemy"))
            {
                listCollisions.Add(listContacts[i].rigidbody.gameObject);
                listNormals.Add(listContacts[i].normal);
                amoutCollision++;
            }
        }
    }

    void CheckBump(Collision2D collision)
    {
        if (isCollisionWithCar
            && isCollisionWithGround
            && collision.contacts[0].normal.y <= -0.85f
            && !CarController.instance.isBump[lineIndex - 1])
        {
            StartCoroutine(Bump(collision.gameObject));
        }
    }

    void CheckWalk()
    {
        if (isCollisionWithCar
            || amoutCollision >= 2
            || gameObject.layer == layerBumping)
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

    protected virtual void FixedUpdate()
    {
        CheckWalk();
    }

    protected IEnumerator JumpStart()
    {
        isJump = true;
        animator.SetFloat("velocityY", 3);
        rb.velocity = new Vector2(rb.velocity.x, forceJump);
        yield return new WaitForSeconds(0.5f);
        JumpEnd();
    }

    void JumpEnd()
    {
        isJump = false;
        animator.SetFloat("velocityY", 0);
        rb.velocity = Vector2.zero;
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
        //content.SetActive(false);
        healthHandler.SetDefaultInfo(enemyInfo);
        enemyInfo.gameObject.SetActive(false);
        healthBar.SetActive(false);
        EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].ERevival(enemyInfo.gameObject);
    }

    public void OnDisable()
    {
        isCollisionWithCar = false;
        isCollisionWithGround = false;
        isStunned = false;
        isTriggerFlame = false;
        isTriggerSaw = false;
        isWalk = false;
        isJump = false;
        frontalCollision = null;
    }
    IEnumerator SetFalseIsStunned(float time)
    {
        yield return new WaitForSeconds(time);
        isStunned = false;
    }
}
