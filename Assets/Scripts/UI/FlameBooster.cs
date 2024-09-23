using UnityEngine;

public class FlameBooster : WeaponBooster
{
    public override void UseBooster()
    {
        BlockController.instance.UseBooster(GameController.WEAPON.FLAME);
    }
}
