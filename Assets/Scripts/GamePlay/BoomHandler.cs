using DG.Tweening;
using UnityEngine;

public class BoomHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            DOVirtual.DelayedCall(1f, delegate
            {
                gameObject.SetActive(false);
            });
        }
    }
}
