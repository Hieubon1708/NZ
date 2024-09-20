using UnityEngine;

public class EnemyTowerController : MonoBehaviour
{
    public static EnemyTowerController instance;

    public GameObject[] towers;
    public EnemyController[] scTowers;
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
        for (int i = 0; i < towers.Length; i++)
        {
            scTowers[i] = Instantiate(towers[i], new Vector2(GameController.instance.carController.transform.position.x + (distanceTower * (i + 1)), GameController.instance.carController.transform.position.y - 0.15f), Quaternion.identity, transform).GetComponent<EnemyController>();
        }
    }

    public void NextTower()
    {
        indexTower++;
        if(indexTower == towers.Length)
        {
            Debug.Log("Win");
            return;
        }
        CarController.instance.multiplier = 1;
        scTowers[indexTower].EnableEs();
    }
}
