using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D rb;

    public float angle = 60;
    public Vector2 start = new Vector2(0f, 0f);
    public Vector2 target = new Vector2(10f, 3f);
    public float gravity = 9.81f;

    void Start()
    {
        float distanceX = target.x - start.x;
        float distanceY = target.y - start.y;
        float time = distanceX / (Mathf.Cos(angle * Mathf.Deg2Rad) * (Mathf.Sqrt((distanceX * distanceX * gravity) / (2 * distanceX * Mathf.Tan(angle * Mathf.Deg2Rad) + distanceY))));
        float velocityX = distanceX / time;
        float velocityY = (distanceY + 0.5f * gravity * time * time) / time;

        Vector2 velocity = new Vector2(velocityX, velocityY);
        rb.velocity = velocity;
        Debug.Log("Velocity: " + velocity);
    }
}
