using TMPro;
using UnityEngine;

public class EquipmentInfo : MonoBehaviour
{
    public EquipmentController.EQUIPMENTTYPE type;
    public EquipmentController.EQUIPMENTLEVEL level;

    public SpriteRenderer bg;
    public SpriteRenderer eq;

    public int value;

    public void SetValue(EquipmentController.EQUIPMENTTYPE type, int value)
    {
        this.value = value;
        this.type = type;
    }

    public void OnClick()
    {

    }
}
