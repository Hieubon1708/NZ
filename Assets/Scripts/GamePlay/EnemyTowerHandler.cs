using System.Collections;
using UnityEngine;

public class EnemyTowerHandler : MonoBehaviour
{
    public Tower towerInfo;
    public GameObject view;
    public HealthHandler healthHandler;
    public Damage damage;
    public Transform towerPos;
    public bool isVisible;
    int damageTaken;
    Coroutine flameTrigger;
    public SpriteRenderer[] fullTowers;

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
        if (collision.CompareTag("Flame"))
        {
            subtractHp = int.Parse(collision.gameObject.name);
            flameTrigger = StartCoroutine(FlameTriggerHandle(subtractHp));
        }
        damageTaken += subtractHp;
        if (damageTaken >= 100)
        {
            UIHandler.instance.progressHandler.PlusGold(1);
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
        damage.ShowDamage(substractHp.ToString(), null, fullTowers);

        if (hp == 0)
        {
            towerInfo.gameObject.SetActive(false);
            damageTaken = 0;
            UIHandler.instance.progressHandler.PlusGold(100);
            GameController.instance.EDeathAll(EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].col);
            EnemyTowerController.instance.NextTower();
            ParController.instance.PlayTowerExplosionParticle(new Vector2(towerPos.transform.position.x + 1.5f, towerPos.transform.position.y));
        }
    }
}
