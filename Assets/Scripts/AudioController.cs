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
    public AudioSource eWalk;

    public AudioClip fly;
    public AudioClip[] attack;
    public AudioClip[] walk;

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
    public AudioClip roll;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip nextMap;

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

    bool isWalkAudio;
    Tween walkAudio;

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
            if (DataManager.instance.dataStorage.isMusicActive) music.DOFade(1f, time2).SetEase(Ease.Linear);
        });
    }

    public void PlaySoundEnemyWalk(AudioSource audioSource, AudioClip audioClip, float time)
    {
        if (walk[GameController.instance.level] != null)
        {
            if (isWalkAudio) return;
            isWalkAudio = true;
            audioSource.DOKill();
            audioSource.volume = 0;
            audioSource.DOKill();
            audioSource.clip = audioClip;
            audioSource.Play();
            if (DataManager.instance.dataStorage.isSoundActive) audioSource.DOFade(1, time).SetEase(Ease.Linear);
            walkAudio = DOVirtual.DelayedCall(Random.Range(7.5f, 9.5f), delegate
            {
                isWalkAudio = false;
                PlaySoundEnemyWalk(audioSource, audioClip, time);
            });
        }
    }

    public void StopSoundEnemyWalk(AudioSource audioSource, float time)
    {
        isWalkAudio = false;
        walkAudio.Kill();
        audioSource.DOFade(0, time).SetEase(Ease.Linear).OnComplete(delegate
        {
            audioSource.Pause();
        });
    }

    public void PlaySound(AudioClip audioClip)
    {
        sound.PlayOneShot(audioClip);
    }

    public void PlaySoundEnemyAttack(AudioSource audioSource, AudioClip audioClip, float time)
    {
        audioSource.volume = 0;
        audioSource.DOKill();
        audioSource.clip = audioClip;
        audioSource.Play();
        if (DataManager.instance.dataStorage.isSoundActive) audioSource.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void StopSoundEnemyAttack(AudioSource audioSource, float time)
    {
        audioSource.DOKill();
        audioSource.DOFade(0, time).SetEase(Ease.Linear).OnComplete(delegate
        {
            audioSource.Stop();
        });
    }

    public void PlaySoundEnemyFly(AudioSource audioSource, AudioClip audioClip, float time)
    {
        audioSource.volume = 0;
        audioSource.DOKill();
        audioSource.clip = audioClip;
        audioSource.Play();
        if (DataManager.instance.dataStorage.isSoundActive) audioSource.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void StopSoundEnemyFly(AudioSource audioSource, float time)
    {
        audioSource.DOKill();
        audioSource.DOFade(0, time).SetEase(Ease.Linear).OnComplete(delegate
        {
            audioSource.Stop();
        });
    }
    public void DisAuEnemy(float time)
    {
        StopSoundEnemyFly(eFly, 0);
    }

    public void PlaySoundWeapon1(AudioClip audioClip, float time)
    {
        weapon1.volume = 0;
        weapon1.DOKill();
        weapon1.clip = audioClip;
        weapon1.Play();
        if (DataManager.instance.dataStorage.isSoundActive) weapon1.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void PlaySoundWeapon2(AudioClip audioClip, float time)
    {
        weapon2.volume = 0;
        weapon2.DOKill();
        weapon2.clip = audioClip;
        weapon2.Play();
        if (DataManager.instance.dataStorage.isSoundActive) weapon2.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void PlaySoundWeapon3(AudioClip audioClip, float time)
    {
        weapon3.volume = 0;
        weapon3.DOKill();
        weapon3.clip = audioClip;
        weapon3.Play();
        if (DataManager.instance.dataStorage.isSoundActive) weapon3.DOFade(1, time).SetEase(Ease.Linear);
    }

    public void StopSoundWeapon(AudioSource audioSource, float time)
    {
        audioSource.DOKill();
        audioSource.DOFade(0, time).SetEase(Ease.Linear).OnComplete(delegate
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
        sound.DOKill();
        sound.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
        eAttack.DOKill();
        eAttack.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
        eFly.DOKill();
        eFly.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
        eWalk.DOKill();
        eWalk.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
    }

    public void EnableMusic(bool isEnable, float time)
    {
        music.DOKill();
        music.DOFade(isEnable ? 1 : 0, time).SetEase(Ease.Linear);
    }
}
