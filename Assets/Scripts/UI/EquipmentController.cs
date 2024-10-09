using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public GameObject equipmentPrefab;
    public List<EquipmentInfo> equipments = new List<EquipmentInfo>();
    public EquipmentInfo[] equipMains;

    public Transform container;

    public Image cap;
    public Image clothes;
    public Image gun;
    public Image boom;
    public Image[] iconTypes;

    public Image panelEquip;
    public Image arrowPopup;
    public RectTransform popupEquip;
    public TextMeshProUGUI currentEquipValue;
    public TextMeshProUGUI equipValue;
    public TextMeshProUGUI currentLevelType;
    public TextMeshProUGUI levelType;
    public Image currentEquipBg;
    public Image equipBg;
    public Image currentEquip;
    public Image equip;
    public TextMeshProUGUI[] damageOrHealthTypes;
    public TextMeshProUGUI textDamage;
    public TextMeshProUGUI textHealth;

    public Image panelUpgradeEquip;
    public RectTransform popupUpgradeEquip;
    public TextMeshProUGUI currentValue;
    public TextMeshProUGUI nextValue;
    public TextMeshProUGUI damageOrHealthUpgrade;
    public TextMeshProUGUI nameTypeUpgrade;
    public Image equipUpgradeBg;
    public Image equipUpgrade;
    public Image iconUpgrade;
    public TextMeshProUGUI currentLevelUpgrade;
    public TextMeshProUGUI currentslotUpgrade;
    public TextMeshProUGUI contentUpgrade;
    public TextMeshProUGUI[] equipCurrentLevels;

    public int capLevel;
    public int clothesLevel;
    public int gunLevel;
    public int boomLevel;

    public int capLevelUpgrade;
    public int clothesLevelUpgrade;
    public int gunLevelUpgrade;
    public int boomLevelUpgrade;

    public EquipmentInfo equipMain;
    public EquipmentInfo equipSelected;
    public EquipmentInfo equipUpgradeSelected;

    public int amoutEquip;

    public Sprite[] bgs;
    public Sprite[] guns;
    public Sprite[] booms;
    public Sprite[] clothess;
    public Sprite[] caps;
    public Sprite damage;
    public Sprite hp;
    public Sprite upArrow;
    public Sprite downArrow;

    public Color upColor;
    public Color downColor;
    public Color designNDushNok;
    public Color[] colorEquipLevels;

    public void Awake()
    {
        instance = this;
        Generate();
    }

    public void Start()
    {
        LoadData();
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
        gunLevel = DataManager.instance.dataStorage.pLayerDataStorage.gunLevel;
        boomLevel = DataManager.instance.dataStorage.pLayerDataStorage.boomLevel;
        capLevel = DataManager.instance.dataStorage.pLayerDataStorage.capLevel;
        clothesLevel = DataManager.instance.dataStorage.pLayerDataStorage.clothesLevel;

        gunLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.gunLevelUpgrade;
        boomLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.boomLevelUpgrade;
        capLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.capLevelUpgrade;
        clothesLevelUpgrade = DataManager.instance.dataStorage.pLayerDataStorage.equipmentUpgradeDataStorages.clothesLevelUpgrade;

        for (int i = 0; i < DataManager.instance.dataStorage.pLayerDataStorage.equipmentDataStorages.Length; i++)
        {
            if (amoutEquip == equipments.Count - 1) Generate();
            equipments[i].gameObject.SetActive(true);
            EquipmentDataStorage eq = DataManager.instance.dataStorage.pLayerDataStorage.equipmentDataStorages[i];
            SetEquip(eq.type, eq.level, equipments[i]);
            amoutEquip++;
        }

        for (int i = 0; i < equipMains.Length; i++)
        {
            int level = 0;

            if (i == 0) level = gunLevel;
            else if (i == 1) level = boomLevel;
            else if (i == 2) level = capLevel;
            else level = clothesLevel;

            SetEquip(i, level, equipMains[i]);
            equipCurrentLevels[i].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(GetLevelUpgrade(equipMains[i].type) + 1);
        }

        UpdateDamage();
        UpdateHealth();
    }

    void UpdateDamage()
    {
        int damage = 0;
        for (int i = 0; i < equipMains.Length; i++)
        {
            if (equipMains[i].type == EQUIPMENTTYPE.SHOTGUN || equipMains[i].type == EQUIPMENTTYPE.GRENADE)
            {
                damage += equipMains[i].value;
            }
        }
        textDamage.text = UIHandler.instance.ConvertNumberAbbreviation(damage);
    }

    void SetSpritePlayer(EQUIPMENTTYPE type, Sprite sprite)
    {
        if (type == EQUIPMENTTYPE.SHOTGUN) gun.sprite = sprite;
        else if (type == EQUIPMENTTYPE.GRENADE) boom.sprite = sprite;
        else if (type == EQUIPMENTTYPE.CAP) cap.sprite = sprite;
        else if (type == EQUIPMENTTYPE.ARMOR) clothes.sprite = sprite;
    }

    void UpdateHealth()
    {
        int health = 0;
        for (int i = 0; i < equipMains.Length; i++)
        {
            if (equipMains[i].type == EQUIPMENTTYPE.CAP || equipMains[i].type == EQUIPMENTTYPE.ARMOR)
            {
                health += equipMains[i].value;
            }
        }
        textHealth.text = UIHandler.instance.ConvertNumberAbbreviation(health);
    }

    void ChangeSpireIcon(Sprite sprite)
    {
        for (int i = 0; i < iconTypes.Length; i++)
        {
            iconTypes[i].sprite = sprite;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }

    void ClassSort()
    {
        for (int i = 0; i < amoutEquip - 1; i++)
        {
            for (int j = i; j < amoutEquip; j++)
            {
                if (equipments[i].type == equipments[j].type)
                {
                    if (equipments[i].level < equipments[j].level)
                    {
                        SwapEquip(equipments[i], equipments[j]);
                    }
                }
                else if (equipments[i].type > equipments[j].type)
                {
                    SwapEquip(equipments[i], equipments[j]);
                }
            }
        }
    }

    void QualitySort()
    {
        for (int i = 0; i < amoutEquip - 1; i++)
        {
            for (int j = i; j < amoutEquip; j++)
            {
                if (equipments[i].level < equipments[j].level)
                {
                    SwapEquip(equipments[i], equipments[j]);
                }
                else if (equipments[i].level == equipments[j].level)
                {
                    if (equipments[i].type > equipments[j].type)
                    {
                        SwapEquip(equipments[i], equipments[j]);
                    }
                }
            }
        }
    }

    void ChangeTextType(string text)
    {
        for (int i = 0; i < damageOrHealthTypes.Length; i++)
        {
            damageOrHealthTypes[i].text = text;
        }
    }

    public void EquipAccept()
    {
        SwapEquip(equipMain, equipSelected);

        if (equipMain.type == EQUIPMENTTYPE.SHOTGUN || equipMain.type == EQUIPMENTTYPE.GRENADE) UpdateDamage();
        else UpdateHealth();

        HidePopupSwap();
    }

    void SwapEquip(EquipmentInfo eq1, EquipmentInfo eq2)
    {
        EQUIPMENTLEVEL tempLevel = eq1.level;
        EQUIPMENTTYPE tempType = eq1.type;
        Sprite tempBg = eq1.bg.sprite;
        Sprite tempEq = eq1.eq.sprite;
        int tempValue = eq1.value;

        eq1.SetSprite(eq2.bg.sprite, eq2.eq.sprite);
        eq1.SetValue(eq2.type, eq2.level, eq2.value);

        eq2.SetSprite(tempBg, tempEq);
        eq2.SetValue(tempType, tempLevel, tempValue);
    }

    public void ShowPopupUpgrade(EquipmentInfo eq)
    {
        equipUpgradeSelected = eq;

        equipUpgradeBg.sprite = eq.bg.sprite;
        equipUpgrade.sprite = eq.eq.sprite;
        currentLevelUpgrade.text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(GetLevelUpgrade(eq.type));
        currentslotUpgrade.text = eq.GetNameType() + " slot";
        currentValue.text = UIHandler.instance.ConvertNumberAbbreviation(eq.value);
        nextValue.text = UIHandler.instance.ConvertNumberAbbreviation(GetEquipValue(eq.type, (int)eq.level, GetLevelUpgrade(eq.type) + 1));
        nextValue.color = upColor;
        nameTypeUpgrade.text = eq.GetNameLevelNType();

        if (eq.type == EQUIPMENTTYPE.SHOTGUN)
        {
            iconUpgrade.sprite = damage;
            damageOrHealthUpgrade.text = "Attack";
            contentUpgrade.text = "Fires 6 bullets at the nearest enemy with average accuracy";

        }
        else if (eq.type == EQUIPMENTTYPE.GRENADE)
        {
            iconUpgrade.sprite = damage;
            damageOrHealthUpgrade.text = "Attack";
            contentUpgrade.text = "Deals area damage 2 seconds after launch";
        }
        else if (eq.type == EQUIPMENTTYPE.CAP)
        {
            iconUpgrade.sprite = hp;
            damageOrHealthUpgrade.text = "Health";
            contentUpgrade.text = "Will protect you from any attack, or maybe not";
        }
        else
        {
            iconUpgrade.sprite = hp;
            damageOrHealthUpgrade.text = "Health";
            contentUpgrade.text = "Casual apocalypse outfit";
        }

        panelUpgradeEquip.gameObject.SetActive(true);
        UIEffect.instance.ScalePopup(panelUpgradeEquip, popupUpgradeEquip, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    void UpdateLevelUpgrade()
    {

    }

    int GetLevelUpgrade(EQUIPMENTTYPE type)
    {
        if (type == EQUIPMENTTYPE.SHOTGUN) return gunLevelUpgrade;
        else if (type == EQUIPMENTTYPE.GRENADE) return boomLevelUpgrade;
        else if (type == EQUIPMENTTYPE.CAP) return capLevelUpgrade;
        else return clothesLevelUpgrade;
    }

    public void HidePopupUpgrade()
    {
        UIEffect.instance.ScalePopup(panelUpgradeEquip, popupUpgradeEquip, 0f, 0f, 0.8f, 0f);
        panelUpgradeEquip.gameObject.SetActive(false);
    }

    public void ShowPopupSwap(EquipmentInfo eq)
    {
        equipSelected = eq;

        if (eq.type == EQUIPMENTTYPE.SHOTGUN || eq.type == EQUIPMENTTYPE.GRENADE)
        {
            ChangeSpireIcon(damage);
            ChangeTextType("Attack");
        }
        else
        {
            ChangeSpireIcon(hp);
            ChangeTextType("Health");
        }

        equipMain = GetEquipMain(eq.type);

        currentEquipValue.text = UIHandler.instance.ConvertNumberAbbreviation(equipMain.value);
        equipValue.text = UIHandler.instance.ConvertNumberAbbreviation(eq.value);
        currentEquipBg.sprite = equipMain.bg.sprite;
        equipBg.sprite = eq.bg.sprite;
        currentLevelType.text = equipMain.GetNameLevelNType();
        levelType.text = eq.GetNameLevelNType();
        currentEquip.sprite = equipMain.eq.sprite;
        equip.sprite = eq.eq.sprite;

        if (equipMain.value > eq.value)
        {
            if (!arrowPopup.enabled) arrowPopup.enabled = true;
            arrowPopup.sprite = downArrow;
            equipValue.color = downColor;
        }
        else if (equipMain.value < eq.value)
        {
            if (!arrowPopup.enabled) arrowPopup.enabled = true;
            arrowPopup.sprite = upArrow;
            equipValue.color = upColor;
        }
        else
        {
            arrowPopup.enabled = false;
            equipValue.color = Vector4.one;
        }

        panelEquip.gameObject.SetActive(true);
        UIEffect.instance.ScalePopup(panelEquip, popupEquip, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HidePopupSwap()
    {
        UIEffect.instance.ScalePopup(panelEquip, popupEquip, 0f, 0f, 0.8f, 0f);
        panelEquip.gameObject.SetActive(false);
    }

    EquipmentInfo GetEquipMain(EQUIPMENTTYPE type)
    {
        for (int i = 0; i < equipMains.Length; i++)
        {
            if (equipMains[i].type == type)
            {
                return equipMains[i];
            }
        }
        return null;
    }

    void SetEquip(int type, int level, EquipmentInfo equipmentInfo)
    {
        Sprite[] eq = null;
        EQUIPMENTTYPE equipType = GetType(type);
        EQUIPMENTLEVEL equipLevel = GetLevel(level);

        if (type == 0)
        {
            eq = guns;
        }
        else if (type == 1)
        {
            eq = booms;
        }
        else if (type == 2)
        {
            eq = caps;
        }
        else
        {
            eq = clothess;
        }

        equipmentInfo.SetSprite(bgs[level], eq[level]);
        equipmentInfo.SetValue(equipType, equipLevel, GetEquipValue(equipType, level, GetLevelUpgrade(equipType)));
    }

    EQUIPMENTTYPE GetType(int type)
    {
        if (type == 0)
        {
            return EQUIPMENTTYPE.SHOTGUN;
        }
        else if (type == 1)
        {
            return EQUIPMENTTYPE.GRENADE;
        }
        else if (type == 2)
        {
            return EQUIPMENTTYPE.CAP;
        }
        else
        {
            return EQUIPMENTTYPE.ARMOR;
        }
    }

    EQUIPMENTLEVEL GetLevel(int level)
    {
        if (level == 0)
        {
            return EQUIPMENTLEVEL.COMMON;
        }
        else if (level == 1)
        {
            return EQUIPMENTLEVEL.GREATE;
        }
        else if (level == 2)
        {
            return EQUIPMENTLEVEL.EXCELLENT;
        }
        else if (level == 3)
        {
            return EQUIPMENTLEVEL.RARE;
        }
        else if (level == 4)
        {
            return EQUIPMENTLEVEL.UNIQUE;
        }
        else if (level == 5)
        {
            return EQUIPMENTLEVEL.EPIC;
        }
        else if (level == 6)
        {
            return EQUIPMENTLEVEL.LEGENDARY;
        }
        else
        {
            return EQUIPMENTLEVEL.ETERNAL;
        }
    }

    int GetEquipValue(EQUIPMENTTYPE type, int level, int levelUpgrade)
    {
        int value = 0;
        int startValue = 0;
        float coefByLevel = 0;
        float coefByRarity = 0;

        if (type == EQUIPMENTTYPE.SHOTGUN)
        {
            startValue = DataManager.instance.equipmentConfig.gunConfig.startDamage;
            coefByLevel = DataManager.instance.equipmentConfig.gunConfig.coefBylevel;
            coefByRarity = DataManager.instance.equipmentConfig.gunConfig.coefByRarity;
        }
        else if (type == EQUIPMENTTYPE.GRENADE)
        {
            startValue = DataManager.instance.equipmentConfig.boomConfig.startDamage;
            coefByLevel = DataManager.instance.equipmentConfig.boomConfig.coefBylevel;
            coefByRarity = DataManager.instance.equipmentConfig.boomConfig.coefByRarity;
        }
        else if (type == EQUIPMENTTYPE.CAP)
        {
            startValue = DataManager.instance.equipmentConfig.capConfig.startHp;
            coefByLevel = DataManager.instance.equipmentConfig.capConfig.coefBylevel;
            coefByRarity = DataManager.instance.equipmentConfig.capConfig.coefByRarity;
        }
        else if (type == EQUIPMENTTYPE.ARMOR)
        {
            startValue = DataManager.instance.equipmentConfig.clothesConfig.startHp;
            coefByLevel = DataManager.instance.equipmentConfig.clothesConfig.coefBylevel;
            coefByRarity = DataManager.instance.equipmentConfig.clothesConfig.coefByRarity;
        }
        value = Mathf.RoundToInt(startValue * Mathf.Pow(coefByRarity, level) * Mathf.Pow(coefByLevel, levelUpgrade));
        return value;
    }

    public enum EQUIPMENTLEVEL
    {
        COMMON, GREATE, EXCELLENT, RARE, UNIQUE, EPIC, LEGENDARY, ETERNAL
    }

    public enum EQUIPMENTTYPE
    {
        SHOTGUN, GRENADE, CAP, ARMOR
    }

}
