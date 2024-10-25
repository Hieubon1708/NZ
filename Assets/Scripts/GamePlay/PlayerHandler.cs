using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHandler : MonoBehaviour
{
    public Player playerInfo;
    public HealthHandler healthHandler;
    public GameObject healthBar;
    public GameObject boxCollider;
    public List<GameObject> listEnemies = new List<GameObject>();
    public SpriteRenderer[] fullBodies;
    public HitEffect hitEffect;

    public void LoadData()
    {
        healthHandler.SetTotalHp(playerInfo.hp);
    }

    public void SubtractHp(int subtractHp)
    {
        if (playerInfo.hp == 0) return;
        float hp = playerInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        hitEffect.PlayHitEffect(fullBodies);

        if (hp == 0)
        {
            GameController.instance.isLose = true;
            GameController.instance.touchScreen.SetActive(false);
            Booster.instance.KillEnergyNBoosterButton();
            PlayerController.instance.DeathAni();
            CarController.instance.DeathAni();
            ParController.instance.PlayPlayerDieParticle(PlayerController.instance.transform.position);
            CarController.instance.multiplier = 0;
            UIHandler.instance.progressHandler.ChestReward(EnemyTowerController.instance.indexTower);
            BlockController.instance.DisableWeapons();
            boxCollider.SetActive(false);
            healthBar.SetActive(false);
            GameController.instance.ShakeCam(0.25f);
            DOVirtual.DelayedCall(1.5f, delegate
            {
                if(UIHandler.instance.tutorial.isFirstTimePlay) UIHandler.instance.progressHandler.ShowLose();
                else
                {
                    UIHandler.instance.progressHandler.HideLose(); 
                    UIHandler.instance.tutorial.isFirstTimePlay = true;
                }
            });
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerInfo.hp == 0) return;
        if (collision.CompareTag("EnemyBullet"))
        {
            SubtractHp(int.Parse(collision.gameObject.name));
        }
    }
}
