using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;

    public Player playerInfo;
    public HealthHandler healthHandler;
    public GameObject healthBar;
    public BoxCollider2D boxCollider;
    public List<GameObject> listEnemies = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void LoadData()
    {
        playerInfo.hp = DataManager.instance.playerConfig.hp;
        playerInfo.gold = DataManager.instance.dataStorage.pLayerDataStorage != null ? DataManager.instance.dataStorage.pLayerDataStorage.gold : 0;
        healthHandler.SetTotalHp(playerInfo.hp);
    }

    public void SubtractHp(int subtractHp)
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
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("Enemy"))
        {
            SubtractHp(int.Parse(collision.gameObject.name));
        }
    }
}
