using DG.Tweening;
using UnityEngine;

// con nhện to 
public class EnemyT4 : EnemyHandler
{
    public GameObject body;
    public float timeRevive;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void DeathHandle()
    {
        healthHandler.SetDefaultInfo(enemyInfo);
        healthBar.SetActive(false);
        DOVirtual.DelayedCall(timeRevive, delegate
        {
            enemyInfo.gameObject.SetActive(false);
        });
    }

    public override void Start()
    {
        SetDamage();
    }

    public override void SetDamage()
    {
        base.SetDamage();
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

    public override void SpawnbyTime()
    {
        float x = GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1;
        float y = CarController.instance.spawnY[0].transform.position.y;
        transform.position = new Vector2(x, y);
        gameObject.SetActive(true);
    }

    protected override void StopCoroutines()
    {
        base.StopCoroutines();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
    }
}
