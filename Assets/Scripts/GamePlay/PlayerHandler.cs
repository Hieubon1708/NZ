using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerHandler : MonoBehaviour
{
    public Player playerInfo;
    public HealthHandler healthHandler;
    public GameObject healthBar;
    public GameObject boxCollider;
    public SpriteRenderer[] fullBodies;
    public HitEffect hitEffect;
    Coroutine bossTrigger;

    public void LoadData()
    {
        healthHandler.SetTotalHp(playerInfo.hp);
    }

    public void SubtractHp(int subtractHp)
    {
        if (playerInfo.hp == 0) return;
        if(!healthBar.activeSelf) healthBar.SetActive(true);
        float hp = playerInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        hitEffect.PlayHitEffect(fullBodies);

        if (hp == 0)
        {
            GameController.instance.isStart = false;
            GameController.instance.isLose = true;
            if (GameController.instance.isPLayBoss)
            {
                UIBoss.instance.bossHandler.End();
            }
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
            DOVirtual.DelayedCall(1.75f, delegate
            {
                if (UIHandler.instance.tutorial.isFirstTimeDestroyTower) UIHandler.instance.progressHandler.ShowLose();
                else UIHandler.instance.progressHandler.HideLose();
            });
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            if (bossTrigger != null) StopCoroutine(bossTrigger);
        }
    }

    public void Resart()
    {
        healthHandler.SetDefaultInfo(ref playerInfo.hp);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerInfo.hp == 0) return;
        if (collision.CompareTag("EnemyBullet"))
        {
            SubtractHp(int.Parse(collision.gameObject.name));
        }
        if (collision.CompareTag("Boss"))
        {
            bossTrigger = StartCoroutine(BossTriggerHandle(DataManager.instance.bossConfig.damage[UIBoss.instance.level]));
        }
    }

    IEnumerator BossTriggerHandle(int subtractHp)
    {
        while (playerInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeBossDamage);
        }
    }
}
