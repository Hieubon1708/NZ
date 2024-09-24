using DG.Tweening;
using UnityEngine;

public class ParController : MonoBehaviour
{
    public static ParController instance;

    public GameObject roadBulletHolePrefab;
    public GameObject zomDiePrefab;
    public GameObject blockDestroyPrefab;
    public GameObject playerDiePrefab;
    public GameObject boomEffectPrefab;
    public GameObject boomHolePrefab;
    public GameObject[] roadBulletHole;
    public GameObject[] boomHoles;
    public GameObject[] boomEffects;
    public GameObject[] zomDies;
    public GameObject[] blockDestroys;
    public GameObject playerDie;
    public Transform container;
    public int amoutHoleBullet;
    public int amoutZomeDie;
    public int amoutBoom;
    public int amoutBlockDestroy;
    int currentCount;
    int currentCountZomDie;
    int currentCountBlockDestroy;
    int currentCountBoom;

    private void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        roadBulletHole = new GameObject[amoutHoleBullet];
        for (int i = 0; i < roadBulletHole.Length; i++)
        {
            roadBulletHole[i] = Instantiate(roadBulletHolePrefab, container);
            roadBulletHole[i].SetActive(false);
        }
        zomDies = new GameObject[amoutZomeDie];
        for (int i = 0; i < zomDies.Length; i++)
        {
            zomDies[i] = Instantiate(zomDiePrefab, container);
            zomDies[i].SetActive(false);
        }
        blockDestroys = new GameObject[amoutBlockDestroy];
        for (int i = 0; i < blockDestroys.Length; i++)
        {
            blockDestroys[i] = Instantiate(blockDestroyPrefab, container);
            blockDestroys[i].SetActive(false);
        }
        playerDie = Instantiate(playerDiePrefab, container);
        playerDie.SetActive(false);
        boomEffects = new GameObject[amoutBoom];
        for (int i = 0; i < boomEffects.Length; i++)
        {
            boomEffects[i] = Instantiate(boomEffectPrefab, container);
            boomEffects[i].SetActive(false);
        }
        boomHoles = new GameObject[amoutBoom];
        for (int i = 0; i < boomEffects.Length; i++)
        {
            boomHoles[i] = Instantiate(boomHolePrefab, container);
            boomHoles[i].SetActive(false);
        }
    }

    public void PlayRoadBulletHoleParticle(Vector2 pos)
    {
        GameObject d = roadBulletHole[currentCount].gameObject;
        d.transform.position = pos;
        d.SetActive(true);
        currentCount++;
        if (currentCount == roadBulletHole.Length) currentCount = 0;
        DOVirtual.DelayedCall(5f, delegate { d.SetActive(false); });
    }

    public void PlayPlayerDieParticle(Vector2 pos)
    {
        playerDie.transform.position = pos;
        playerDie.SetActive(true);
    }

    public void PlayZomDieParticle(Vector2 pos)
    {
        GameObject d = zomDies[currentCountZomDie];
        d.transform.position = pos;
        d.SetActive(true);
        currentCountZomDie++;
        if (currentCountZomDie == zomDies.Length) currentCountZomDie = 0;
        DOVirtual.DelayedCall(1f, delegate { d.SetActive(false); });
    }

    public void PlayBlockDestroyParticle(Vector2 pos)
    {
        GameObject d = blockDestroys[currentCountBlockDestroy];
        d.transform.position = pos;
        d.SetActive(true);
        currentCountBlockDestroy++;
        if (currentCountBlockDestroy == blockDestroys.Length) currentCountBlockDestroy = 0;
        DOVirtual.DelayedCall(1f, delegate { d.SetActive(false); });
    }
    public void PlayBoomParticle(Vector2 pos)
    {
        GameObject b1 = boomEffects[currentCountBoom];
        GameObject b2 = boomHoles[currentCountBoom];
        b1.transform.position = pos;
        b2.transform.position = pos;
        b1.SetActive(true);
        b2.SetActive(true);
        currentCountBoom++;
        if (currentCountBoom == boomEffects.Length) currentCountBoom = 0;
        DOVirtual.DelayedCall(5f, delegate { b1.SetActive(false); b2.SetActive(false); });
    }
}
