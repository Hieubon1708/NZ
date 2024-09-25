using UnityEngine;

public class EnemyBulletHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    public CircleCollider2D col;
    Vector2 anglePrevious;
    public Vector2 target;
    public int damage;

    public void Start()
    {
        name = damage.ToString();
    }

    private void FixedUpdate()
    {
        if (anglePrevious != null)
        {
            Vector2 dir = anglePrevious - (Vector2)transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, EUtils.GetAngle(dir) + 90), 0.1f);
        }
        anglePrevious = transform.position;
        if (Vector2.Distance(transform.position, target) <= 0.3f) col.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Road_2"))
        {
            gameObject.SetActive(false);
            ParLv1.instance.PlayZomHitOnRoadParticle(transform.position);
        }
        if (collision.CompareTag("Block") || collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            ParLv1.instance.PlayZomHitOnHeroParticle(transform.position);
        }
    }
}
