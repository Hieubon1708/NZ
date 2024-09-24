using UnityEngine;

public class test : MonoBehaviour
{
    public Rigidbody2D rb;

    public void Start()
    {
        rb.AddForce (new Vector2 (1, 10), ForceMode2D.Impulse);
        rb.AddTorque(-1f);
    }

    private void FixedUpdate()
    {
    }
}
