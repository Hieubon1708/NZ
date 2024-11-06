using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    public AudioSource music;
    public AudioSource button;
    public AudioSource boom;
    public AudioSource tower;
    public AudioSource weapon1;
    public AudioSource weapon2;
    public AudioSource weapon3;
    public AudioSource player;

    public AudioClip buttonClick;
    public AudioClip sellEquip;
    public AudioClip boomEquip;
    public AudioClip clothesEquip;
    public AudioClip gunEquip;
    public AudioClip reward;
    public AudioClip convert;
    public AudioClip gemReward;

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
    public AudioClip towerDestroy;

    public void Awake()
    {
        instance = this;
    }

    public void PlayMusic(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isMusicActive) return;
        music.clip = audioClip;
        music.Play();
    }
    
    public void PlaySoundButton(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        button.clip = audioClip;
        button.Play();
    }
    
    public void PlaySoundTower(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        tower.clip = audioClip;
        tower.Play();
    }
    
    public void PlaySoundPlayer(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        player.clip = audioClip;
        player.Play();
    }
    
    public void PlaySoundWeapon1(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        weapon1.clip = audioClip;
        weapon1.Play();
    }
    
    public void PlaySoundWeapon2(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        weapon2.clip = audioClip;
        weapon2.Play();
    }
    
    public void PlaySoundWeapon3(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        weapon3.clip = audioClip;
        weapon3.Play();
    }
    
    public void PlaySoundBoom(AudioClip audioClip)
    {
        if (!DataManager.instance.dataStorage.isSoundActive) return;
        boom.clip = audioClip;
        boom.Play();
    }
}
