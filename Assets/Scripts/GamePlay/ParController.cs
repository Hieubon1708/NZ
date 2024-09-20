using UnityEngine;

public class ParController : MonoBehaviour
{
    public static ParController instance;

    public GameObject roadBulletHolePrefab;
    public ParticleSystem[] roadBulletHole;
    public Transform container;
    public int count;
    int currentCount;

    private void Awake()
    {
        instance = this;
        Generate();
    }

    private void Start()
    {

    }

    void Generate()
    {
        roadBulletHole = new ParticleSystem[count];
        for (int i = 0; i < roadBulletHole.Length; i++)
        {
            roadBulletHole[i] = Instantiate(roadBulletHolePrefab, container).GetComponent<ParticleSystem>();
        }
    }

    public void PlayRoadBulletHole(Vector2 pos)
    {
        roadBulletHole[currentCount].transform.position = pos;
        roadBulletHole[currentCount].Play();
        currentCount++;
        if (currentCount == roadBulletHole.Length) currentCount = 0;
    }
}
