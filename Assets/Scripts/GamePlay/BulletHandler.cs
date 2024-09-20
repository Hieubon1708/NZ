using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    public TrailRenderer trailRenderer;
    int indexRoadCollider;

    public void Shot(float speed, Vector2 dir)
    {
        indexRoadCollider = Random.Range(0, RoadColliderGenerator.instance.count);
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        //Debug.DrawLine(transform.position, raycastDirection * 10, Color.red, 10);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Road_" + indexRoadCollider))
        {
            gameObject.SetActive(false);
            ParController.instance.PlayRoadBulletHole(transform.position);
        }
    }
}
