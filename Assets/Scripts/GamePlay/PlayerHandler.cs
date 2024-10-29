using UnityEngine;
using DG.Tweening;

public class PlayerHandler : MonoBehaviour
{
    public Player playerInfo;
    public HealthHandler healthHandler;
    public GameObject healthBar;
    public GameObject boxCollider;
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
            GameController.instance.EndGame();
            Booster.instance.KillEnergyNBoosterButton();
            PlayerController.instance.DeathAni();
            CarController.instance.DeathAni();
            ParController.instance.PlayPlayerDieParticle(PlayerController.instance.transform.position);
            CarController.instance.multiplier = 0;
            BlockController.instance.DisableWeapons();
            boxCollider.SetActive(false);
            healthBar.SetActive(false);
            UIHandler.instance.EndGame();
            if (EnemyTowerController.instance.indexTower == 1)
            {
                if (!UIHandler.instance.tutorial.isSecondTimeDestroyTower) UIHandler.instance.tutorial.isSecondTimeDestroyTower = true;
            }
            DOVirtual.DelayedCall(1.75f, delegate
            {
                if (UIHandler.instance.tutorial.isFirstTimeDestroyTower) UIHandler.instance.progressHandler.ShowLose();
                else UIHandler.instance.progressHandler.HideLose();
            });
        }
    }

    public void Resart()
    {
        healthHandler.SetDefaultInfo(ref playerInfo.hp);
        healthBar.SetActive(true);
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
