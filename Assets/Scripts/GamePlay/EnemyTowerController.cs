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
            GameObject t = Instantiate(towers[i], new Vector2(CarController.instance.transform.position.x + (distanceTower * (i + 1)), CarController.instance.transform.position.y - 0.25f), Quaternion.identity, transform);
            scTowers[i] = t.GetComponent<EnemyController>();
            enemyTowerMovements[i] = t.GetComponentInChildren<EnemyTowerMovement>();
        }
    }

    public EnemyHandler GetScEInTower(GameObject e)
    {
        return scTowers[indexTower].GetScE(e);
    }

    public EnemyController GetTower()
    {
        return scTowers[indexTower];
    }

    public void NextTower()
    {
        if (indexTower == towers.Length - 1)
        {
            Debug.Log("Win");
            CarController.instance.multiplier = 0;
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
