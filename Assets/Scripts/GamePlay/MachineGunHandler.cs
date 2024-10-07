using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradeEvolutionController;

public class MachineGunHandler : WeaponShoter
{
    public GameObject preBullet;
    public GameObject preBulletBooster;
    public List<List<GameObject>> listBullets = new List<List<GameObject>>();
    public List<List<MachineGunBulletHandler>> listScBullets = new List<List<MachineGunBulletHandler>>();
    public List<GameObject> listBulletBoosters = new List<GameObject>();
    public List<MachineGunBulletHandler> listScBulletBoosters = new List<MachineGunBulletHandler>();
    public Transform[] containers;
    public GameObject block;

    public int startAmoutLine = 1;
    public int amoutLine;
    public int count;

    Coroutine[] shots;
    Coroutine shotBoosters;
    Coroutine endBooster;
    int currentCountBoosterBullet;
    int currentCountBullet;

    public float timeDistance;
    public float timeDistanceBooster;
    public float speedBullet;
    public float cooldown;
    float startCooldown = 1;
    public float attackDuration;
    float startAttackDuration = 2;

    public void Awake()
    {
        Generate();
    }

    public override void LoadData()
    {
        AddBullet();
        AttackDurationChange();
        AttackCooldownChange();
        Booster.instance.DecreaseEnergyMachineGun();
    }

    public void AddBullet()
    {
        if (instance.machineGuns.Contains(MACHINEGUNEVO.ADDBULLET))
        {
            int amout = instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.ADDBULLET);
            amoutLine = startAmoutLine + amout;
        }
    }

    public void AttackDurationChange()
    {
        float multiplier = 1;
        if (instance.machineGuns.Contains(MACHINEGUNEVO.ATTACKDURATION))
        {
            int level = instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.ATTACKDURATION);

            if (level == 1) multiplier = 0.75f;
            else if (level == 2) multiplier = 0.5f;
            else if (level == 3) multiplier = 0.25f;
        }
        attackDuration = startAttackDuration * multiplier;
    }

    public void AttackCooldownChange()
    {
        float multiplier = 1;
        if (instance.machineGuns.Contains(MACHINEGUNEVO.ATTACKCOOLDOWN))
        {
            int level = instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.ATTACKCOOLDOWN);

            if (level == 1) multiplier = 0.7f;
            else if (level == 2) multiplier = 0.5f;
            else if (level == 3) multiplier = 0.3f;
        }
        cooldown = startCooldown * multiplier;
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
        for (int i = 0; i < amoutLine; i++)
        {
            shots[i] = StartCoroutine(Shot(listBullets[i], listScBullets[i], i, timeDistance));
        }
    }

    void Generate()
    {
        shots = new Coroutine[3];
        for (int j = 0; j < 3; j++)
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
            float time = 0;
            ani.SetBool("attack", true);
            while (time <= attackDuration)
            {
                time += timeDistance;
                SetDefaultBullet(listB[currentCountBullet], listScB[currentCountBullet], index);
                listB[currentCountBullet].transform.SetParent(GameController.instance.poolBullets);
                listB[currentCountBullet].SetActive(true);
                listScB[currentCountBullet].Shot(speedBullet, listB[currentCountBullet].transform.right);
                currentCountBullet++;
                if (currentCountBullet == listB.Count) currentCountBullet = 0;
                yield return new WaitForSeconds(timeDistance);
            }
            ani.SetBool("attack", false);
            yield return new WaitForSeconds(cooldown);
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
        if (endBooster != null) StopCoroutine(endBooster);
        StopAll();
        shotBoosters = StartCoroutine(BoosterShot(listBulletBoosters, listScBulletBoosters, 0, timeDistanceBooster));
    }

    void EndBooster()
    {
        endBooster = StartCoroutine(EndBoosterHandle());
    }

    IEnumerator EndBoosterHandle()
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
        float multiplier = 1;
        float decreaseDamage = 1;

        IncreaseDamage(ref multiplier);

        for (int i = 0; i < listBullets.Count; i++)
        {
            if (i == 1) decreaseDamage = 0.4f;
            else if (i == 2) decreaseDamage = 0.5f;
            for (int j = 0; j < listBullets[i].Count; j++)
            {
                listBullets[i][j].name = (damage * multiplier * decreaseDamage).ToString();
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

    void IncreaseDamage(ref float multiplier)
    {
        if (instance.machineGuns.Contains(MACHINEGUNEVO.INCREASEDAMAGE))
        {
            int level = instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.INCREASEDAMAGE);

            if (level == 1) multiplier = 1.2f;
            else if (level == 2) multiplier = 1.4f;
            else if (level == 3) multiplier = 1.6f;
        }
    }
}
