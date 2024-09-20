using System.Collections;
using UnityEngine;

public class SawHandler : MonoBehaviour
{
    int count;
    public Animation sawAttackAni;
    public ParticleSystem sawBlood;
    Coroutine blood;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            count++;
            sawAttackAni.Play();
            if (blood == null) blood = StartCoroutine(SawBlood());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            count--;
            if (count == 0)
            {
                //blood = null;
                StopCoroutine(blood);
                sawAttackAni.Stop();
            }
        }
    }

    IEnumerator SawBlood()
    {
        while(true)
        {
            sawBlood.Play();
            yield return new WaitForSeconds(0.35f);
        }
    }
}
