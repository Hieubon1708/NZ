using UnityEngine;

public class FlameBooster : WeaponBooster
{
    public override void UseBooster()
    {
        AudioController.instance.PlaySoundWeapon2(AudioController.instance.flameBooster, 0.25f);
        BlockController.instance.UseBooster(GameController.WEAPON.FLAME);
    }
}
