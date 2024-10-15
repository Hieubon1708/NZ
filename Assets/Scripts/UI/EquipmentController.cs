using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public PlayerInventory playerInventory;

    public GameObject equipmentPrefab;
    public List<EquipmentInfo> equipments = new List<EquipmentInfo>();
    public EquipmentInfo[] equipMains;

    public Transform container;

    public Image cap;
    public Image clothes;
    public Image gun;
    public Image boom;
    public Image[] iconTypes;

    public Image iconDesign;
    public Sprite[] iconDesigns;

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
    public TextMeshProUGUI textGunDesign;
    public TextMeshProUGUI textBoomDesign;
    public TextMeshProUGUI textCapDesign;
    public TextMeshProUGUI textClothesDesign;
    public GameObject gunDesign;
    public GameObject boomDesign;
    public GameObject capDesign;
    public GameObject clothesDesign;
    public TextMeshProUGUI dush;
    public TextMeshProUGUI desgin;
    public TextMeshProUGUI[] equipCurrentLevels;

    public EquipmentInfo equipMain;
    public EquipmentInfo equipSelected;
    public EquipmentInfo equipUpgradeSelected;

    public int amoutEquip;
    public int dushSelected;
    public int designSelected;

    public Sprite[] bgs;
    public Sprite[] guns;
    public Sprite[] booms;
    public Sprite[] clothess;
    public Sprite[] caps;
    public Sprite damage;
    public Sprite hp;
    public Sprite upArrow;
    public Sprite downArrow;
    public Sprite buttonOk;
    public Sprite buttonNok;

    public Image maxLevel;
    public Image levelUp;

    public Color upColor;
    public Color downColor;
    public Color[] colorEquipLevels;

    bool isQuality = true;
    public TextMeshProUGUI textQualityNClass;

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
        playerInventory.LoadData();

        textGunDesign.text = UIHandler.instance.ConvertNumberAbbreviation(playerInventory.amoutGunDesign);
        textBoomDesign.text = UIHandler.instance.ConvertNumberAbbreviation(playerInventory.amoutBoomDesign);
        textCapDesign.text = UIHandler.instance.ConvertNumberAbbreviation(playerInventory.amoutCapDesign);
        textClothesDesign.text = UIHandler.instance.ConvertNumberAbbreviation(playerInventory.amoutClothesDesign);

        if (DataManager.instance.dataStorage.playerDataStorage != null)
        {
            for (int i = 0; i < DataManager.instance.dataStorage.playerDataStorage.equipmentDataStorages.Length; i++)
            {
                //if (amoutEquip == equipments.Count - 1) Generate();
                equipments[i].gameObject.SetActive(true);
                EquipmentDataStorage eq = DataManager.instance.dataStorage.playerDataStorage.equipmentDataStorages[i];
                SetEquip(eq.type, eq.level, equipments[i]);
                amoutEquip++;
            }
        }

        for (int i = 0; i < equipMains.Length; i++)
        {
            int level = GetIndexLevel(i);
            SetEquip(i, level, equipMains[i]);
            equipCurrentLevels[i].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(GetLevelUpgrade(equipMains[i].type) + 1);
        }

        UpdateDamage();
        UpdateHealth();
        QualitySort();
    }

    int GetIndexLevel(int index)
    {
        if (index == 0) return playerInventory.gunLevel;
        else if (index == 1) return playerInventory.boomLevel;
        else if (index == 2) return playerInventory.capLevel;
        else return playerInventory.clothesLevel;
    }

    void CheckDisplayDesign()
    {
        if (playerInventory.amoutClothesDesign == 0) clothesDesign.SetActive(false);
        else clothesDesign.SetActive(true);
        if (playerInventory.amoutCapDesign == 0) capDesign.SetActive(false);
        else capDesign.SetActive(true);
        if (playerInventory.amoutBoomDesign == 0) boomDesign.SetActive(false);
        else boomDesign.SetActive(true);
        if (playerInventory.amoutGunDesign == 0) gunDesign.SetActive(false);
        else gunDesign.SetActive(true);
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
            iconTypes[i].SetNativeSize();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }

    public void QualityNClass()
    {
        isQuality = !isQuality;

        textQualityNClass.text = isQuality ? "Quality" : "Class";

        if (isQuality) QualitySort();
        else ClassSort();
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
                        SwapValueEquip(equipments[i], equipments[j]);
                    }
                }
                else if (equipments[i].type > equipments[j].type)
                {
                    SwapValueEquip(equipments[i], equipments[j]);
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
                    SwapValueEquip(equipments[i], equipments[j]);
                }
                else if (equipments[i].level == equipments[j].level)
                {
                    if (equipments[i].type > equipments[j].type)
                    {
                        SwapValueEquip(equipments[i], equipments[j]);
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

        HidePopupSwap();
    }

    public void UpgradeAccept()
    {
        Upgrade();
    }

    void Upgrade()
    {
        SubtractDesgin(equipUpgradeSelected.type, designSelected);
        playerInventory.dush -= dushSelected;
        if (equipUpgradeSelected.type == EQUIPMENTTYPE.SHOTGUN)
        {
            int damage = GetEquipValue(EQUIPMENTTYPE.SHOTGUN, playerInventory.gunLevel, ++playerInventory.gunLevelUpgrade);
            equipUpgradeSelected.value = damage;
            UpdateDamage();
            BulletController.instance.SetDamage(damage);
            equipCurrentLevels[0].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(playerInventory.gunLevelUpgrade + 1);
        }
        else if (equipUpgradeSelected.type == EQUIPMENTTYPE.GRENADE)
        {
            int damage = GetEquipValue(EQUIPMENTTYPE.GRENADE, playerInventory.boomLevel, ++playerInventory.boomLevelUpgrade);
            UpdateDamage();
            PlayerController.instance.BoomSetDamage(damage);
            equipCurrentLevels[1].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(playerInventory.gunLevelUpgrade + 1);
        }
        else if (equipUpgradeSelected.type == EQUIPMENTTYPE.CAP)
        {
            GetEquipValue(EQUIPMENTTYPE.CAP, playerInventory.capLevel, ++playerInventory.capLevelUpgrade);
            UpdateHealth();
            PlayerController.instance.player.HpChange();
            equipCurrentLevels[2].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(playerInventory.gunLevelUpgrade + 1);
        }
        else
        {
            GetEquipValue(EQUIPMENTTYPE.ARMOR, playerInventory.clothesLevel, ++playerInventory.clothesLevelUpgrade);
            UpdateHealth();
            PlayerController.instance.player.HpChange();
            equipCurrentLevels[3].text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(playerInventory.gunLevelUpgrade + 1);
        }
        UpdateInfoUpgrade(equipUpgradeSelected);
        CheckStateUpgrade(equipUpgradeSelected);
    }

    void UpdateInfoUpgrade(EquipmentInfo eq)
    {
        currentLevelUpgrade.text = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(GetLevelUpgrade(eq.type) + 1);
        currentValue.text = UIHandler.instance.ConvertNumberAbbreviation(eq.value);
        nextValue.text = UIHandler.instance.ConvertNumberAbbreviation(GetEquipValue(eq.type, (int)eq.level, GetLevelUpgrade(eq.type) + 1));
    }

    void CheckStateUpgrade(EquipmentInfo eq)
    {
        bool isNok = false;

        int levelUpgrade = GetLevelUpgrade(eq.type);
        dushSelected = GetDush(levelUpgrade);
        designSelected = GetDesign(levelUpgrade);

        int amoutDesgin = GetAmoutDesign(eq.type);

        if (playerInventory.dush < dushSelected)
        {
            isNok = true;
            dush.text = "<color=red>" + playerInventory.dush + "</color>" + "/" + dushSelected;
        }
        else
        {
            dush.color = Vector4.one;
            dush.text = playerInventory.dush + "/" + dushSelected;
        }

        if (amoutDesgin < designSelected)
        {
            isNok = true;
            desgin.text = "<color=red>" + amoutDesgin + "</color>" + "/" + designSelected;
        }
        else
        {
            desgin.text = amoutDesgin + "/" + designSelected;
        }

        if (isNok)
        {
            levelUp.sprite = buttonNok;
            maxLevel.sprite = buttonNok;
            levelUp.raycastTarget = false;
            maxLevel.raycastTarget = false;

        }
        else
        {
            levelUp.sprite = buttonOk;
            maxLevel.sprite = buttonOk;
            levelUp.raycastTarget = true;
            maxLevel.raycastTarget = true;
        }
    }

    public void UpradeMaxAccept()
    {
        while (GetAmoutDesign(equipUpgradeSelected.type) >= designSelected && playerInventory.dush >= dushSelected)
        {
            Upgrade();
        }
    }

    void CheckStateEquipBest()
    {

    }

    void CheckStateSellDuplicate()
    {

    }

    void SwapEquip(EquipmentInfo eq1, EquipmentInfo eq2)
    {
        SwapValueEquip(eq1, eq2);
        if (eq1.type == EQUIPMENTTYPE.SHOTGUN)
        {
            UpdateDamage();
            playerInventory.gunLevel = (int)eq1.level;
            PlayerController.instance.player.playerSkiner.GunChange();
            BulletController.instance.SetDamage(GetEquipValue(EQUIPMENTTYPE.SHOTGUN, playerInventory.gunLevel, ++playerInventory.gunLevelUpgrade));
        }
        else if (eq1.type == EQUIPMENTTYPE.GRENADE)
        {
            UpdateDamage();
            playerInventory.boomLevel = (int)eq1.level;
            PlayerController.instance.BoomChange();
            PlayerController.instance.BoomSetDamage(GetEquipValue(EQUIPMENTTYPE.GRENADE, playerInventory.boomLevel, playerInventory.boomLevelUpgrade));
        }
        else if (eq1.type == EQUIPMENTTYPE.CAP)
        {
            UpdateHealth();
            playerInventory.capLevel = (int)eq1.level;
            PlayerController.instance.player.playerSkiner.CapChange();
            PlayerController.instance.player.HpChange();
        }
        else
        {
            UpdateHealth();
            playerInventory.clothesLevel = (int)eq1.level;
            PlayerController.instance.player.playerSkiner.ClothesChange();
            PlayerController.instance.player.HpChange();
        }

        if (isQuality) QualitySort();
        else ClassSort();
    }

    void SwapValueEquip(EquipmentInfo eq1, EquipmentInfo eq2)
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

    public void SellDuplicates()
    {
        int length = amoutEquip;
        for (int i = 0; i < length; i++)
        {
            if (equipments[i].gameObject.activeSelf)
            {
                if ((int)equipments[i].level <= GetIndexLevel((int)equipments[i].type))
                {
                    equipments[i].gameObject.SetActive(false);
                    amoutEquip--;
                }
                else
                {
                    for (int j = i + 1; j < length; j++)
                    {
                        if (equipments[j].gameObject.activeSelf && (int)equipments[i].level == (int)equipments[j].level && (int)equipments[i].type == (int)equipments[j].type)
                        {
                            equipments[j].gameObject.SetActive(false);
                            amoutEquip--;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < length; i++)
        {
            if (equipments[i].gameObject.activeSelf)
            {
                for (int j = 0; j < length; j++)
                {
                    if (!equipments[j].gameObject.activeSelf)
                    {
                        SwapValueEquip(equipments[i], equipments[j]);
                        equipments[j].gameObject.SetActive(true);
                        equipments[i].gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    public void EquipBest()
    {
        for (int i = 0; i < equipMains.Length; i++)
        {
            for (int j = 0; j < amoutEquip; j++)
            {
                if (equipMains[i].type == equipments[j].type && equipMains[i].level < equipments[j].level)
                {
                    SwapEquip(equipMains[i], equipments[j]);
                }
            }
        }
    }

    public void ShowPopupUpgrade(EquipmentInfo eq)
    {
        equipUpgradeSelected = eq;

        equipUpgradeBg.sprite = eq.bg.sprite;
        equipUpgrade.sprite = eq.eq.sprite;
        currentslotUpgrade.text = eq.GetNameType() + " slot";
        UpdateInfoUpgrade(eq);
        nextValue.color = upColor;
        nameTypeUpgrade.text = eq.GetNameLevelNType();
        iconDesign.sprite = iconDesigns[(int)eq.type];

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

        iconUpgrade.SetNativeSize();

        CheckStateUpgrade(eq);

        panelUpgradeEquip.gameObject.SetActive(true);
        UIEffect.instance.ScalePopup(panelUpgradeEquip, popupUpgradeEquip, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    int GetDush(int levelUpgrade)
    {
        int length = DataManager.instance.equipmentConfig.dushUpgrades.Length - 1;
        if (levelUpgrade <= length)
        {
            return DataManager.instance.equipmentConfig.dushUpgrades[levelUpgrade];
        }
        else
        {
            int step = 0;
            int result = 0;
            int multiplier = 0;

            for (int i = length; i <= levelUpgrade; i++)
            {
                result = DataManager.instance.equipmentConfig.dushUpgrades[length] + (multiplier * DataManager.instance.equipmentConfig.dushUpgradeStep);
                step++;
                if (step == DataManager.instance.equipmentConfig.dushStep)
                {
                    step = 0;
                    multiplier++;
                }
            }
            return result;
        }
    }

    int GetDesign(int levelUpgrade)
    {
        int length = DataManager.instance.equipmentConfig.desginUpgrades.Length - 1;
        if (levelUpgrade <= length)
        {
            return DataManager.instance.equipmentConfig.desginUpgrades[levelUpgrade];
        }
        else
        {
            int step = 0;
            int result = 0;
            int multiplier = 0;

            for (int i = length; i <= levelUpgrade; i++)
            {
                result = DataManager.instance.equipmentConfig.desginUpgrades[length] + (multiplier * DataManager.instance.equipmentConfig.designUpgradeStep);
                step++;
                if (step == DataManager.instance.equipmentConfig.designStep)
                {
                    step = 0;
                    multiplier++;
                }
            }
            return result;
        }
    }

    int GetLevelUpgrade(EQUIPMENTTYPE type)
    {
        if (type == EQUIPMENTTYPE.SHOTGUN) return playerInventory.gunLevelUpgrade;
        else if (type == EQUIPMENTTYPE.GRENADE) return playerInventory.boomLevelUpgrade;
        else if (type == EQUIPMENTTYPE.CAP) return playerInventory.capLevelUpgrade;
        else return playerInventory.clothesLevelUpgrade;
    }

    int GetAmoutDesign(EQUIPMENTTYPE type)
    {
        if (type == EQUIPMENTTYPE.SHOTGUN) return playerInventory.amoutGunDesign;
        else if (type == EQUIPMENTTYPE.GRENADE) return playerInventory.amoutBoomDesign;
        else if (type == EQUIPMENTTYPE.CAP) return playerInventory.amoutCapDesign;
        else return playerInventory.amoutClothesDesign;
    }

    void SubtractDesgin(EQUIPMENTTYPE type, int amout)
    {
        if (type == EQUIPMENTTYPE.SHOTGUN) playerInventory.amoutGunDesign -= amout;
        else if (type == EQUIPMENTTYPE.GRENADE) playerInventory.amoutBoomDesign -= amout;
        else if (type == EQUIPMENTTYPE.CAP) playerInventory.amoutCapDesign -= amout;
        else playerInventory.amoutClothesDesign -= amout;
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
        currentLevelType.color = colorEquipLevels[(int)equipMain.level];
        levelType.text = eq.GetNameLevelNType();
        levelType.color = colorEquipLevels[(int)eq.level];
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

    public int GetEquipValue(EQUIPMENTTYPE type, int level, int levelUpgrade)
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
