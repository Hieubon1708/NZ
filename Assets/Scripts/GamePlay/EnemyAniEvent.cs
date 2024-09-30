using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAniEvent : MonoBehaviour
{
    public GameObject preBullet;
    public EnemyBulletHandler[] scBullets;
    public int count;
    public Transform mouth;
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
        scBullets[index].gameObject.SetActive(false);
        scBullets[index].gameObject.SetActive(true);
        scBullets[index].transform.position = mouth.position;
        float YUnder = -1, YAbove = -1, x = PlayerController.instance.transform.position.x + 0.7f;
        if (BlockController.instance.blocks.Count > 0) YUnder = BlockController.instance.blocks[0].transform.position.y - 0.75f;
        else YUnder = PlayerController.instance.transform.position.y;
        YAbove = PlayerController.instance.transform.position.y + 0.7f;
        float randomTarget = Random.Range(YUnder, YAbove);

        //Vector2 target = new Vector2(x, randomTarget);
        Vector2 target = new Vector2(BlockController.instance.blocks[1].transform.position.x + 0.7f, BlockController.instance.blocks[1].transform.position.y);

        Vector2 direction = (target - (Vector2)mouth.position).normalized;
        float angle = EUtils.GetAngle(direction);
        angle = angle > 90 ? 90 - (angle - 90) : angle;

        float sinAngle = Mathf.Sin(angle * Mathf.Deg2Rad);
        float distance = Mathf.Abs(mouth.position.x - target.x);
        float V0Squared = distance * 9.81f / sinAngle;
        float V0 = Mathf.Sqrt(V0Squared);

        scBullets[index].transform.localRotation = Quaternion.Euler(0, 0, EUtils.GetAngle(direction) - 90);
        scBullets[index].rb.velocity = V0 * direction;

        index++;
        if (index == scBullets.Length) index = 0;
        Debug.DrawLine(mouth.position, target, Color.red, 1);
    }
}
