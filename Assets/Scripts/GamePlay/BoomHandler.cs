using DG.Tweening;
using UnityEngine;

public class BoomHandler : MonoBehaviour
{
    bool isOnGround;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isOnGround)
        {
            isOnGround = true;
            DOVirtual.DelayedCall(1f, delegate
            {
                isOnGround = false;
                GameController.instance.ShakeCam(0.25f);
                gameObject.SetActive(false);
                ParController.instance.PlayBoomParticle(gameObject.transform.position);
            });
        }
    }
}
