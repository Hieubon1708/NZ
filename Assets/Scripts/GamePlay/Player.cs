using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;
    public int gold;

    public PlayerSkiner playerSkiner;

    public int SubtractHp(int hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        return this.hp;
    }

    public void SubtractGold(int gold)
    {
        this.gold -= gold;
    }
    
    public void PlusGold(int gold)
    {
        this.gold += gold;
    }

    public void LoadData()
    {
        gold = DataManager.instance.playerDataStorage.gold;
        HpChange();
    }

    public void HpChange()
    {
        int capHp = EquipmentController.instance.GetEquipValue(EquipmentController.EQUIPMENTTYPE.CAP, EquipmentController.instance.playerInventory.capLevel, EquipmentController.instance.playerInventory.capLevelUpgrade);
        int clothesHp = EquipmentController.instance.GetEquipValue(EquipmentController.EQUIPMENTTYPE.ARMOR, EquipmentController.instance.playerInventory.clothesLevel, EquipmentController.instance.playerInventory.clothesLevelUpgrade);
        hp = capHp + clothesHp;
    }
}
