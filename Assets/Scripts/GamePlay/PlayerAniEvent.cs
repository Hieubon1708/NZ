using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void ShotAniEvent() {  }
    
    public void ShotEvent()
    {
        if (!GameController.instance.isStart) return;
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
