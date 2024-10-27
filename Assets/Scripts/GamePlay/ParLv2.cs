using DG.Tweening;
using UnityEngine;

public class ParLv2 : MonoBehaviour
{
    public static ParLv2 instance;

    public GameObject spiderHitOnBlockOHeroPrefab;
    public GameObject[] spiderHitOnBlockOHeros;
    public int amoutSpiderHitOnBlockOHero;
    int currentCountSpiderHitOnBlockOHero;

    public void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        spiderHitOnBlockOHeros = new GameObject[amoutSpiderHitOnBlockOHero];
        for (int i = 0; i < spiderHitOnBlockOHeros.Length; i++)
        {
            spiderHitOnBlockOHeros[i] = Instantiate(spiderHitOnBlockOHeroPrefab, GameController.instance.poolPars);
            spiderHitOnBlockOHeros[i].SetActive(false);
        }
    }

    public void PlaySpriderHitOnBlockOHeroParticle(Vector2 pos)
    {
        GameObject h = spiderHitOnBlockOHeros[currentCountSpiderHitOnBlockOHero];
        h.transform.position = pos;
        h.SetActive(true);
        currentCountSpiderHitOnBlockOHero++;
        if (currentCountSpiderHitOnBlockOHero == spiderHitOnBlockOHeros.Length) currentCountSpiderHitOnBlockOHero = 0;
        DOVirtual.DelayedCall(1f, delegate { h.SetActive(false); });
    }

    public void SetActivePar(bool isActive)
    {
        for (int i = 0; i < spiderHitOnBlockOHeros.Length; i++)
        {
            if (spiderHitOnBlockOHeros[i].activeSelf) spiderHitOnBlockOHeros[i].SetActive(isActive);
        }
    }
}
