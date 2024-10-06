using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    List<int> remainingLines = new List<int>() { 0, 1, 2 };
    public Transform[] enemyPools;
    public int amoutLimit;
    int milestone;
    float spawnX;
    public GameObject col;

    public GameObject[] enemies;
    public int[] amout;
    public float[] speed;
    public List<List<GameObject>> listTypeEs = new List<List<GameObject>>();
    public List<EnemyHandler> listScRandomEs = new List<EnemyHandler>();
    public List<GameObject> listRandomEs;
    public float defaultDistance;

    public int[] amoutEs;
    public float[] distances;
    public int[] startDistanceMultipliers;
    public int[] endDistanceMultipliers;

    public void Awake()
    {
        Generate();
    }

    public void Start()
    {
        milestone = amoutLimit;
        RandomEs();
        AssignSpanwX();
        SetPosition();
    }

    void AssignSpanwX()
    {
        spawnX = GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1;
    }

    public EnemyHandler GetScE(GameObject e)
    {
        int indexOf = listRandomEs.IndexOf(e);
        return listScRandomEs[indexOf];
    }

    public void EnableEs()
    {
        if (listRandomEs.Count == 0) return;
        for (int i = 0; i < listRandomEs.Count; i++)
        {
            if (!listRandomEs[i].activeSelf) listRandomEs[i].SetActive(true);
            if (!listScRandomEs[i].view.activeSelf) listScRandomEs[i].view.SetActive(true);
        }
    }

    void DisableEs()
    {
        for (int i = 0; i < listRandomEs.Count; i++)
        {
            if (listScRandomEs[i].content.activeSelf) listScRandomEs[i].content.SetActive(false);
            if (listRandomEs[i].activeSelf) listRandomEs[i].SetActive(false);
        }
    }

    void Generate()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            List<GameObject> listEs = new List<GameObject>();
            for (int j = 0; j < amout[i]; j++)
            {
                GameObject e = Instantiate(enemies[i], GameController.instance.poolEnemies);
                EnemyHandler sc = e.GetComponent<EnemyHandler>();
                sc.speed = speed[i];
                sc.content.SetActive(false);
                e.SetActive(false);
                listEs.Add(e);
            }
            listTypeEs.Add(listEs);
        }
    }

    public List<GameObject> temp = new List<GameObject>();

    public void Restart()
    {
        DisableEs();
        remainingLines = new List<int>() { 0, 1, 2 };
        AssignSpanwX();
        SetPosition();
    }

    void RandomEs()
    {
        int indexMaxType = int.MinValue;
        int max = int.MinValue;
        for (int i = 0; i < listTypeEs.Count; i++)
        {
            if (listTypeEs[i].Count > max)
            {
                max = listTypeEs[i].Count;
                indexMaxType = i;
            }
        }
        listRandomEs = listTypeEs[indexMaxType];

        //Debug.Log(listTypeEs.Count);
        for (int i = 0; i < listTypeEs.Count; i++)
        {
            if (listTypeEs[i] != listTypeEs[indexMaxType])
            {
                int distance = listRandomEs.Count / listTypeEs[i].Count;
                //Debug.Log("Distance " + distance);
                for (int j = 0; j < listRandomEs.Count; j += distance + 1)
                {
                    if (listTypeEs[i].Count > 0)
                    {
                        int indexRandom = Random.Range(j, j + distance + 1);
                        GameObject e = listTypeEs[i][0];
                        listRandomEs.Insert(indexRandom, e);
                        listTypeEs[i].Remove(e);
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
        while (count < listRandomEs.Count)
        {
            RandomEnemy(i < amoutEs.Length ? amoutEs[i] : amoutEs[startDistanceMultipliers.Length - 1]
                , i < startDistanceMultipliers.Length ? startDistanceMultipliers[i] : startDistanceMultipliers[startDistanceMultipliers.Length - 1]
                , i < endDistanceMultipliers.Length ? endDistanceMultipliers[i] : endDistanceMultipliers[startDistanceMultipliers.Length - 1]
                , i < distances.Length ? distances[i] : distances[startDistanceMultipliers.Length - 1], ref count);
            i++;
        }
    }

    void RandomEnemy(int amout, int startDistance, int endDistance, float distance, ref int count)
    {
        while (amout > 0 && count < listRandomEs.Count)
        {
            amout--;
            CheckAmoutEnemyEachLine();
            int randomLine = 1;// remainingLines[Random.Range(0, remainingLines.Count)];
            int indexLine = randomLine + 1;
            int randomDistance = Random.Range(startDistance, endDistance);

            GameObject e = listRandomEs[count];
            EnemyHandler scE = e.GetComponent<EnemyHandler>();

            if (!listScRandomEs.Contains(scE)) listScRandomEs.Add(scE);

            e.transform.SetParent(enemyPools[randomLine]);

            float y = CarController.instance.spawnY[randomLine].position.y;

            if (e.name.Contains("Level 2 simpleEnemy 3 fl"))
            {
                if (spawnX < col.transform.position.x) y = EUtils.RandomYDistanceByCar(3, 7f);
                else y = Random.Range(CarController.instance.spawnY[randomLine].position.y + 0.5f, CarController.instance.spawnY[randomLine].position.y + 1f);
            }
            if (e.name.Contains("Level 2 simpleEnemy 5 el"))
            {
                randomLine = 0;
                indexLine = randomLine;
            }

            e.transform.position = new Vector2(spawnX, y);

            SetLayer(randomLine, e);
            SetLayer(randomLine, scE.colObj);

            scE.sortingGroup.sortingLayerName = "Line_" + indexLine;
            scE.rb.excludeLayers = (randomLine == 0 ? 0 : 1 << 9) | (randomLine == 1 ? 0 : 1 << 10) | (randomLine == 2 ? 0 : 1 << 11) | (randomLine == 0 ? 0 : 1 << 6) | (randomLine == 1 ? 0 : 1 << 7) | (randomLine == 2 ? 0 : 1 << 8);

            spawnX += randomDistance * distance;
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

        int indexLine = EUtils.GetIndexLine(e);

        float x = xHighest < col.transform.position.x ? GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1 : xHighest + defaultDistance;
        float y = CarController.instance.spawnY[indexLine - 1].position.y;

        if (e.name.Contains("Level 2 simpleEnemy 3 fl"))
        {
            y = Random.Range(CarController.instance.spawnY[indexLine - 1].position.y + 0.5f, CarController.instance.spawnY[indexLine - 1].position.y + 1f);
        }

        e.transform.position = new Vector2(x, y);

        eSc.SetColNKinematicNRevival(true);
        eSc.SetActiveContentNView(true);
        eSc.ResetBone();
    }

    void CheckAmoutEnemyEachLine()
    {
        for (int i = 0; i < remainingLines.Count; i++)
        {
            if (enemyPools[remainingLines[i]].childCount >= milestone)
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

    void SetLayer(int line, GameObject enemy)
    {
        int indexLayer = -1;
        if (line == 0) indexLayer = 6;
        if (line == 1) indexLayer = 7;
        if (line == 2) indexLayer = 8;
        if (indexLayer == -1) Debug.LogError("!");
        enemy.layer = indexLayer;
    }
}
