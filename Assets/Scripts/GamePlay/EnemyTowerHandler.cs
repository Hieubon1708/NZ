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
    bool isTriggerFlame;
    int damageTaken;

    public void Start()
    {
        healthHandler.SetTotalHp(towerInfo.hp);
    }

    public IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (!isVisible) yield break;
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
            isTriggerFlame = true;
            while (isTriggerFlame && towerInfo.hp > 0)
            {
                SubtractHp(subtractHp);
                yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
            }
        }
        damageTaken += subtractHp;
        if(damageTaken >= 100)
        {
            UIHandler.instance.progressHandler.PlusGold(1);
            damageTaken -= 100;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!view.activeSelf) return;
        if (collision.CompareTag("Flame")) isTriggerFlame = false;
    }

    void SubtractHp(int substractHp)
    {
        float hp = towerInfo.SubstractHp(substractHp);
        healthHandler.SubtractHp(hp);
        damage.ShowDamage(substractHp.ToString());

        if (hp == 0)
        {
            damageTaken = 0;
            UIHandler.instance.progressHandler.PlusGold(100);
            GameController.instance.EDeathAll(EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].col);
            towerInfo.gameObject.SetActive(false);
            EnemyTowerController.instance.NextTower();
            ParController.instance.PlayTowerExplosionParticle(new Vector2(towerPos.transform.position.x + 1.5f, towerPos.transform.position.y));
        }
    }
}
