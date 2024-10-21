using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void ShotAniEvent()
    {
        return;
        if(!GameController.instance.isStart) return;
        StartCoroutine(PlayerController.instance.StartFindTarget());
    }
    
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
