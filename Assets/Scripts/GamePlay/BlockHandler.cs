using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    public Block blockInfo;
    public GameObject healthBar;
    public HealthHandler healthHandler;
    public List<GameObject> listEnemies = new List<GameObject>();

    public void OnEnable()
    {
        healthHandler.SetTotalHp(blockInfo.hp);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (!listEnemies.Contains(enemy))
        {
            listEnemies.Add(enemy);
            if (!healthBar.activeSelf) healthBar.SetActive(true);
            float hp = blockInfo.SubtractHp(25);
            healthHandler.SubtractHp(hp);
            DOVirtual.DelayedCall(0.5f, delegate
            {
                listEnemies.Remove(enemy);
            });
            if (hp == 0) BlockController.instance.DeleteBlock(blockInfo.gameObject);
        }
    }
}
