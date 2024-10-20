public class ShockerBooster : WeaponBooster
{
    public override void UseBooster()
    {
        BlockController.instance.UseBooster(GameController.WEAPON.SHOCKER);
    }
}
