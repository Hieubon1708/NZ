using UnityEngine;

public class MachineGunBuyHandler : WeaponBuyButton
{
    public override void Buy()
    {
        blockUpgradeHandler.BuyWeapon(GameController.WEAPON.MACHINE_GUN, 0);
        scBlock.PlusGold(DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN));
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN).ToString();
    }

    public override void CheckButtonState()
    {
        if (DataManager.instance.playerData.gold < DataManager.instance.GetPriceWeaponConfig(GameController.WEAPON.MACHINE_GUN)) UIHandler.instance.ChangeSpriteWeaponBuyer(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, frameGold);
        else UIHandler.instance.ChangeSpriteWeaponBuyer(UIHandler.Type.ENOUGH_MONEY, frameButton, frameGold);
    }
}
