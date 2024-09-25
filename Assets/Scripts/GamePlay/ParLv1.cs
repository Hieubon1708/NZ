using DG.Tweening;
using UnityEngine;

public class ParLv1 : MonoBehaviour
{
    public static ParLv1 instance;
    public GameObject zomHitOnRoadPrefab;
    public GameObject zomHitOnHeroPrefab;
    public GameObject[] zomHitOnRoads;
    public GameObject[] zomHitOnHeros;
    public int amoutZomHitOnRoad;
    public int amoutZomHitOnHero;
    int currentCountZomHitOnRoad;
    int currentCountZomHitOnHero;

    public void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        zomHitOnHeros = new GameObject[amoutZomHitOnHero];
        for (int i = 0; i < zomHitOnHeros.Length; i++)
        {
            zomHitOnHeros[i] = Instantiate(zomHitOnHeroPrefab, GameController.instance.poolPars);
            zomHitOnHeros[i].SetActive(false);
        }
        zomHitOnRoads = new GameObject[amoutZomHitOnRoad];
        for (int i = 0; i < zomHitOnRoads.Length; i++)
        {
            zomHitOnRoads[i] = Instantiate(zomHitOnRoadPrefab, GameController.instance.poolPars);
            zomHitOnRoads[i].SetActive(false);
        }
    }


    public void PlayZomHitOnRoadParticle(Vector2 pos)
    {
        GameObject h = zomHitOnRoads[currentCountZomHitOnRoad];
        h.transform.position = pos;
        h.SetActive(true);
        currentCountZomHitOnRoad++;
        if (currentCountZomHitOnRoad == zomHitOnRoads.Length) currentCountZomHitOnRoad = 0;
        DOVirtual.DelayedCall(1f, delegate { h.SetActive(false); });
    }

    public void PlayZomHitOnHeroParticle(Vector2 pos)
    {
        GameObject h = zomHitOnHeros[currentCountZomHitOnHero];
        h.transform.position = pos;
        h.SetActive(true);
        currentCountZomHitOnHero++;
        if (currentCountZomHitOnHero == zomHitOnHeros.Length) currentCountZomHitOnHero = 0;
        DOVirtual.DelayedCall(1f, delegate { h.SetActive(false); });
    }
}
