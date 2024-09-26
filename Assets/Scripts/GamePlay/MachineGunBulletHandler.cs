using UnityEngine;

public class MachineGunBulletHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    int indexRoadCollider;
    public bool isGunBooster;

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
            if(!isGunBooster) ParController.instance.PlayRoadBulletHoleParticle(transform.position);
            else ParController.instance.PlayGunHitOnRoadParticle(transform.position);
        }
    }
}
