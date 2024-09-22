public class BoomBooster : WeaponBooster
{
    public override void UseBooster()
    {
        PlayerController.instance.ThrowBoom();
    }
}
