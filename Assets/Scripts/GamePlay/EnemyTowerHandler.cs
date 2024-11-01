using System.Collections;
using UnityEngine;

public class EnemyTowerHandler : MonoBehaviour
{
    public Tower towerInfo;
    public GameObject view;
    public HealthHandler healthHandler;
    public Damage damage;
    public EnemyController enemyController;
    public bool isVisible;
    int damageTaken;
    Coroutine flameTrigger;
    public SpriteRenderer[] fullTowers;
    public HitEffect hitEffect;

    public void Start()
    {
        healthHandler.SetTotalHp(towerInfo.hp);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isVisible) return;
        if (towerInfo.hp == 0) return;
        int subtractHp = 0;
        if (collision.CompareTag("Bullet"))
        {
            subtractHp = int.Parse(collision.gameObject.name);
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
        if (collision.CompareTag("ShockerBooster"))
        {
            subtractHp = int.Parse(collision.attachedRigidbody.name);
            SubtractHp(subtractHp);
        }
        if (collision.CompareTag("Flame"))
        {
            subtractHp = int.Parse(collision.gameObject.name.Substring(0, collision.name.Length - 1));
            flameTrigger = StartCoroutine(FlameTriggerHandle(subtractHp));
        }
        damageTaken += subtractHp;
        if (damageTaken >= 100)
        {
            UIHandler.instance.FlyGold(enemyController.col.transform.position, 1);
            damageTaken -= 100;
        }
    }

    IEnumerator FlameTriggerHandle(int subtractHp)
    {
        while (towerInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!view.activeSelf || towerInfo.hp == 0) return;
        if (collision.CompareTag("Flame"))
        {
            if (flameTrigger != null) StopCoroutine(flameTrigger);
        }
    }

    void SubtractHp(int substractHp)
    {
        if (towerInfo.hp == 0) return;
        float hp = towerInfo.SubstractHp(substractHp);
        healthHandler.SubtractHp(hp);
        damage.ShowDamage(substractHp, null, false);
        hitEffect.PlayHitEffect(fullTowers);

        if (hp == 0)
        {
            UIHandler.instance.daily.CheckDaily(Daily.DailyType.DestroyTower);
            damageTaken = 0;
            UIHandler.instance.FlyGold(enemyController.col.transform.position, 100);
            GameController.instance.EDeathAll(enemyController.col);
            GameController.instance.ShakeCam(0.35f);
            towerInfo.gameObject.SetActive(false);
            if (!UIHandler.instance.tutorial.isFirstTimeDestroyTower) UIHandler.instance.tutorial.isFirstTimeDestroyTower = true;
            if (EnemyTowerController.instance.indexTower > 1 && !UIHandler.instance.tutorial.isSecondTimeDestroyTower) UIHandler.instance.tutorial.isSecondTimeDestroyTower = true;
            UIHandler.instance.progressHandler.ChestReward(EnemyTowerController.instance.indexTower);
            EnemyTowerController.instance.NextTower();
            ParController.instance.PlayTowerExplosionParticle(new Vector2(view.transform.position.x + 1.5f, view.transform.position.y));
        }
    }
}
