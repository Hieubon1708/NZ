using DG.Tweening;
using UnityEngine;

public class BoomHandler : MonoBehaviour
{
    public CircleCollider2D col;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !col.enabled)
        {
            col.enabled = true;
            DOVirtual.DelayedCall(0.75f, delegate
            {
                AudioController.instance.PlaySoundBoom(AudioController.instance.boomBooster);
                col.enabled = false;
                GameController.instance.ShakeCam(0.25f);
                gameObject.SetActive(false);
                ParController.instance.PlayBoomParticle(gameObject.transform.position);
            });
        }
    }
}
