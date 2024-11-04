using System.Collections;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    public Block blockInfo;
    public GameObject healthBar;
    public HealthHandler healthHandler;
    public SpriteRenderer[] fullBlocks;
    public HitEffect hitEffect;
    Coroutine bossTrigger;

    public void SetTotalHp()
    {
        healthHandler.SetTotalHp(blockInfo.hp);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) CarController.instance.amoutCollison++;
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) CarController.instance.amoutCollison--;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            if (bossTrigger != null) StopCoroutine(bossTrigger);
        }
    }

    public void SubtractHp(int subtractHp)
    {
        if (!healthBar.activeSelf) healthBar.SetActive(true);
        float hp = blockInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        hitEffect.PlayHitEffect(fullBlocks);

        if (hp == 0)
        {
            GameController.instance.ShakeCam(0.15f);
            BlockController.instance.DeleteBlockInGame(blockInfo.gameObject);
            ParController.instance.PlayBlockDestroyParticle(blockInfo.transform.position);
            if(blockInfo.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) Booster.instance.CheckButtonState(blockInfo.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
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
        while (blockInfo.hp > 0)
        {
            SubtractHp(subtractHp);
            yield return new WaitForSeconds(GameController.instance.timeBossDamage);
        }
    }

    public void Restart()
    {
        healthBar.gameObject.SetActive(false);
        healthHandler.SetDefaultInfo(ref blockInfo.hp);
        Color color = blockInfo.sp.color;
        color.a = 1;
        blockInfo.sp.color = color;
    }
}
