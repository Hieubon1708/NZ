using DG.Tweening;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource music;
    public AudioSource sound;

    public AudioClip[] bgMenu;
    public AudioClip[] bgIngame;

    public AudioSource weapon1;
    public AudioSource weapon2;
    public AudioSource weapon3;
    public AudioSource eFly;
    public AudioSource eAttack;

    public AudioClip fly;
    public AudioClip[] attack;

    public AudioClip buttonClick;
    public AudioClip sellEquip;
    public AudioClip boomEquip;
    public AudioClip clothesEquip;
    public AudioClip gunEquip;
    public AudioClip reward;
    public AudioClip convert;
    public AudioClip gemReward;
    public AudioClip flyGold;
    public AudioClip flyGoldEnd;

    public AudioClip boomBooster;
    public AudioClip sawBooster;
    public AudioClip shockerBooster;
    public AudioClip flameBooster;
    public AudioClip machineGunBooster;

    public AudioClip saw;
    public AudioClip shocker;
    public AudioClip flame;
    public AudioClip machineGun;

    public AudioClip shot;
    public AudioClip playerDie;
    public AudioClip enemyDie;
    public AudioClip towerDestroy;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMusic(AudioClip audioClip, float time1, float time2)
    {
        music.DOKill();
        music.DOFade(0f, time1).SetEase(Ease.Linear).OnComplete(delegate
        {
            music.clip = audioClip;
            music.Play();
            if (!DataManager.instance.dataStorage.isMusicActive) return;
            music.DOFade(1f, time2).SetEase(Ease.Linear);
        });
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        sound.PlayOneShot(audioClip);
    }

    public void PlaySoundEnemyAttack(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        eAttack.clip = audioClip;
        eAttack.Play();
    }

    public void PlaySoundEnemyFly(float time)
    {
        eFly.volume = 0;
        eFly.DOKill();
        eFly.clip = fly;
        eFly.Play();
        eFly.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void StopSoundEnemyFly(float time)
    {
        eFly.DOKill();
        eFly.DOFade(0, time).SetEase(Ease.Linear).OnComplete(delegate
        {
            eFly.Stop();
        });
    }
    public void DisAuEnemy(float time)
    {
        StopSoundEnemyFly(0);
    }

    public void PlaySoundWeapon1(AudioClip audioClip, float time)
    {
        weapon1.volume = 0;
        weapon1.DOKill();
        weapon1.clip = audioClip;
        weapon1.Play();
        weapon1.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void PlaySoundWeapon2(AudioClip audioClip, float time)
    {
        weapon2.volume = 0;
        weapon2.DOKill();
        weapon2.clip = audioClip;
        weapon2.Play();
        weapon2.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void PlaySoundWeapon3(AudioClip audioClip, float time)
    {
        weapon3.volume = 0;
        weapon3.DOKill();
        weapon3.clip = audioClip;
        weapon3.Play();
        weapon3.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void StopSoundWeapon(AudioSource audioSource, float time)
    {
        audioSource.DOKill();
        weapon1.DOFade(0, time).SetEase(Ease.Linear).OnComplete(delegate
        {
            audioSource.Stop();
        });
    }

    public void EnableSound(bool isEnable, float time)
    {
        weapon1.DOKill();
        weapon1.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
        weapon2.DOKill();
        weapon2.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
        weapon3.DOKill();
        weapon3.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
    }

    public void EnableMusic(bool isEnable, float time)
    {
        music.DOKill();
        music.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
    }
}
