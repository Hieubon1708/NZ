using UnityEngine;

public class PlayerAniEvent : MonoBehaviour
{
    public void ShotAniEvent()
    {
        if(!GameController.instance.isStart || !UIHandler.instance.tutorial.isFirstTimePlay) return;
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
