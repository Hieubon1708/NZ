using UnityEngine;

public class Block : MonoBehaviour
{
    public int level;
    public int hp;
    public Animation ani;
    public Canvas canvas;
    public int sellingPrice;
    public BlockHandler blockHandler;
    public BlockUpgradeHandler blockUpgradeHandler;
    public SpriteRenderer sp;

    public AnimationClip[] animationClips = new AnimationClip[3];
    public ParticleSystem addBlock;

    public void Awake()
    {
        animationClips[0] = ani.GetClip("addBlock");
        animationClips[1] = ani.GetClip("destroyBlock");
        animationClips[2] = ani.GetClip("upgradeBlock");
    }

    public int SubtractHp(int hp)
    {
        this.hp -= hp;
        if(this.hp < 0) this.hp = 0;
        return this.hp;
    }

    public void PlusGold(int gold)
    {
        sellingPrice += gold;
        PlayerController.instance.player.gold -= gold;
        UIHandler.instance.GoldUpdatee();
    }

    public void SubtractGold()
    {
        UIHandler.instance.FlyGoldMenu(transform.position);
        PlayerController.instance.player.gold += sellingPrice;
        sellingPrice = 0;
        UIHandler.instance.GoldUpdatee();
    }

    public void AddBlockAni()
    {
        ani.clip = animationClips[0];
        ani.Play();
    }

    public void StopDeleteBlockAni()
    {
        ani.clip = animationClips[1];
        ani.Stop();
    }

    public void DeleteBlockAni()
    {
        ani.clip = animationClips[1];
        ani.Play();
    }

    public void UpgradeBlockAni()
    {
        if(ani.isPlaying) ani.Stop();
        ani.clip = animationClips[2];
        ani.Play();
    }

    public void AddBlockParEvent()
    {
        addBlock.Play();
    }
}
