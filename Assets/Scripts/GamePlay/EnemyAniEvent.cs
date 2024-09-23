using System.Linq;
using UnityEngine;

public class EnemyAniEvent : MonoBehaviour
{
    public GameObject preBullet;
    public Rigidbody2D[] rbBullets;
    public int count;
    public Transform mouth;
    float force = 13;
    int index;
    Vector2 dir;

    void Awake()
    {
        Generate();
    }

    void Generate()
    {
        rbBullets = new Rigidbody2D[count];
        for (int i = 0; i < count; i++)
        {
            GameObject b = Instantiate(preBullet, transform);
            Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
            b.SetActive(false);
            rbBullets[i] = rb;
        }
    }

    public void ShotEvent()
    {
        rbBullets[index].gameObject.SetActive(true);
        rbBullets[index].transform.position = mouth.position;
        float YUnder = -1, YAbove = -1, x = 0;
        if (BlockController.instance.blocks.Count > 0) YUnder = BlockController.instance.blocks[0].transform.position.y;
        else YUnder = PlayerController.instance.transform.position.y;
        x = PlayerController.instance.transform.position.x;
        YAbove = PlayerController.instance.transform.position.y + 0.75f;
        float randomTarget = Random.Range(YUnder, YAbove);
        MakeAngle(new Vector2(x, randomTarget));
        rbBullets[index].transform.localRotation = Quaternion.Euler(0, 0, EUtils.GetAngle(dir) - 90);
        rbBullets[index].velocity = force * dir;
        index++;
        if (index == rbBullets.Length) index = 0;
    }

    public GameObject p;

    public void MakeAngle(Vector2 target)
    {
        for (float i = 110; i <= 170; i++)
        {
            dir = GetDir(i);
            for (int j = 0; j < 30; j++)
            {
                Vector2 r = PointPosition(j * 0.05f);
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
