using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAttackEventT2 : MonoBehaviour
{
    public GameObject preBullet;
    public SortingGroup sortingGroupParent;
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

    public void SetActiveBullet(bool isActive)
    {
        for (int i = 0; i < scBullets.Length; i++)
        {
            if (scBullets[i].gameObject.activeSelf) scBullets[i].gameObject.SetActive(isActive);
        }
    }

    public void ShotEvent()
    {
        scBullets[index].gameObject.SetActive(true);
        scBullets[index].transform.position = mouth.position;
        scBullets[index].sortingGroup.sortingLayerName = sortingGroupParent.sortingLayerName;
        float YUnder = PlayerController.instance.transform.position.y;
        if (BlockController.instance.blocks.Count > 0) YUnder = BlockController.instance.blocks[0].transform.position.y;
        float YAbove = PlayerController.instance.transform.position.y + 0.7f;
        float x = PlayerController.instance.transform.position.x;

        float randomTarget = Random.Range(YUnder, YAbove);

        Vector2 target = new Vector2(x, randomTarget);

        float angle = EUtils.GetAngle(target - (Vector2)mouth.position);
        angle += 145 - angle;

        float distanceX = target.x - mouth.position.x;
        float distanceY = Mathf.Clamp(target.y - mouth.position.y, 0.0001f, float.MaxValue);
        float time = distanceX / (Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Sqrt(distanceX * distanceX * Mathf.Abs(Physics2D.gravity.y) / (2 * distanceX * Mathf.Tan(angle * Mathf.Deg2Rad) + distanceY)));
        float velocityX = distanceX / time;
        float velocityY = (distanceY + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time * time) / time;

        Vector2 velocity = new Vector2(velocityX, velocityY);

        scBullets[index].transform.localRotation = Quaternion.Euler(0, 0, EUtils.GetAngle(velocity.normalized) - 90);
        scBullets[index].rb.velocity = velocity;

        index++;
        if (index == scBullets.Length) index = 0;
        //Debug.DrawLine(mouth.position, target, Color.red, 1);
    }
}
