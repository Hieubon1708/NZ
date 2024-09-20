using System.Collections;
using UnityEngine;

public class EnemyTowerHandler : MonoBehaviour
{
    public Tower towerInfo;
    public HealthHandler healthHandler;
    public Damage damage;
    public bool isVisible;
    bool isTriggerFlame;

    public void OnEnable()
    {
        healthHandler.SetTotalHp(towerInfo.hp);
    }

    public IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (!isVisible) yield break;
        if (towerInfo.hp == 0) yield break;
        int substractHp;
        if (collision.CompareTag("Bullet"))
        {
            substractHp = 70;
            collision.gameObject.SetActive(false);
            SubstractHp(substractHp);
        }
        if (collision.CompareTag("MachineGun"))
        {
            substractHp = 44;
            collision.gameObject.SetActive(false);
            SubstractHp(substractHp);
        }
        if (collision.CompareTag("Flame"))
        {
            substractHp = 9;
            isTriggerFlame = true;
            while (isTriggerFlame && towerInfo.hp > 0)
            {
                SubstractHp(substractHp);
                yield return new WaitForSeconds(GameController.instance.timeFlameDamage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Flame")) isTriggerFlame = false;
    }

    void SubstractHp(float substractHp)
    {
        float hp = towerInfo.SubstractHp(substractHp);
        healthHandler.SubtractHp(hp);
        damage.ShowDamage(substractHp.ToString());

        if (hp == 0)
        {
            EnemyTowerController.instance.NextTower();
            towerInfo.gameObject.SetActive(false);
        }
    }
}
