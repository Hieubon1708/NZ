using System;
using System.Collections;
using UnityEngine;

public class MachineGunHandler : MonoBehaviour
{
    public Animator ani;

    public GameObject preBullet;
    public int count;
    public float timeDistance;
    public float speedBullet;
    public float turnTimeDelay;
    public GameObject[] listBullets;
    public MachineGunBulletHandler[] listScBullets;
    public Transform container;

    public void Awake()
    {
        Generate();
    }

    private void Start()
    {
        StartCoroutine(Shot());
    }

    void Generate()
    {
        listBullets = new GameObject[count];
        listScBullets = new MachineGunBulletHandler[count];
        for (int i = 0; i < count; i++)
        {
            listBullets[i] = Instantiate(preBullet, container);
            listBullets[i].SetActive(false);
            listScBullets[i] = listBullets[i].GetComponent<MachineGunBulletHandler>();
        }
    }

    public void SetDefaultBullets()
    {
        for (int i = 0; i < listBullets.Length; i++)
        {
            listBullets[i].SetActive(false);
            listBullets[i].transform.SetParent(container);
            listScBullets[i].rb.velocity = Vector2.zero;
            listBullets[i].transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public IEnumerator Shot()
    {
        while (true)
        {
            for (int i = 0; i < listBullets.Length; i++)
            {
                listBullets[i].transform.SetParent(GameController.instance.poolBullets);
                listBullets[i].SetActive(true);
                listScBullets[i].Shot(speedBullet, listBullets[i].transform.right);
                yield return new WaitForSeconds(timeDistance);
            }
            yield return new WaitForSeconds(turnTimeDelay);
            SetDefaultBullets();
        }
    }

    public void PlayShotAni()
    {
        ani.Play("game idle");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            PlayShotAni();
        }
    }
}
