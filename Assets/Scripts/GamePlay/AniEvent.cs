using UnityEngine;

public class AniEvent : MonoBehaviour
{
    public void ShotAniEvent()
    {
        if (CarController.instance.multiplier == 0) return;
        PlayerController.instance.ShotAni();
    }
    
    public void ShotEvent()
    {
        BulletController.instance.SetDefaultBullets();
        BulletController.instance.SetUpShot();
    }
    
    public void BulletAniEvent()
    {
        PlayerController.instance.bulletPar.Play();
    }    
}
