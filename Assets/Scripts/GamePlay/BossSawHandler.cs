using System.Collections;
using UnityEngine;

public class BossSawHandler : MonoBehaviour
{
    public Animation chainSaw;
    public ParticleSystem spark;
    Coroutine corSpark;
    int count;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Block"))
        {
            if (count == 0)
            {
                corSpark = StartCoroutine(PlaySpark());
                chainSaw.Play();
            }
            count++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Block"))
        {
            count--;
            if (count == 0)
            {
                chainSaw.Stop();
                if (corSpark != null)
                {
                    StopCoroutine(corSpark);
                }
            }
        }
    }

    IEnumerator PlaySpark()
    {
        while (true)
        {
            spark.Play();
            yield return new WaitForSeconds(0.15f);
        }
    }
}
