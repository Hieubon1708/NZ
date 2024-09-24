using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class BlockHandler : MonoBehaviour
{
    public Block blockInfo;
    public GameObject healthBar;
    public HealthHandler healthHandler;
    public List<GameObject> listEnemies = new List<GameObject>();

    public void OnEnable()
    {
        blockInfo.hp = DataManager.instance.blockData.hps[blockInfo.level];
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

    public void OnCollisionStay2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (!listEnemies.Contains(enemy))
        {
            listEnemies.Add(enemy);
            if (!healthBar.activeSelf) healthBar.SetActive(true);
            float hp = blockInfo.SubtractHp(int.Parse(collision.gameObject.name));
            healthHandler.SubtractHp(hp);
            DOVirtual.DelayedCall(0.5f, delegate
            {
                listEnemies.Remove(enemy);
            });
            if (hp == 0)
            {
                BlockController.instance.DeleteBlockInGame(blockInfo.gameObject);
                ParController.instance.PlayBlockDestroyParticle(blockInfo.transform.position);
            }
        }
    }
}
