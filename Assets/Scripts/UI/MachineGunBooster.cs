public class MachineGunBooster : WeaponBooster
{
    public override void UseBooster()
    {
        AudioController.instance.PlaySoundWeapon3(AudioController.instance.machineGunBooster, 0.25f);
        BlockController.instance.UseBooster(GameController.WEAPON.MACHINE_GUN);
    }
}
