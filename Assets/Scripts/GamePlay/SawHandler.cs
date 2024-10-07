using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradeEvolutionController;

public class SawHandler : WeaponShoter
{
    int count;
    public Animation sawAttackAni;
    public ParticleSystem sawBlood;
    Coroutine blood;
    Coroutine sawBooster;
    public GameObject sawBoosterPref;
    public int amout;
    int countBooster;
    int amoutSawFBooster = 1;
    public Rigidbody2D[] sawBoosters;
    public Transform container;
    public List<GameObject> listEs = new List<GameObject>();

    public void Awake()
    {
        sawBoosters = new Rigidbody2D[amout];
        for (int i = 0; i < amout; i++)
        {
            GameObject s = Instantiate(sawBoosterPref, container);
            sawBoosters[i] = s.GetComponent<Rigidbody2D>();
            s.SetActive(false);
        }
    }

    public override void LoadData()
    {
        AddSawBooster();
        Booster.instance.DecreaseEnergySaw();
    }

    public void AddSawBooster()
    {
        if (instance.saws.Contains(SAWEVO.ADDFROMBOOSTER))
        {
            amoutSawFBooster++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!listEs.Contains(collision.attachedRigidbody.gameObject))
            {
                listEs.Add(collision.attachedRigidbody.gameObject);
            }
            if (count == 0) blood = StartCoroutine(SawBlood());
            sawAttackAni.Play();
            count++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (listEs.Contains(collision.attachedRigidbody.gameObject))
            {
                EnemyHandler eSc = EnemyTowerController.instance.GetScEInTower(collision.attachedRigidbody.gameObject);
                eSc.EndBumpByWeapon();
                listEs.Remove(collision.attachedRigidbody.gameObject);
            }
            count--;
            if (count == 0)
            {
                if (blood != null) StopCoroutine(blood);
                sawAttackAni.Stop();
            }
        }
    }

    public override void Restart()
    {
        base.Restart();
        sawAttackAni.Stop();
        if (blood != null) StopCoroutine(blood);
        if (sawBooster != null) StopCoroutine(sawBooster);
        for (int i = 0; i < sawBoosters.Length; i++)
        {
            if (sawBoosters[i].gameObject.activeSelf) sawBoosters[i].gameObject.SetActive(false);
        }
        count = 0;
    }

    IEnumerator SawBlood()
    {
        while (true)
        {
            if (instance.saws.Contains(SAWEVO.PUSHESENEMIES))
            {
                int level = instance.GetAmoutSawEvo(SAWEVO.PUSHESENEMIES);

                int percentage = 0;
                if (level == 1) percentage = 5;
                else if (level == 2) percentage = 10;
                else if (level == 3) percentage = 15;

                int random = Random.Range(0, 100);
                if (random <= percentage)
                {
                    for (int i = 0; i < listEs.Count; i++)
                    {
                        EnemyHandler eSc = EnemyTowerController.instance.GetScEInTower(listEs[i]);
                        eSc.StartBumpByWeapon();
                    }
                }
            }

            if (instance.saws.Contains(SAWEVO.STUNENEMY))
            {
                int level = instance.GetAmoutSawEvo(SAWEVO.STUNENEMY);
                int percentage = 0; float time = 0f;
                if (level == 1)
                {
                    percentage = 20;
                    time = 1.5f;
                }
                else if (level == 2)
                {
                    percentage = 25;
                    time = 2f;
                }
                else if (level == 3)
                {
                    percentage = 30;
                    time = 2.5f;
                }
                int random = Random.Range(0, 100);

                if (random <= percentage)
                {
                    Stun(percentage, time);
                }
            }

            sawBlood.Play();
            yield return new WaitForSeconds(0.35f);
        }
    }

    void Stun(float chance, float time)
    {
        float random = Random.Range(0f, 10f);
        if (random <= chance)
        {
            for (int i = 0; i < listEs.Count; i++)
            {
                EnemyHandler eSc = EnemyTowerController.instance.GetScEInTower(listEs[i]);
                eSc.Stun(time);
                Vector2 topBound = eSc.GetPositionTopBound(eSc.col);
                ParController.instance.PlayStunOnEnemyParticle(new Vector2(topBound.x, topBound.y + 0.35f), time, eSc.transform);
            }
        }
    }

    public override void StartGame() { }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
        sawBooster = StartCoroutine(ThrowSaw());
    }

    IEnumerator ThrowSaw()
    {
        GameObject[] listS = new GameObject[amoutSawFBooster];
        for (int i = 0; i < amoutSawFBooster; i++)
        {
            listS[i] = sawBoosters[countBooster].gameObject;
            listS[i].transform.position = transform.position;
            listS[i].SetActive(true);
            sawBoosters[countBooster].velocity = new Vector2(25, sawBoosters[countBooster].velocity.y);
            countBooster++;
            if (countBooster == sawBoosters.Length) countBooster = 0;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public override void SetDamageBooster(int damage)
    {
        float multiplier = 1;

        GetPercentageIncreaseDamage(ref multiplier);

        for (int i = 0; i < sawBoosters.Length; i++)
        {
            sawBoosters[i].name = (damage * multiplier).ToString();
        }
    }

    public override void SetDamage(int damage)
    {
        float multiplier = 1;

        GetPercentageIncreaseDamage(ref multiplier);

        gameObject.name = (damage * multiplier).ToString();
    }

    void GetPercentageIncreaseDamage(ref float multiplier)
    {
        if (instance.saws.Contains(SAWEVO.INCREASEDAMAGE))
        {
            int level = instance.GetAmoutSawEvo(SAWEVO.INCREASEDAMAGE);

            if (level == 1) multiplier = 1.2f;
            else if (level == 2) multiplier = 1.4f;
            else if (level == 3) multiplier = 1.6f;
        }
    }
}
