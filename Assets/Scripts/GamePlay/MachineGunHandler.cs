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
    public List<List<GameObject>> listBulletBoosters = new List<List<GameObject>>();
    public List<List<MachineGunBulletHandler>> listScBullets = new List<List<MachineGunBulletHandler>>();
    public List<List<MachineGunBulletHandler>> listScBulletBoosters = new List<List<MachineGunBulletHandler>>();
    public Transform[] containers;
    public GameObject block;
    public int amoutLine;
    Coroutine[] shots;
    Coroutine[] shotBoosters;
    Coroutine booster;

    public void Awake()
    {
        Generate();
    }

    public override void StartGame()
    {
        SetDamage();
        ShotAll();
        StartCoroutine(FindTarget());
        StartCoroutine(Rotate());
    }

    void ShotAll()
    {
        for (int i = 0; i < listBullets.Count; i++)
        {
            shots[i] = StartCoroutine(Shot(listBullets[i], listScBullets[i], i, false, timeDistance));
        }
    }

    void Generate()
    {
        shots = new Coroutine[amoutLine];
        shotBoosters = new Coroutine[amoutLine];
        for (int j = 0; j < amoutLine; j++)
        {
            List<GameObject> listB = new List<GameObject>();
            List<GameObject> listBt = new List<GameObject>();
            List<MachineGunBulletHandler> listScB = new List<MachineGunBulletHandler>();
            List<MachineGunBulletHandler> listScBt = new List<MachineGunBulletHandler>();
            for (int i = 0; i < count; i++)
            {
                GameObject b = Instantiate(preBullet, containers[j]);
                GameObject bt = Instantiate(preBulletBooster, containers[j]);
                b.SetActive(false);
                bt.SetActive(false);
                MachineGunBulletHandler scB = b.GetComponent<MachineGunBulletHandler>();
                MachineGunBulletHandler scBt = bt.GetComponent<MachineGunBulletHandler>();
                listScB.Add(scB);
                listScBt.Add(scBt);
                listB.Add(b);
                listBt.Add(bt);
            }
            listBullets.Add(listB);
            listBulletBoosters.Add(listBt);
            listScBullets.Add(listScB);
            listScBulletBoosters.Add(listScBt);
        }
    }

    void SetDamage()
    {
        for (int i = 0; i < listBullets.Count; i++)
        {
            for (int j = 0; j < listBullets[i].Count; j++)
            {
                listBullets[i][j].name = GameController.instance.listDamages[block].ToString();
                listBulletBoosters[i][j].name = GameController.instance.listDamages[block].ToString();
            }
        }
    }

    public void SetDefaultBullets(List<GameObject> listB, List<MachineGunBulletHandler> listScB, int index)
    {
        for (int i = 0; i < listB.Count; i++)
        {
            listB[i].SetActive(false);
            listB[i].transform.SetParent(containers[index]);
            listScB[i].rb.velocity = Vector2.zero;
            listB[i].transform.localPosition = new Vector3(0, 0, 0);
            listB[i].transform.localRotation = Quaternion.identity;
        }
    }

    public IEnumerator Shot(List<GameObject> listB, List<MachineGunBulletHandler> listScB, int index, bool isBooster, float timeDistance)
    {
        while (true)
        {
            if(!isBooster) ani.SetBool("attack", true);
            for (int i = 0; i < listB.Count; i++)
            {
                listB[i].transform.SetParent(GameController.instance.poolBullets);
                listB[i].SetActive(true);
                listScB[i].Shot(speedBullet, listB[i].transform.right);
                yield return new WaitForSeconds(timeDistance);
            }
            if (!isBooster) ani.SetBool("attack", false);
            yield return new WaitForSeconds(turnTimeDelay);
            SetDefaultBullets(listB, listScB, index);
        }
    }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
        ani.SetBool("attack", false);
        if (booster != null) StopCoroutine(booster);
        booster = StartCoroutine(Booster());
    }

    IEnumerator Booster()
    {
        StopAll();
        for (int i = 0; i < listBulletBoosters.Count; i++)
        {
            shotBoosters[i] = StartCoroutine(Shot(listBulletBoosters[i], listScBulletBoosters[i], i, true, timeDistanceBooster));
        }
        yield return new WaitForSeconds(2.85f);
        for (int i = 0; i < amoutLine; i++)
        {
            if (shotBoosters[i] != null) StopCoroutine(shotBoosters[i]);
        }
        yield return new WaitForSeconds(0.5f);
        ShotAll();
    }

    void StopAll()
    {
        for (int i = 0; i < amoutLine; i++)
        {
            if (shots[i] != null) StopCoroutine(shots[i]);
            if (shotBoosters[i] != null) StopCoroutine(shotBoosters[i]);
            SetDefaultBullets(listBulletBoosters[i], listScBulletBoosters[i], i);
            SetDefaultBullets(listBullets[i], listScBullets[i], i);
        }
    }
}
