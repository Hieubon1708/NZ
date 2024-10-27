using UnityEngine;

public abstract class WeaponBuyButton : ButtonBuyer
{
    public BlockUpgradeHandler blockUpgradeHandler;
    public Block scBlock;
    public Animation ani;

    public virtual void EnableAniShowButton()
    {
        ani.Play();
    }
}
