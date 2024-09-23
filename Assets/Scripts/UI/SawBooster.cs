public class SawBooster : WeaponBooster
{
    public override void UseBooster()
    {
        BlockController.instance.UseBooster(GameController.WEAPON.SAW);
    }
}
