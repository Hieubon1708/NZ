using UnityEngine;

public class test : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ok");
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("ok");
    }
}
