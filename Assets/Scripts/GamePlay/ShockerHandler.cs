using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UpgradeEvolutionController;

public class ShockerHandler : WeaponShoter
{
    int count;
    public Animation shockerAttackAni;
    public ParticleSystem shockerLight;
    public Transform pointSpawnBooster;
    Coroutine light;
    Coroutine shockerBooster;
    public GameObject shockerBoosterPref;
    public int amout;
    int countBooster;
    int amoutShockerBooster = 1;
    public Rigidbody2D[] shockerBoosters;
    public ShockerBoosterHandler[] shockerScBoosters;
    public Transform container;
    public List<GameObject> listEs = new List<GameObject>();
    CircleCollider2D col;

    public void Awake()
    {
        shockerBoosters = new Rigidbody2D[amout];
        shockerScBoosters = new ShockerBoosterHandler[amout];
        for (int i = 0; i < amout; i++)
        {
            GameObject s = Instantiate(shockerBoosterPref, container);
            shockerBoosters[i] = s.GetComponent<Rigidbody2D>();
            shockerScBoosters[i] = s.GetComponent<ShockerBoosterHandler>();
            s.SetActive(false);
        }
    }

    public void Start()
    {
        col = GetComponent<CircleCollider2D>();
    }

    public override void LoadData()
    {
        AddShockerBooster();
        Booster.instance.DecreaseEnergyShocker(level);
    }

    public void AddShockerBooster()
    {
        if (instance.IsShockerContains(SHOCKEREVO.ADDFROMBOOSTER, level))
        {
            amoutShockerBooster++;
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
            if (count == 0) light = StartCoroutine(ShockerLight());
            shockerAttackAni.Play();
            count++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (listEs.Contains(collision.attachedRigidbody.gameObject))
            {
                EnemyHandler eSc = EnemyTowerController.instance.GetScE(collision.attachedRigidbody.gameObject);
                if (eSc.isStunByWeapon) eSc.EndBumpByWeapon();
                listEs.Remove(collision.attachedRigidbody.gameObject);
            }
            count--;
            if (count == 0)
            {
                if (light != null) StopCoroutine(light);
                shockerAttackAni.Stop();
            }
        }
    }

    public override void Restart()
    {
        base.Restart();
        shockerAttackAni.Stop();
        if (light != null) StopCoroutine(light);
        if (shockerBooster != null) StopCoroutine(shockerBooster);
        for (int i = 0; i < shockerBoosters.Length; i++)
        {
            if (shockerBoosters[i].gameObject.activeSelf) shockerBoosters[i].gameObject.SetActive(false);
        }
        count = 0;
    }

    IEnumerator ShockerLight()
    {
        while (true)
        {
            if (instance.IsShockerContains(SHOCKEREVO.PUSHESENEMIES, level))
            {
                int amout = instance.GetAmoutShockerEvo(SHOCKEREVO.PUSHESENEMIES, level);

                int percentage = 0;
                if (amout == 1) percentage = 5;
                else if (amout == 2) percentage = 10;
                else if (amout == 3) percentage = 15;

                int random = Random.Range(0, 100);
                if (random <= percentage)
                {
                    for (int i = 0; i < listEs.Count; i++)
                    {
                        EnemyHandler eSc = EnemyTowerController.instance.GetScE(listEs[i]);
                        eSc.StartBumpByWeapon();
                    }
                }
            }

            if (instance.IsShockerContains(SHOCKEREVO.STUNENEMY, level))
            {
                int amout = instance.GetAmoutShockerEvo(SHOCKEREVO.STUNENEMY, level);
                int percentage = 0; float time = 0f;
                if (amout == 1)
                {
                    percentage = 10;
                    time = 1.5f;
                }
                else if (amout == 2)
                {
                    percentage = 20;
                    time = 2f;
                }
                else if (amout == 3)
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

            shockerLight.Play();
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
                EnemyHandler eSc = EnemyTowerController.instance.GetScE(listEs[i]);
                eSc.Stun(time);
                Vector2 topBound = eSc.GetPositionTopBound(eSc.col);
                ParController.instance.PlayStunOnEnemyParticle(new Vector2(topBound.x, topBound.y + 0.35f), time, eSc.healthBar.transform);
            }
        }
    }

    public override void StartGame() { col.enabled = true; }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
        shockerBooster = StartCoroutine(ThrowLight());
    }

    IEnumerator ThrowLight()
    {
        for (int i = 0; i < amoutShockerBooster; i++)
        {
            GameObject s = shockerBoosters[countBooster].gameObject;
            s.transform.position = pointSpawnBooster.position;
            s.SetActive(true);
            shockerBoosters[countBooster].velocity = new Vector2(10, shockerBoosters[countBooster].velocity.y);
            shockerScBoosters[countBooster].ZoomInBooster();
            countBooster++;
            if (countBooster == shockerBoosters.Length) countBooster = 0;
            DOVirtual.DelayedCall(1f, delegate
            {
                s.SetActive(false);
            });
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void SetDamageBooster(int damage)
    {
        float multiplier = 1;

        GetPercentageIncreaseDamage(ref multiplier);

        for (int i = 0; i < shockerBoosters.Length; i++)
        {
            shockerBoosters[i].name = ((int)(damage * multiplier)).ToString();
        }
    }

    public override void SetDamage(int damage)
    {
        float multiplier = 1;

        GetPercentageIncreaseDamage(ref multiplier);

        gameObject.name = ((int)(damage * multiplier)).ToString();
    }

    void GetPercentageIncreaseDamage(ref float multiplier)
    {
        if (instance.IsShockerContains(SHOCKEREVO.INCREASEDAMAGE, level))
        {
            int amout = instance.GetAmoutShockerEvo(SHOCKEREVO.INCREASEDAMAGE, level);

            if (amout == 1) multiplier = 1.2f;
            else if (amout == 2) multiplier = 1.4f;
            else if (amout == 3) multiplier = 1.6f;
        }
    }

    public override void DisableWeapon()
    {
        col.enabled = false;
        shockerAttackAni.Stop();
        if (light != null) StopCoroutine(light);
        if (shockerBooster != null) StopCoroutine(shockerBooster);
        if (rotate != null) StopCoroutine(rotate);
        count = 0;
    }
}
