using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunHandler : WeaponShoter
{
    public GameObject preBullet;
    public int count;
    public float timeDistance;
    public float speedBullet;
    public float turnTimeDelay;
    public List<List<GameObject>> listBullets = new List<List<GameObject>>();
    public List<List<MachineGunBulletHandler>> listScBullets = new List<List<MachineGunBulletHandler>>();
    public Transform[] containers;
    public GameObject block;
    public int amoutLine;

    public void Awake()
    {
        Generate();
    }

    public override void StartGame()
    {
        SetDamage();
        for (int i = 0; i < listBullets.Count; i++)
        {
            StartCoroutine(Shot(listBullets[i], listScBullets[i], i));
        }
        StartCoroutine(FindTarget());
        StartCoroutine(Rotate());
    }

    void Generate()
    {
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
    }

    void SetDamage()
    {
        for (int i = 0; i < listBullets.Count; i++)
        {
            for (int j = 0; j < listBullets[i].Count; j++)
            {
                listBullets[i][j].name = GameController.instance.listDamages[block].ToString();
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

    public IEnumerator Shot(List<GameObject> listB, List<MachineGunBulletHandler> listScB, int index)
    {
        while (true)
        {
            for (int i = 0; i < listB.Count; i++)
            {
                listB[i].transform.SetParent(GameController.instance.poolBullets);
                listB[i].SetActive(true);
                listScB[i].Shot(speedBullet, listB[i].transform.right);
                yield return new WaitForSeconds(timeDistance);
            }
            yield return new WaitForSeconds(turnTimeDelay);
            SetDefaultBullets(listB, listScB, index);
        }
    }
}
