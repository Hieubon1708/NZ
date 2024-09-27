using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void ShotAniEvent()
    {
        return;
        PlayerController.instance.ShotAni();
        StartCoroutine(PlayerController.instance.StartFindTarget());
    }
    
    public void ShotEvent()
    {
        BulletController.instance.SetDefaultBullets();
        BulletController.instance.SetUpShot();
        PlayerController.instance.isFindingTarget = false;
    }
    
    public void BulletAniEvent()
    {
        PlayerController.instance.bulletPar.Play();
        PlayerController.instance.FindTarget();
    }
}
