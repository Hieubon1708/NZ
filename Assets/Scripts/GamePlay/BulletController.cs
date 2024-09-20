using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public static BulletController instance;

    public GameObject preBullet;
    public GameObject[] listBullets;
    public BulletHandler[] listScBullets;
    public int[] listTimes;
    public Transform container;
    public int count;
    public float speedBullet;

    private void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        listBullets = new GameObject[count];
        listScBullets = new BulletHandler[count];
        listTimes = new int[count];
        for (int i = 0; i < count; i++)
        {
            listTimes[i] = i;
            listBullets[i] = Instantiate(preBullet, container);
            listBullets[i].SetActive(false);
            listScBullets[i] = listBullets[i].GetComponent<BulletHandler>();
        }
    }

    public void SetDefaultBullets()
    {
        for (int i = 0; i < listBullets.Length; i++)
        {
            listBullets[i].SetActive(false);
            listBullets[i].transform.SetParent(container);
            listScBullets[i].rb.velocity = Vector2.zero;
            listScBullets[i].trailRenderer.Clear();
            listBullets[i].transform.localPosition = new Vector3(0, 0, 0);
            listBullets[i].transform.localRotation = Quaternion.identity;
        }
    }

    public void SetUpShot()
    {
        int start = listBullets.Length / 2;
        List<int> times = new List<int>(listTimes.ToList());
        for (int i = 0; i < listBullets.Length; i++)
        {
            float startAngle = (i - start) * 2f;
            float endAngle = startAngle;
            if (startAngle < 0) endAngle += 2f;
            if (startAngle > 0) endAngle -= 2f;

            float randomAngle = startAngle;
            if (randomAngle != 0) randomAngle = Random.Range(startAngle, endAngle);

            StartCoroutine(Shot(RandomTime(times), i, randomAngle));
        }
    }

    float RandomTime(List<int> times)
    {
        if (times.Count > 0)
        {
            int indexRandom = Random.Range(0, times.Count);
            float time = times[indexRandom] * 0.01f;
            times.RemoveAt(indexRandom);
            return time;
        }
        else
        {
            Debug.LogError("!");
        }
        return 0;
    }

    IEnumerator Shot(float time, int index, float angle)
    {
        listBullets[index].transform.localRotation = Quaternion.Euler(0, 0, listBullets[index].transform.localRotation.z + angle);
        Vector2 dir = listBullets[index].transform.right;
        listBullets[index].transform.SetParent(GameController.instance.poolBullets);
        yield return new WaitForSeconds(time);
        listBullets[index].SetActive(true);
        listScBullets[index].Shot(speedBullet, dir);
    }
}
