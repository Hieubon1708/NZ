using DG.Tweening;
using UnityEngine;

// con nhện to 
public class EnemyT4 : EnemyHandler
{
    public GameObject body;
    public float timeRevive;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void DeathHandle()
    {
        //content.SetActive(false);
        healthHandler.SetDefaultInfo(enemyInfo);
        healthBar.SetActive(false);
        DOVirtual.DelayedCall(timeRevive, delegate
        {
            enemyInfo.gameObject.SetActive(false);
        });
    }
}
