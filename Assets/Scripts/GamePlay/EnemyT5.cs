using System.Collections;
using UnityEngine;

// con nhện treo
public class EnemyT5 : EnemyHandler
{
    public float targetX;
    public GameObject silk;
    public GameObject v;
    public GameObject objScaler;
    Coroutine breakingSilk;
    public bool isDeadByTower;

    public override void Start() { }
    public override void SetDefaultField() { }

    public override void SpawnbyTime()
    {
        isDeadByTower = false;
        SetDamage();
        SetHp();
        hitObj = objScaler;
        RestartSilk();
        GameController.instance.listEVisible.Add(enemyInfo.gameObject);
        targetX = EUtils.RandomXDistanceByCar(GameController.instance.xPlus1 + 4, GameController.instance.xPlus2);
        transform.position = new Vector2(targetX, CarController.instance.spawnY[Random.Range(0, CarController.instance.spawnY.Length)].transform.position.y);
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
            PlayerController.instance.playerHandler.SubtractHp(enemyInfo.damage);
            if (ParLv2.instance != null) ParLv2.instance.PlaySpriderHitOnBlockOHeroParticle(new Vector2(transform.position.x, transform.position.y + 6));
            StopCoroutines();
            SetColNKinematicNRevival(false);
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            SetDeathAni();
            GameController.instance.listEVisible.Remove(enemyInfo.gameObject);
        }
    }

    protected override void FixedUpdate()
    {
        if (!colObj.activeSelf) return;
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

    public override void Restart()
    {
        base.Restart();
        StopCoroutines();
        RestartSilk();
        healthBar.SetActive(false);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        SetColNKinematicNRevival(false);
    }


    public override void DeathHandle()
    {
        UIHandler.instance.daily.CheckDaily(Daily.DailyType.DestroyEnemy);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        StopCoroutines();
        if (!silk.transform.IsChildOf(GameController.instance.poolDynamics))
        {
            silk.transform.SetParent(GameController.instance.poolDynamics);
            breakingSilk = StartCoroutine(BreakingSpiderSilk());
        }
        SetColNKinematicNRevival(false);
        healthBar.SetActive(false);
        if(!isDeadByTower) UIHandler.instance.FlyGold(hitObj.transform.position, 2);
        SetDeathAni();
        GameController.instance.listEVisible.Remove(enemyInfo.gameObject);
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
}
