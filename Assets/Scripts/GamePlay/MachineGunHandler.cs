using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunHandler : WeaponShoter
{
    public GameObject preBullet;
    public GameObject preBulletBooster;
    public int count;
    public float timeDistance;
    public float timeDistanceBooster;
    public float speedBullet;
    public float turnTimeDelay;
    public List<List<GameObject>> listBullets = new List<List<GameObject>>();
    public List<List<MachineGunBulletHandler>> listScBullets = new List<List<MachineGunBulletHandler>>();
    public List<GameObject> listBulletBoosters = new List<GameObject>();
    public List<MachineGunBulletHandler> listScBulletBoosters = new List<MachineGunBulletHandler>();
    public Transform[] containers;
    public GameObject block;
    public int amoutLine;
    Coroutine[] shots;
    Coroutine shotBoosters;
    int currentCountBoosterBullet;

    public void Awake()
    {
        Generate();
    }

    public override void StartGame()
    {
        ShotAll();
        findTarget = StartCoroutine(FindTarget());
        rotate = StartCoroutine(Rotate());
    }

    public override void Restart()
    {
        base.Restart();
        ani.SetBool("attack", false);
        if (shotBoosters != null) StopCoroutine(shotBoosters);
        StopAll();
    }

    void ShotAll()
    {
        for (int i = 0; i < listBullets.Count; i++)
        {
            shots[i] = StartCoroutine(Shot(listBullets[i], listScBullets[i], i, timeDistance));
        }
    }

    void Generate()
    {
        shots = new Coroutine[amoutLine];
        for (int j = 0; j < amoutLine; j++)
        {
            List<GameObject> listB = new List<GameObject>();
            List<MachineGunBulletHandler> listScB = new List<MachineGunBulletHandler>();
            for (int i = 0; i < count; i++)
            {
                GameObject b = Instantiate(preBullet, containers[j]);
                b.SetActive(false);
                MachineGunBulletHandler scB = b.GetComponent<MachineGunBulletHandler>();
                listScB.Add(scB);
                listB.Add(b);
            }
            listBullets.Add(listB);
            listScBullets.Add(listScB);
        }
        for (int i = 0; i < count; i++)
        {
            GameObject b = Instantiate(preBulletBooster, containers[0]);
            b.SetActive(false);
            MachineGunBulletHandler scB = b.GetComponent<MachineGunBulletHandler>();
            listBulletBoosters.Add(b);
            listScBulletBoosters.Add(scB);
        }
    }

    public void SetDefaultBullet(GameObject b, MachineGunBulletHandler sc, int index)
    {
        b.SetActive(false);
        b.transform.SetParent(containers[index]);
        sc.rb.velocity = Vector2.zero;
        b.transform.localPosition = new Vector3(0, 0, 0);
        b.transform.localRotation = Quaternion.identity;
    }

    public IEnumerator Shot(List<GameObject> listB, List<MachineGunBulletHandler> listScB, int index, float timeDistance)
    {
        while (true)
        {
            ani.SetBool("attack", true);
            for (int i = 0; i < listB.Count; i++)
            {
                SetDefaultBullet(listB[i], listScB[i], index);
                listB[i].transform.SetParent(GameController.instance.poolBullets);
                listB[i].SetActive(true);
                listScB[i].Shot(speedBullet, listB[i].transform.right);
                yield return new WaitForSeconds(timeDistance);
            }
            ani.SetBool("attack", false);
            yield return new WaitForSeconds(turnTimeDelay);
        }
    }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
        ani.SetBool("attack", false);
        if (shotBoosters != null) StopCoroutine(shotBoosters);
    }

    public IEnumerator BoosterShot(List<GameObject> listB, List<MachineGunBulletHandler> listScB, int index, float timeDistance)
    {
        while (true)
        {
            SetDefaultBullet(listB[currentCountBoosterBullet], listScB[currentCountBoosterBullet], 0);
            listB[currentCountBoosterBullet].transform.SetParent(GameController.instance.poolBullets);
            listB[currentCountBoosterBullet].SetActive(true);
            listScB[currentCountBoosterBullet].Shot(speedBullet, listB[currentCountBoosterBullet].transform.right);
            currentCountBoosterBullet++;
            if (currentCountBoosterBullet == listB.Count) currentCountBoosterBullet = 0;
            yield return new WaitForSeconds(timeDistance);
        }
    }

    void StartBooster()
    {
        StopAll();
        shotBoosters = StartCoroutine(BoosterShot(listBulletBoosters, listScBulletBoosters, 0, timeDistanceBooster));
    }

    IEnumerator EndBooster()
    {
        if (shotBoosters != null) StopCoroutine(shotBoosters);
        yield return new WaitForSeconds(1);
        ShotAll();
    }

    void StopAll()
    {
        for (int i = 0; i < amoutLine; i++)
        {
            if (shots[i] != null) StopCoroutine(shots[i]);
        }
    }

    public override void SetDamage(int damage)
    {
        for (int i = 0; i < listBullets.Count; i++)
        {
            for (int j = 0; j < listBullets[i].Count; j++)
            {
                listBullets[i][j].name = damage.ToString();
            }
        }
    }

    public override void SetDamageBooster(int damage)
    {
        for (int i = 0; i < listBulletBoosters.Count; i++)
        {
            listBulletBoosters[i].name = damage.ToString();
        }
    }
}
