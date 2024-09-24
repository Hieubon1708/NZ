using DG.Tweening;
using UnityEngine;

public class ParController : MonoBehaviour
{
    public static ParController instance;

    public GameObject roadBulletHolePrefab;
    public GameObject dieParticlePrefab;
    public ParticleSystem[] roadBulletHole;
    public GameObject[] dieParticles;
    public Transform container;
    public int amoutHoleBullet;
    public int amoutDieParitcle;
    int currentCount;
    int currentCountDieParticle;

    private void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        roadBulletHole = new ParticleSystem[amoutHoleBullet];
        for (int i = 0; i < roadBulletHole.Length; i++)
        {
            roadBulletHole[i] = Instantiate(roadBulletHolePrefab, container).GetComponent<ParticleSystem>();
        }
        dieParticles = new GameObject[amoutDieParitcle];
        for (int i = 0; i < dieParticles.Length; i++)
        {
            dieParticles[i] = Instantiate(dieParticlePrefab, container);
            dieParticles[i].SetActive(false);
        }
    }

    public void PlayRoadBulletHole(Vector2 pos)
    {
        roadBulletHole[currentCount].transform.position = pos;
        roadBulletHole[currentCount].Play();
        currentCount++;
        if (currentCount == roadBulletHole.Length) currentCount = 0;
    }

    public void PlayDieParticle(Vector2 pos)
    {
        GameObject d = dieParticles[currentCountDieParticle];
        d.transform.position = pos;
        d.SetActive(true);
        currentCountDieParticle++;
        if (currentCountDieParticle == dieParticles.Length) currentCountDieParticle = 0;
        DOVirtual.DelayedCall(1f, delegate { d.SetActive(false); });
    }
}
