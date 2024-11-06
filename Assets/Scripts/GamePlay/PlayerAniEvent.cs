using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void ShotAniEvent() {  }
    
    public void ShotEvent()
    {
        if (!GameController.instance.isStart) return;
        AudioController.instance.PlaySound(AudioController.instance.shot);
        BulletController.instance.SetDefaultBullets();
        BulletController.instance.SetUpShot();
        PlayerController.instance.isFindingTarget = false;
    }
    
    public void BulletAniEvent()
    {
        if(!GameController.instance.isStart) return; 
        PlayerController.instance.bulletPar.Play();
        PlayerController.instance.FindTarget();
    }
}
