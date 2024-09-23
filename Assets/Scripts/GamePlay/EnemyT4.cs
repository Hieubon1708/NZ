using DG.Tweening;
using System.Collections;
using UnityEngine;

// con nhện to 
public class EnemyT4 : EnemyHandler
{
    public GameObject body;
    public float timeRevive;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay")) content.SetActive(true);
        if (!content.activeSelf) yield break;
        if (collision.CompareTag("Car")) animator.SetBool("attack", true);
        StartCoroutine(base.OnTriggerEnter2D(collision));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }

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
            EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].ERevival(enemyInfo.gameObject);
        });
    }
}
