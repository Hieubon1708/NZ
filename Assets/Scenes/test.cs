using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D rb;
    Vector2 anglePrevious;

    public void Start()
    {
        rb.AddForce(new Vector2(-1, 15), ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if(anglePrevious != null)
        {
            Vector2 dir = anglePrevious - (Vector2)transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, EUtils.GetAngle(dir)), 0.05f);
        }
        anglePrevious = transform.position;
    }
}
