using UnityEngine;

public class test : MonoBehaviour
{
    private void Start()
    {
        Vector2 target = new Vector2(10f, -1f);
        Transform mouth = transform; // Assuming you are calculating this from the current object's transform
        float angle = 45f; // Set your desired launch angle here

        float distanceX = target.x - mouth.position.x;
        float distanceY = target.y - mouth.position.y - 1f;
        float time = distanceX / (Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Sqrt(distanceX * distanceX * Mathf.Abs(Physics2D.gravity.y) / (2 * distanceX * Mathf.Tan(angle * Mathf.Deg2Rad) + distanceY)));
        float velocityX = distanceX / time;
        float velocityY = (distanceY + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time * time) / time;

        Vector2 velocity = new Vector2(velocityX, velocityY);
    }
}
