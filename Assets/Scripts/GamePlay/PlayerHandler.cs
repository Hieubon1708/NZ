using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public Player playerInfo;
    public HealthHandler healthHandler;
    public List<GameObject> listEnemies = new List<GameObject>();


    public void OnEnable()
    {
        healthHandler.SetTotalHp(playerInfo.hp);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        GameObject enemy = collision.gameObject;
        if (!listEnemies.Contains(enemy))
        {
            listEnemies.Add(enemy);
            float hp = playerInfo.SubtractHp(25);
            healthHandler.SubtractHp(hp);
            DOVirtual.DelayedCall(0.5f, delegate
            {
                listEnemies.Remove(enemy);
            });
        }
    }
}
