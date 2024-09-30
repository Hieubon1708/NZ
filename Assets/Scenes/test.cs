using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D rb;

    public void Start()
    {
        float g = 10.0f; 
        float xTarget = 3.0f; 
        float yTarget = 3.0f; 
        float angle = 70.0f; 

        float angleRad = angle * Mathf.PI / 180.0f;

        float distance = Vector2.Distance(new Vector2(xTarget, yTarget), transform.position);

        float v0 = Mathf.Sqrt((g * Mathf.Pow(distance, 2)) / (2 * (yTarget - Mathf.Tan(angleRad) * xTarget)));
        rb.velocity = (new Vector2(xTarget, yTarget) - (Vector2)transform.position).normalized * v0;
    }
}
