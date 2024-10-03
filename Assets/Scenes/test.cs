using UnityEngine;

public class test : MonoBehaviour
{
    public bool isDrop;
    float time;
    public float speed;
    public float angleTarget;
    public bool firstPush;

    public void FixedUpdate()
    {
        if (isDrop)
        {
            time += Time.fixedDeltaTime;
            transform.Translate(Vector2.up * time * speed);
            if (!firstPush) transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, angleTarget), 0.1f);
            if (firstPush) transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 0.05f);
            if (transform.localEulerAngles.z >= angleTarget * 0.75f) firstPush = true;
        }
    }

    public void Restart()
    {
        time = 0;
        isDrop = false;
        firstPush = false;
    }
}
