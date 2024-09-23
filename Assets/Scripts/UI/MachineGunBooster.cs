public class MachineGunBooster : WeaponBooster
{
    public override void UseBooster()
    {
        BlockController.instance.UseBooster(GameController.WEAPON.MACHINE_GUN);
    }
}
