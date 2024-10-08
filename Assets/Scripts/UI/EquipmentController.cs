using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public GameObject equipmentPrefab;
    public List<EquipmentInfo> equipments = new List<EquipmentInfo>();

    public Transform container;

    public SpriteRenderer cap;
    public SpriteRenderer clothes;
    public SpriteRenderer gun;
    public SpriteRenderer icon;

    public Image panelEquip;
    public RectTransform popupEquip;
    public TextMeshProUGUI currentEquipValue;
    public TextMeshProUGUI equipValue;

    public int indexCap;
    public int indexClothes;
    public int indexGun;
    public int indexBoom;

    public int amoutEquip;

    public Sprite[] bgs;
    public Sprite[] guns;
    public Sprite[] booms;
    public Sprite[] clothess;
    public Sprite[] caps;
    public Sprite damage;
    public Sprite hp;

    public void Awake()
    {
        instance = this;
        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject equip = Instantiate(equipmentPrefab, container);
            equipments.Add(equip.GetComponent<EquipmentInfo>());
            equip.SetActive(false);
        }
    }

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.pLayerDataStorage.equipmentDataStorages != null)
        {
            for (int i = 0; i < DataManager.instance.dataStorage.pLayerDataStorage.equipmentDataStorages.Length; i++)
            {
                if (amoutEquip == equipments.Count - 1) Generate();
                equipments[i].gameObject.SetActive(true);
                EquipmentDataStorage eq = DataManager.instance.dataStorage.pLayerDataStorage.equipmentDataStorages[i];
                SetEquip(eq.type, eq.level, equipments[i]);
                amoutEquip++;
            }
        }
    }

    public void ShowPopupSwap(EQUIPMENTTYPE type)
    {
        if (type == EQUIPMENTTYPE.GUN || type == EQUIPMENTTYPE.BOOM) icon.sprite = damage;
        if (type == EQUIPMENTTYPE.CAP || type == EQUIPMENTTYPE.CLOTHES) icon.sprite = hp;
    }

    void SetEquip(int type, int level, EquipmentInfo equipmentInfo)
    {
        Sprite[] eq = null;
        EQUIPMENTTYPE equipType;

        if (type == 0)
        {
            eq = guns;
            equipType = EQUIPMENTTYPE.GUN;
        }
        else if (type == 1)
        {
            eq = booms;
            equipType = EQUIPMENTTYPE.BOOM;
        }
        else if (type == 2)
        {
            eq = caps;
            equipType = EQUIPMENTTYPE.CAP;
        }
        else
        {
            eq = clothess;
            equipType = EQUIPMENTTYPE.CLOTHES;
        }

        equipmentInfo.eq.sprite = eq[level];
        equipmentInfo.bg.sprite = bgs[level];
        equipmentInfo.SetValue(equipType, GetEquipValue(type, level));
    }

    int GetEquipValue(int type, int level)
    {
        int value = 0;
        int startValue = 0;
        float coef = 0;

        if (type == 0)
        {
            startValue = DataManager.instance.equipmentConfig.gunConfig.startDamage;
            coef = DataManager.instance.equipmentConfig.gunConfig.coef;
        }
        else if (type == 1)
        {
            startValue = DataManager.instance.equipmentConfig.boomConfig.startDamage;
            coef = DataManager.instance.equipmentConfig.boomConfig.coef;
        }
        else if (type == 2)
        {
            startValue = DataManager.instance.equipmentConfig.capConfig.startHp;
            coef = DataManager.instance.equipmentConfig.capConfig.coef;
        }
        else if (type == 3)
        {
            startValue = DataManager.instance.equipmentConfig.clothesConfig.startHp;
            coef = DataManager.instance.equipmentConfig.clothesConfig.coef;
        }
        value = Mathf.RoundToInt(startValue * Mathf.Pow(coef, level));
        return value;
    }

    public enum EQUIPMENTLEVEL
    {
        COMMON, GREATE, EXCELLENT, RARE, UNIQUE, EPIC, LEGENDARY, ETERNAL, SUPREME, MYTHIC, IMMORTAL
    }

    public enum EQUIPMENTTYPE
    {
        GUN, BOOM, CAP, CLOTHES
    }

}
