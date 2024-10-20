using UnityEngine;

public class EnemyAttackEventT3 : MonoBehaviour
{
    public GameObject preBullet;
    public EnemyBulletHandler[] scBullets;
    public int count;
    public Transform tail;
    public Enemy enemy;
    int index;

    void Awake()
    {
        Generate();
    }

    void Generate()
    {
        scBullets = new EnemyBulletHandler[count];
        for (int i = 0; i < count; i++)
        {
            GameObject b = Instantiate(preBullet, GameController.instance.poolBullets);
            b.name = enemy.damage.ToString();
            b.SetActive(false);
            scBullets[i] = b.GetComponent<EnemyBulletHandler>();
        }
    }

    public void ShotEvent()
    {
        scBullets[index].gameObject.SetActive(true);
        scBullets[index].transform.position = tail.position;
        float YUnder = PlayerController.instance.transform.position.y;
        if(BlockController.instance.blocks.Count > 0 ) YUnder = BlockController.instance.blocks[0].transform.position.y;
        float YAbove = PlayerController.instance.transform.position.y + 0.7f;
        float x = PlayerController.instance.transform.position.x + 0.7f;

        float randomTarget = Random.Range(YUnder, YAbove);

        Vector2 target = new Vector2(x, randomTarget);
        Vector2 dir = target - (Vector2)tail.position;

        scBullets[index].transform.localRotation = Quaternion.Euler(0, 0, EUtils.GetAngle(dir) - 90);
        scBullets[index].rb.velocity = dir * 3;

        index++;
        if (index == scBullets.Length) index = 0;
        //Debug.DrawLine(tail.position, target, Color.red, 1);
    }
}
