using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerController : MonoBehaviour
{
    public static EnemyTowerController instance;

    public GameController.WEAPON[] weaponUseds;

    public GameObject[] towers;
    public EnemyController[] scTowers;
    public BackgroundMovement[] backgroundMovements;
    public EnemyTowerMovement[] enemyTowerMovements;
    public float distanceTower;
    public int indexTower;
    public int[] lineRandoms;

    public Transform poolEnemies;

    List<int> remainingLines = new List<int>() { 0, 1, 2 };
    public GameObject[] enemies;
    public int[] amouts;

    List<GameObject> temp = new List<GameObject>();
    List<List<GameObject>> poolEs = new List<List<GameObject>>();
    List<EnemyHandler> poolScEs = new List<EnemyHandler>();
    List<EnemyHandler> poolScEByTimes = new List<EnemyHandler>();
    public List<GameObject> listRandomEs;

    public int amoutLimit;
    int milestone;
    float spawnX;

    public GameObject[] enemySpawnByTimes;
    GameObject[][] poolEnemySpawnByTimes;
    public int[] amoutEnemySpawnByTimes;
    public float[] speedEnemySpawnByTimes;
    public int[] timeSpawns;
    Coroutine[] eSpawnByTimes;

    private void Awake()
    {
        instance = this;
        GenerateTower();
        GenerateEs();
    }

    public void Start()
    {
        milestone = amoutLimit;
        eSpawnByTimes = new Coroutine[enemySpawnByTimes.Length];
        BlockController.instance.LoadWeaponBuyButtonInCurrentLevel(GameController.instance.level);
    }

    void AssignSpanwX()
    {
        spawnX = GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1;
    }

    public EnemyHandler GetScE(GameObject e)
    {
        for (int i = 0; i < poolScEs.Count; i++)
        {
            if (poolScEs[i].gameObject == e) return poolScEs[i];
        }
        if (enemySpawnByTimes != null && scTowers[indexTower].isSpawnByTime)
        {
            for (int i = 0; i < poolScEByTimes.Count; i++)
            {
                if (poolScEByTimes[i].gameObject == e) return poolScEByTimes[i];
            }
        }
        return null;
    }

    public void EnableEs()
    {
        for (int i = 0; i < listRandomEs.Count; i++)
        {
            listRandomEs[i].SetActive(true);
            EnemyHandler eSc = GetScE(listRandomEs[i]);
            eSc.view.SetActive(true);
        }
        if (enemySpawnByTimes != null && scTowers[indexTower].isSpawnByTime)
        {
            for (int i = 0; i < enemySpawnByTimes.Length; i++)
            {
                eSpawnByTimes[i] = StartCoroutine(SpawnEnemyByTime(poolEnemySpawnByTimes[i], timeSpawns[i]));
            }
        }
    }

    public void DisableEs()
    {
        for (int i = 0; i < listRandomEs.Count; i++)
        {
            UIHandler.instance.daily.CheckDaily(Daily.DailyType.DestroyEnemy);
            EnemyHandler eSc = GetScE(listRandomEs[i]);
            eSc.col.enabled = false;
            eSc.content.SetActive(false);
            listRandomEs[i].SetActive(false);
            eSc.Restart();
            eSc.col.enabled = true;
        }
        if (enemySpawnByTimes != null && scTowers[indexTower].isSpawnByTime)
        {
            for (int i = 0; i < eSpawnByTimes.Length; i++)
            {
                if (eSpawnByTimes[i] != null)
                {
                    StopCoroutine(eSpawnByTimes[i]);
                }
            }
            for (int i = 0; i < poolScEByTimes.Count; i++)
            {
                if (!poolScEByTimes[i].gameObject.activeSelf) continue;
                EnemyHandler eSc = GetScE(poolScEByTimes[i].gameObject);
                if (GameController.instance.isLose)
                {
                    eSc.Restart();
                    poolScEByTimes[i].gameObject.SetActive(false);
                }
                else
                {
                    if(eSc as EnemyT5)
                    {
                        (eSc as EnemyT5).isDeadByTower = true;
                    }
                    if(eSc as EnemyT4)
                    {
                        (eSc as EnemyT4).isDeadByTower = true;
                    }
                    eSc.DeathHandle();
                }
            }
        }
    }

    IEnumerator SpawnEnemyByTime(GameObject[] es, float timeSpawn)
    {
        int index = 0;
        while (!GameController.instance.isLose)
        {
            yield return new WaitForSeconds(timeSpawn);
            EnemyHandler eSc = GetScE(es[index]);

            if (GameController.instance.listEVisible.Contains(scTowers[indexTower].col) && eSc is EnemyT5) yield break;

            es[index].SetActive(true);
            eSc.SpawnbyTime();
            eSc.healthHandler.SetDefaultInfo(ref eSc.enemyInfo.hp);
            eSc.SetColNKinematicNRevival(true);
            eSc.animator.Rebind();
            index++;
            if (index == es.Length) index = 0;
        }
    }

    void GenerateEs()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            List<GameObject> es = new List<GameObject>();
            for (int j = 0; j < amouts[i]; j++)
            {
                GameObject e = Instantiate(enemies[i], poolEnemies);
                EnemyHandler sc = e.GetComponent<EnemyHandler>();
                sc.content.SetActive(false);
                e.SetActive(false);
                poolScEs.Add(sc);
                es.Add(e);
            }
            poolEs.Add(es);
        }
        if (enemySpawnByTimes != null)
        {
            poolEnemySpawnByTimes = new GameObject[enemySpawnByTimes.Length][];
            for (int i = 0; i < enemySpawnByTimes.Length; i++)
            {
                GameObject[] eSpawnbyTimes = new GameObject[amoutEnemySpawnByTimes[i]];
                for (int j = 0; j < eSpawnbyTimes.Length; j++)
                {
                    eSpawnbyTimes[j] = Instantiate(enemySpawnByTimes[i], poolEnemies);
                    eSpawnbyTimes[j].SetActive(false);
                    EnemyHandler sc = eSpawnbyTimes[j].GetComponent<EnemyHandler>();
                    if (sc is EnemyT4)
                    {
                        sc.realSpeed = speedEnemySpawnByTimes[i];
                        sc.speed = speedEnemySpawnByTimes[i];
                        sc.startSpeed = speedEnemySpawnByTimes[i];
                    }
                    poolScEByTimes.Add(sc);
                }
                poolEnemySpawnByTimes[i] = eSpawnbyTimes;
            }
        }
    }

    public void Restart()
    {
        DisableEs();
        if (enemySpawnByTimes != null && scTowers[indexTower].isSpawnByTime)
        {
            for (int i = 0; i < eSpawnByTimes.Length; i++)
            {
                if (eSpawnByTimes[i] != null)
                {
                    StopCoroutine(eSpawnByTimes[i]);
                }
            }
        }

        for (int i = 0; i < scTowers.Length; i++)
        {
            scTowers[i].gameObject.SetActive(true);
            scTowers[i].enemyTowerHandler.Resart();
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

    void RandomEs()
    {
        if (scTowers[indexTower].indexEs.Length == 0) return;
        float minSpeed = float.MaxValue;
        for (int i = 0; i < scTowers[indexTower].speeds.Length; i++)
        {
            if (scTowers[indexTower].speeds[i] < minSpeed) minSpeed = scTowers[indexTower].speeds[i];
        }

        List<List<GameObject>> typeEs = new List<List<GameObject>>();

        for (int i = 0; i < scTowers[indexTower].indexEs.Length; i++)
        {
            int index = scTowers[indexTower].indexEs[i];
            List<GameObject> es = new List<GameObject>();
            for (int j = 0; j < scTowers[indexTower].amoutIndexEs[i]; j++)
            {
                GameObject e = poolEs[index][j];
                EnemyHandler sc = GetScE(e);
                sc.realSpeed = scTowers[indexTower].speeds[i];
                sc.startSpeed = minSpeed;
                sc.speed = minSpeed;
                es.Add(e);
            }
            typeEs.Add(es);
        }

        int indexMaxType = int.MinValue;
        int max = int.MinValue;
        for (int i = 0; i < typeEs.Count; i++)
        {
            if (typeEs[i].Count > max)
            {
                max = typeEs[i].Count;
                indexMaxType = i;
            }
        }
        listRandomEs = typeEs[indexMaxType];

        //Debug.Log(listTypeEs.Count);
        for (int i = 0; i < typeEs.Count; i++)
        {
            if (typeEs[i] != typeEs[indexMaxType])
            {
                int distance = listRandomEs.Count / typeEs[i].Count;
                //Debug.Log("Distance " + distance);
                for (int j = 0; j < listRandomEs.Count; j += distance + 1)
                {
                    if (typeEs[i].Count > 0)
                    {
                        int indexRandom = Random.Range(j, j + distance + 1);
                        GameObject e = typeEs[i][0];
                        listRandomEs.Insert(indexRandom, e);
                        typeEs[i].Remove(e);
                    }
                    else
                    {
                        int index = j;
                        int count = listRandomEs.Count - j;
                        for (; j < listRandomEs.Count; j++)
                        {
                            temp.Add(listRandomEs[j]);
                        }
                        /*Debug.LogWarning(index);
                        Debug.LogWarning(count);*/
                        listRandomEs.RemoveRange(index, count);
                        while (temp.Count > 0)
                        {
                            int indexRandom = Random.Range(0, listRandomEs.Count);
                            listRandomEs.Insert(indexRandom, temp[0]);
                            temp.RemoveAt(0);
                        }
                    }
                }
            }
        }
    }

    void SetPosition()
    {
        int count = 0;
        int i = 0;

        remainingLines = new List<int>() { 0, 1, 2 };
        lineRandoms = new int[3];

        EnemyController eController = scTowers[indexTower];

        while (count < listRandomEs.Count)
        {
            RandomEnemy(i < eController.amoutEs.Length ? eController.amoutEs[i] : eController.amoutEs[eController.startDistanceMultipliers.Length - 1]
                , i < eController.startDistanceMultipliers.Length ? eController.startDistanceMultipliers[i] : eController.startDistanceMultipliers[eController.startDistanceMultipliers.Length - 1]
                , i < eController.endDistanceMultipliers.Length ? eController.endDistanceMultipliers[i] : eController.endDistanceMultipliers[eController.startDistanceMultipliers.Length - 1]
                , i < eController.distances.Length ? eController.distances[i] : eController.distances[eController.startDistanceMultipliers.Length - 1], ref count);
            i++;
        }
    }

    void RandomEnemy(int amout, int startDistance, int endDistance, float distance, ref int count)
    {
        while (amout > 0 && count < listRandomEs.Count)
        {
            amout--;
            CheckAmoutEnemyEachLine();
            int randomLine = remainingLines[Random.Range(0, remainingLines.Count)];
            int indexLine = randomLine + 1;
            int randomDistance = Random.Range(startDistance, endDistance);

            GameObject e = listRandomEs[count];
            EnemyHandler scE = e.GetComponent<EnemyHandler>();

            float y = CarController.instance.spawnY[randomLine].position.y;

            if (e.name.Contains("Level 2 simpleEnemy 3 fl"))
            {
                int lineIndex = Random.Range(0, 3);
                if (spawnX < scTowers[indexTower].col.transform.position.x)
                {
                    y = EUtils.RandomYDistanceByCar(3, 7f);
                    (scE as EnemyT3).isLevingCave = true;
                }
                else y = Random.Range(CarController.instance.spawnY[lineIndex].position.y + 0.5f, CarController.instance.spawnY[lineIndex].position.y + 1f);
            }
            else
            {
                lineRandoms[randomLine]++;
            }

            e.transform.position = new Vector2(spawnX, y);

            SetLayer(randomLine, e);
            SetLayer(randomLine, scE.colObj);

            scE.sortingGroup.sortingLayerName = "Line_" + indexLine;
            scE.rb.excludeLayers = (randomLine == 0 ? 0 : 1 << 9) | (randomLine == 1 ? 0 : 1 << 10) | (randomLine == 2 ? 0 : 1 << 11) | (randomLine == 0 ? 0 : 1 << 6) | (randomLine == 1 ? 0 : 1 << 7) | (randomLine == 2 ? 0 : 1 << 8) | 1 << 18;

            if (!e.name.Contains("Level 2 simpleEnemy 3 fl"))
            {
                scE.GetLayer();
                spawnX += randomDistance * distance;
            }
            count++;
        }
    }

    public void ERevival(GameObject e, EnemyHandler eSc)
    {
        int index = -1;
        float xHighest = int.MinValue;
        for (int i = 0; i < listRandomEs.Count; i++)
        {
            if (listRandomEs[i] == e) continue;
            float xE = listRandomEs[i].transform.position.x;
            if (xHighest < xE)
            {
                xHighest = xE;
                index = i;
            }
        }

        float x = index == -1 ? GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1 : xHighest + scTowers[indexTower].defaultDistance;
        float y = 0;

        if (e.name.Contains("Level 2 simpleEnemy 3 fl"))
        {
            int lineIndex = Random.Range(0, 3);
            if (x < scTowers[indexTower].col.transform.position.x)
            {
                y = EUtils.RandomYDistanceByCar(3, 7f);
                (eSc as EnemyT3).isLevingCave = true;
            }
            else y = Random.Range(CarController.instance.spawnY[lineIndex].position.y + 0.5f, CarController.instance.spawnY[lineIndex].position.y + 1f);
        }
        else
        {
            y = CarController.instance.spawnY[eSc.lineIndex - 1].position.y;
        }

        e.transform.position = new Vector2(x, y);

        eSc.SetDefaultField();
        eSc.SetColNKinematicNRevival(true);
    }

    void SetLayer(int line, GameObject enemy)
    {
        int indexLayer = -1;
        if (line == 0) indexLayer = 6;
        if (line == 1) indexLayer = 7;
        if (line == 2) indexLayer = 8;
        if (indexLayer == -1) Debug.LogError("!");
        enemy.layer = indexLayer;
    }

    void CheckAmoutEnemyEachLine()
    {
        for (int i = 0; i < remainingLines.Count; i++)
        {
            if (lineRandoms[remainingLines[i]] >= milestone)
            {
                remainingLines.RemoveAt(i);
            }
        }
        if (remainingLines.Count == 0)
        {
            remainingLines = new List<int>() { 0, 1, 2 };
            milestone += amoutLimit;
        }
    }

    void GenerateTower()
    {
        scTowers = new EnemyController[towers.Length];
        enemyTowerMovements = new EnemyTowerMovement[towers.Length];
        for (int i = 0; i < towers.Length; i++)
        {
            //Debug.LogWarning(CarController.instance.transform.localPosition.x + (distanceTower * (i + 1)));
            GameObject t = Instantiate(towers[i], new Vector2(CarController.instance.transform.localPosition.x + (distanceTower * (i + 1)), CarController.instance.transform.position.y - 0.25f), Quaternion.identity, transform);
            //Debug.Log(CarController.instance.transform.position.x - t.transform.position.x);
            scTowers[i] = t.GetComponent<EnemyController>();
            enemyTowerMovements[i] = t.GetComponentInChildren<EnemyTowerMovement>();
        }
    }

    public EnemyController GetTower()
    {
        return scTowers[indexTower];
    }

    public void NextTower()
    {
        if (indexTower == towers.Length - 1)
        {
            DisableEs();
            CarController.instance.multiplier = 0;
            GameController.instance.level++;
            if (GameController.instance.level > 1) GameController.instance.level = 0;
            GameController.instance.isStart = false;
            //UIHandler.instance.SetActiveProgressNGem(false);
            UIHandler.instance.tutorial.CheckTutorialShopNWeaponNBoss();
            UIHandler.instance.menu.CheckDisplayButtonPage();
            UIHandler.instance.daily.CheckDaily(Daily.DailyType.CompleteLevel);
            BlockController.instance.DisableWeapons();
            PlayerController.instance.DisableWeapons();
            Booster.instance.KillEnergyNBoosterButton();
            BlockController.instance.energyUpgradee.level = 0;
            UIHandler.instance.goldRewardHighest = 500;
            UIHandler.instance.UpdateTextRewardHoldHighest();
            BlockController.instance.energyUpgradee.UpgradeHandle();
            GameController.instance.isLose = true;
            BlockController.instance.SellAllBlocks();
            BlockController.instance.ClearBlocks();
            UIHandler.instance.menu.CheckNotifAll();
            UIHandler.instance.progressHandler.StopProgress();
            EquipmentController.instance.playerInventory.ConvertGoldToDush();
            BlockController.instance.CheckButtonStateAll();
            DOVirtual.DelayedCall(2f, delegate
            {
                UIHandler.instance.progressHandler.ShowReward();
            });
        }
        else
        {
            indexTower++;
            CarController.instance.multiplier = 1;
            AssignSpanwX();
            RandomEs();
            SetPosition();
            EnableEs();
            UIHandler.instance.progressHandler.StartLauchProgress();
        }
    }
}
