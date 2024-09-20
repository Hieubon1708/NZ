using UnityEngine;

public class MachineGunBulletHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    int indexRoadCollider;

    public void Shot(float speed, Vector2 dir)
    {
        indexRoadCollider = Random.Range(1, RoadColliderGenerator.instance.count);
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        //Debug.DrawLine(transform.position, raycastDirection * 10, Color.red, 10);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Road_" + indexRoadCollider) || collision.CompareTag("Road_0"))
        {
            gameObject.SetActive(false);
            ParController.instance.PlayRoadBulletHole(transform.position);
        }
    }
}
