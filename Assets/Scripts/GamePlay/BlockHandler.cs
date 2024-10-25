using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    public Block blockInfo;
    public GameObject healthBar;
    public HealthHandler healthHandler;
    public List<GameObject> listEnemies = new List<GameObject>();
    public SpriteRenderer[] fullBlocks;
    public HitEffect hitEffect;

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
    }
}
