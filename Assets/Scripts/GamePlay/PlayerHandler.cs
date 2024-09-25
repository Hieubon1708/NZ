using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public Player playerInfo;
    public HealthHandler healthHandler;
    public GameObject healthBar;
    public BoxCollider2D boxCollider;
    public List<GameObject> listEnemies = new List<GameObject>();


    public void OnEnable()
    {
        playerInfo.hp = DataManager.instance.playerData.playerHp;
        healthHandler.SetTotalHp(playerInfo.hp);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (!listEnemies.Contains(enemy))
        {
            listEnemies.Add(enemy);
            SubtractHp(int.Parse(collision.gameObject.name));
            DOVirtual.DelayedCall(0.5f, delegate
            {
                listEnemies.Remove(enemy);
            });
        }
    }

    void SubtractHp(int subtractHp)
    {
        float hp = playerInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        if (hp == 0)
        {
            PlayerController.instance.DeathAni();
            CarController.instance.DeathAni();
            ParController.instance.PlayPlayerDieParticle(PlayerController.instance.transform.position);
            CarController.instance.multiplier = 0;
            boxCollider.enabled = false;
            healthBar.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            SubtractHp(int.Parse(collision.gameObject.name));
        }
    }
}
