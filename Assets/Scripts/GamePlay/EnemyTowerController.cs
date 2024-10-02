using UnityEngine;

public class EnemyTowerController : MonoBehaviour
{
    public static EnemyTowerController instance;

    public GameObject[] towers;
    public EnemyController[] scTowers;
    public BackgroundMovement[] backgroundMovements;
    public EnemyTowerMovement[] enemyTowerMovements;
    public float distanceTower;
    public int indexTower = -1;

    private void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        scTowers = new EnemyController[towers.Length];
        enemyTowerMovements = new EnemyTowerMovement[towers.Length];
        for (int i = 0; i < towers.Length; i++)
        {
            GameObject t = Instantiate(towers[i], new Vector2(GameController.instance.carController.transform.position.x + (distanceTower * (i + 1)), GameController.instance.carController.transform.position.y - 0.25f), Quaternion.identity, transform);
            scTowers[i] = t.GetComponent<EnemyController>();
            enemyTowerMovements[i] = t.GetComponentInChildren<EnemyTowerMovement>();
        }
    }

    public void NextTower()
    {
        if (indexTower == towers.Length)
        {
            Debug.Log("Win");
            return;
        }
        indexTower++;
        CarController.instance.multiplier = 1;
        scTowers[indexTower].EnableEs();
    }

    public void Restart()
    {
        for (int i = indexTower; i >= 0; i--)
        {
            if (!towers[i].activeSelf) towers[i].SetActive(true);
            scTowers[i].Restart();
        }
        indexTower = -1;
        for (int i = 0; i < backgroundMovements.Length; i++)
        {
            backgroundMovements[i].Restart();
        }
        for (int i = 0; i < enemyTowerMovements.Length; i++)
        {
            enemyTowerMovements[i].Restart();
        }
    }
}
