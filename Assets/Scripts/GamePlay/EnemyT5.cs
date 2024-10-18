using System.Collections;
using UnityEngine;

// con nhện treo
public class EnemyT5 : EnemyHandler
{
    public float targetX;
    public GameObject silk;
    public GameObject v;
    Coroutine breakingSilk;

    public override void Start()
    {
        SetDamage();
    }

    public override void SpawnbyTime()
    {
        RestartSilk();
        targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1 + 4, GameController.instance.xPlus2);
        transform.position = new Vector2(targetX, CarController.instance.spawnY[Random.Range(0, CarController.instance.spawnY.Length)].transform.position.y);
        gameObject.SetActive(true);
    }

    public override void SetDamage()
    {
        col.name = enemyInfo.damage.ToString();
        healthHandler.SetTotalHp(enemyInfo.hp);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Block") || collision.CompareTag("Player"))
        {
            PlayerController.instance.playerHandler.SubtractHp(int.Parse(col.name));
            if (ParLv2.instance != null) ParLv2.instance.PlaySpriderHitOnBlockOHeroParticle(new Vector2(transform.position.x, transform.position.y + 6));
            DeathHandle();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    protected override void FixedUpdate()
    {
        if (transform.position.x <= PlayerController.instance.transform.position.x + 2f)
        {
            if (rb.gravityScale == 0)
            {
                rb.gravityScale = 1;
                animator.SetFloat("velocityY", -3);
                silk.transform.SetParent(GameController.instance.poolDynamics);
                breakingSilk = StartCoroutine(BreakingSpiderSilk());
            }
            rb.velocity = new Vector2(Mathf.Clamp(-1.5f - BlockController.instance.tempBlocks.Count * multiplier, -2f, -1.5f) * multiplier, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-GameController.instance.backgroundSpeed * multiplier, rb.velocity.y);
        }
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
        rb.gravityScale = 0;
        animator.SetFloat("velocityY", 3);
    }

    protected override void StopCoroutines()
    {
        base.StopCoroutines();
        if (breakingSilk != null)
        {
            StopCoroutine(breakingSilk); breakingSilk = null;
        }
    }

    void RestartSilk()
    {
        silk.transform.SetParent(v.transform);
        silk.transform.localRotation = Quaternion.identity;
        silk.transform.localPosition = new Vector2(-0.06f, 5.231f);
    }

    IEnumerator BreakingSpiderSilk()
    {
        float time = 0;
        float speed = 0.8f;
        float angleTarget = 8;
        bool firstPush = false;

        yield return new WaitForSeconds(0.15f);

        while (time <= 3f)
        {
            time += Time.fixedDeltaTime;
            silk.transform.Translate(Vector2.up * time * speed);
            if (!firstPush) silk.transform.localRotation = Quaternion.Lerp(silk.transform.localRotation, Quaternion.Euler(0, 0, angleTarget), 0.1f);
            if (firstPush) silk.transform.localRotation = Quaternion.Lerp(silk.transform.localRotation, Quaternion.identity, 0.05f);
            if (silk.transform.localEulerAngles.z >= angleTarget * 0.75f) firstPush = true;
            yield return new WaitForFixedUpdate();
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
    }
}
