using System.Collections;
using UnityEngine;

public class SawHandler : WeaponShoter
{
    int count;
    public Animation sawAttackAni;
    public ParticleSystem sawBlood;
    Coroutine blood;
    public GameObject sawBoosterPref;
    public int amout;
    int countBooster;
    int amoutSawFBooster = 1;
    public Rigidbody2D[] sawBoosters;
    public Transform container;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            count++;
            sawAttackAni.Play();
            if (blood == null) blood = StartCoroutine(SawBlood());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            count--;
            if (count == 0)
            {
                //blood = null;
                StopCoroutine(blood);
                sawAttackAni.Stop();
            }
        }
    }

    IEnumerator SawBlood()
    {
        while (true)
        {
            sawBlood.Play();
            yield return new WaitForSeconds(0.35f);
        }
    }

    public override void StartGame() { }

    public override void UseBooster()
    {
        ani.SetTrigger("booster");
        StartCoroutine(ThrowSaw());
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
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < listS.Length; i++)
        {
            listS[i].SetActive(false);
        }
    }

    public override void SetDamageBooster(int damage)
    {
        for (int i = 0; i < sawBoosters.Length; i++)
        {
            sawBoosters[i].name = damage.ToString();
        }
    }

    public override void SetDamage(int damage)
    {
        gameObject.name = damage.ToString();
    }
}
