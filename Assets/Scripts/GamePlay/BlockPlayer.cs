using UnityEngine;

public class BlockPlayer : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) CarController.instance.amoutCollison++;
        CarController.instance.PlayAudioEnemyAttack();
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) CarController.instance.amoutCollison--;
        if(CarController.instance.amoutCollison == 0) AudioController.instance.eAttack.Stop();
    }
}
