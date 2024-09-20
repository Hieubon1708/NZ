using UnityEngine;

public class TowerVisible : MonoBehaviour
{
    public EnemyTowerHandler enemyTowerHandler;

    public void OnBecameVisible()
    {
        enemyTowerHandler.isVisible = true;
    }
}
