public class ShockerBooster : WeaponBooster
{
    public override void UseBooster()
    {
        AudioController.instance.PlaySoundWeapon1(AudioController.instance.shockerBooster, 0.25f);
        BlockController.instance.UseBooster(GameController.WEAPON.SHOCKER);
    }
}
