using UnityEngine;
using UnityEngine.UI;

public class EquipmentInfo : MonoBehaviour
{
    public EquipmentController.EQUIPMENTTYPE type;
    public EquipmentController.EQUIPMENTLEVEL level;

    public Image bg;
    public Image eq;

    public int value;
    public string levelType;

    public void SetValue(EquipmentController.EQUIPMENTTYPE type, EquipmentController.EQUIPMENTLEVEL level, int value)
    {
        this.value = value;
        this.type = type;
        this.level = level;
    }

    public void SetSprite(Sprite bg, Sprite eq)
    {
        this.bg.sprite = bg;
        this.eq.sprite = eq;
    }

    public void ShowPopupSwap()
    {
        EquipmentController.instance.ShowPopupSwap(this);
    }
    
    public void ShowPopupUpgrade()
    {
        EquipmentController.instance.ShowPopupUpgrade(this);
    }

    public string GetNameLevelNType()
    {
        string nameLevel = char.ToUpper(level.ToString()[0]) + level.ToString().Substring(1).ToLower();
        string nameType = GetNameType();

        return nameLevel + " " + nameType;
    }

    public string GetNameType()
    {
        return char.ToUpper(type.ToString()[0]) + type.ToString().Substring(1).ToLower();
    }
}
