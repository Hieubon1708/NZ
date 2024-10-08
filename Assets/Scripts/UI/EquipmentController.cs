using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public int summonQuipmentLevel = 1;

    float[] chances1 = new float[] {  };

    public void Awake()
    {
        instance = this;
    }

    public enum EQUIPMENTLEVEL
    {
        COMMON, GREATE, EXCELLENT, RARE, UNIQUE, EPIC, LEGENDARY, ETERNAL, SUPREME, MYTHIC, IMMORTAL
    }

    public enum EQUIPMENTTYPE
    {
        DAMAGE, HP
    }
}
