public class SawBooster : WeaponBooster
{
    public override void UseBooster()
    {
        AudioController.instance.PlaySoundWeapon1(AudioController.instance.sawBooster, 0.25f);
        BlockController.instance.UseBooster(GameController.WEAPON.SAW);
    }
}
