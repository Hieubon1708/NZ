using UnityEngine;

public class SawHandler : MonoBehaviour
{
    int count;
    public Animation sawAttackAni;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        count++;
        sawAttackAni.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        count--;
        if (count == 0)
        {
            sawAttackAni.Stop();
        }
    }
}
