using UnityEngine;

public class EnemyAniEvent : MonoBehaviour
{
    public GameObject preBullet;
    public EnemyBulletHandler[] scBullets;
    public int count;
    public Transform mouth;
    public Enemy enemy;
    float force;
    int index;
    Vector2 dir;

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
        force = 9;
        scBullets[index].gameObject.SetActive(false);
        scBullets[index].gameObject.SetActive(true);
        scBullets[index].transform.position = mouth.position;
        float YUnder = -1, YAbove = -1, x = 0;
        if (BlockController.instance.blocks.Count > 0) YUnder = BlockController.instance.blocks[0].transform.position.y - 0.75f;
        else YUnder = PlayerController.instance.transform.position.y;
        x = PlayerController.instance.transform.position.x;
        YAbove = PlayerController.instance.transform.position.y + 0.75f;
        float randomTarget = Random.Range(YUnder, YAbove);
        force += randomTarget;
        Vector2 target = new Vector2(x, randomTarget);
        Vector2 dir = target - (Vector2)mouth.transform.position;
        float distance = Vector2.Distance(target, mouth.transform.position);
        //MakeAngle(target);
        scBullets[index].target = target;
        scBullets[index].transform.rotation = Quaternion.Euler(0, 0, EUtils.GetAngle(dir) - 90);
        scBullets[index].rb.velocity = distance * dir;
        index++;
        if (index == scBullets.Length) index = 0;
        Debug.DrawLine(mouth.position, new Vector2(x, randomTarget), Color.red, 0.5f);
    }

    public GameObject p;

    public void MakeAngle(Vector2 target)
    {
        for (float i = 100; i <= 130; i++)
        {
            dir = GetDir(i);
            for (int j = 0; j < 100; j++)
            {
                Vector2 r = PointPosition(j * 0.025f);
                //Instantiate(p, r, Quaternion.Euler(0,0, EUtils.GetAngle(dir)));
                if (Vector2.Distance(r, target) <= 0.25f) return;
            }
        }
    }

    Vector2 GetDir(float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
    }

    Vector2 PointPosition(float t)
    {
        return (Vector2)mouth.position + (dir.normalized * force * t) + 0.5f * Physics2D.gravity * (t * t);
    }
}
