using UnityEngine;

public class TowerVisible : MonoBehaviour
{
    public EnemyTowerHandler enemyTowerHandler;

    public void OnBecameVisible()
    {
        enemyTowerHandler.isVisible = true;
        enemyTowerHandler.enemyController.col.SetActive(false);
        enemyTowerHandler.enemyController.col.SetActive(true);
    }
}
