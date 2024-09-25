using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BlockHandler : MonoBehaviour
{
    public Block blockInfo;
    public GameObject healthBar;
    public HealthHandler healthHandler;
    public List<GameObject> listEnemies = new List<GameObject>();

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
        if (!healthBar.activeSelf) healthBar.SetActive(true);
        float hp = blockInfo.SubtractHp(subtractHp);
        healthHandler.SubtractHp(hp);
        if (hp == 0)
        {
            BlockController.instance.DeleteBlockInGame(blockInfo.gameObject);
            ParController.instance.PlayBlockDestroyParticle(blockInfo.transform.position);
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
