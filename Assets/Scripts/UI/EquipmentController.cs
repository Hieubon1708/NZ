using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public PlayerInventory playerInventory;

    public GameObject equipmentPrefab;
    public List<EquipmentInfo> equipments = new List<EquipmentInfo>();
    public EquipmentInfo[] equipMains;
    public GameObject[] arrowEquipMains;

    public RectTransform container;
    public RectTransform view;
    public ScrollRect scrollInventory;
    public DesignContraint designContraint;

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
    public Sprite equipBest;
    public Sprite sellDuplicates;

    public Image frameEquipBest;
    public Image frameSellDuplicates;
    public Image maxLevel;
    public Image levelUp;

    public GameObject lightEquipBest;

    public Color upColor;
    public Color downColor;
    public Color[] colorEquipLevels;

    public bool isQuality;
    public TextMeshProUGUI textQualityNClass;

    Coroutine delayDesignContraint;

    public void Awake()
    {
        instance = this;
        isQuality = true;
        Generate();
    }

    public void DesignUpdatePosition()
    {
        if (delayDesignContraint != null) StopCoroutine(delayDesignContraint);
        delayDesignContraint = StartCoroutine(SetPosition());
    }

    IEnumerator SetPosition()
    {
        yield return new WaitForFixedUpdate();
        Debug.LogWarning(designContraint.content.sizeDelta.y);
        float y = Mathf.Clamp(container.position.y - container.sizeDelta.y - 260, float.MinValue, designContraint.startY);
        designContraint.transform.position = new Vector2(designContraint.transform.position.x, y);
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

    public void DesignContraint()
    {
        float y = Mathf.Clamp(container.position.y - container.sizeDelta.y - 260, float.MinValue, designContraint.startY);
        designContraint.transform.position = new Vector2(designContraint.transform.position.x, y);
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
                EquipmentDataStorage eq = DataManager.instance.dataStorage.playerDataStorage.equipmentDataStorages[i];
                AddEquip(eq.type, eq.level);
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
        CheckDisplayDesign();
        SortNCheckStateButton();
        QualitySort();
        CheckWeaponUpgrade();
    }

    public void SortNCheckStateButton()
    {
        CheckStateEquipBest();
        CheckStateSellDuplicates();
    }

    public void AddEquip(int type, int level)
    {
        equipments[amoutEquip].gameObject.SetActive(true);
        SetEquip(type, level, equipments[amoutEquip]);
        amoutEquip++;
        if (amoutEquip == equipments.Count - 1) Generate();
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
        scrollInventory.normalizedPosition = new Vector2(0, 1);

        if (playerInventory.amoutClothesDesign == 0) clothesDesign.SetActive(false);
        else clothesDesign.SetActive(true);
        if (playerInventory.amoutCapDesign == 0) capDesign.SetActive(false);
        else capDesign.SetActive(true);
        if (playerInventory.amoutBoomDesign == 0) boomDesign.SetActive(false);
        else boomDesign.SetActive(true);
        if (playerInventory.amoutGunDesign == 0) gunDesign.SetActive(false);
        else gunDesign.SetActive(true);

        if (playerInventory.amoutClothesDesign == 0 && playerInventory.amoutCapDesign == 0 && playerInventory.amoutBoomDesign == 0 && playerInventory.amoutGunDesign == 0)
        {
            designContraint.label.SetActive(false);
            view.offsetMin = new Vector2(view.offsetMin.x, 30);
        }
        else
        {
            designContraint.label.SetActive(true);
            view.offsetMin = new Vector2(view.offsetMin.x, 345);
        }
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

    public void QualityNClass()
    {
        isQuality = !isQuality;

        textQualityNClass.text = isQuality ? "Quality" : "Class";

        if (isQuality) QualitySort();
        else ClassSort();
    }

    public void ClassSort()
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

    void CheckWeaponUpgrade()
    {
        for (int i = 0; i < equipMains.Length; i++)
        {
            bool isNok = false;
            int levelUpgrade = GetLevelUpgrade(equipMains[i].type);
            int dushSelected = GetDush(levelUpgrade);
            int designSelected = GetDesign(levelUpgrade);
            int amoutDesgin = GetAmoutDesign(equipMains[i].type);

            if (playerInventory.dush < dushSelected || amoutDesgin < designSelected)
            {
                isNok = true;
            }

            if (isNok) arrowEquipMains[i].SetActive(false);
            else arrowEquipMains[i].SetActive(true);
        }
    }

    public void QualitySort()
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
        CheckStateEquipBest();
        CheckStateSellDuplicates();
    }

    public void UpgradeAccept()
    {
        Upgrade();
        CheckDisplayDesign();
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
        CheckWeaponUpgrade();
    }

    void UpdateInfoUpgrade(EquipmentInfo eq)
    {
        string level = "Lv." + UIHandler.instance.ConvertNumberAbbreviation(GetLevelUpgrade(eq.type) + 1);
        currentLevelUpgrade.text = level;
        equipCurrentLevels[(int)eq.type].text = level;
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
        CheckDisplayDesign();
    }

    void CheckStateEquipBest()
    {
        bool isHave = false;
        bool[] isMaxOnce = new bool[equipMains.Length];
        for (int i = 0; i < equipMains.Length; i++)
        {
            int max = GetEquipMax(equipMains[i].type, equipMains[i].level);
            for (int j = 0; j < amoutEquip; j++)
            {
                if (equipMains[i].type == equipments[j].type)
                {
                    if (equipMains[i].level < equipments[j].level) isHave = true;
                    if ((int)equipments[j].level == max && !isMaxOnce[i] && equipMains[i].level != equipments[j].level)
                    {
                        isMaxOnce[i] = true;
                        equipments[j].frameLight.SetActive(true);
                    }
                    else
                    {
                        equipments[j].frameLight.SetActive(false);
                    }
                }
            }
        }
        lightEquipBest.SetActive(isHave);
        frameEquipBest.raycastTarget = isHave;
        frameEquipBest.sprite = isHave ? equipBest : buttonNok;
    }

    void CheckStateSellDuplicates()
    {
        int maxGun = GetEquipMax(EQUIPMENTTYPE.SHOTGUN, equipMains[0].level);
        int maxBoom = GetEquipMax(EQUIPMENTTYPE.GRENADE, equipMains[1].level);
        int maxCap = GetEquipMax(EQUIPMENTTYPE.CAP, equipMains[2].level);
        int maxClothes = GetEquipMax(EQUIPMENTTYPE.ARMOR, equipMains[3].level);

        for (int i = 0; i < amoutEquip; i++)
        {
            if (equipments[i].type == EQUIPMENTTYPE.SHOTGUN)
            {
                if ((int)equipMains[0].level == maxGun)
                {
                    if (CheckDuplicates(-1, EQUIPMENTTYPE.SHOTGUN, maxGun)) return;
                }
                else if ((int)equipments[i].level == maxGun)
                {
                    if (CheckDuplicates(i, EQUIPMENTTYPE.SHOTGUN, maxGun)) return;
                }
            }
            if (equipments[i].type == EQUIPMENTTYPE.GRENADE)
            {
                if ((int)equipMains[1].level == maxBoom)
                {
                    if (CheckDuplicates(-1, EQUIPMENTTYPE.GRENADE, maxBoom)) return;
                }
                else if ((int)equipments[i].level == maxBoom)
                {
                    if (CheckDuplicates(i, EQUIPMENTTYPE.GRENADE, maxBoom)) return;
                }
            }
            if (equipments[i].type == EQUIPMENTTYPE.CAP)
            {
                if ((int)equipMains[2].level == maxCap)
                {
                    if (CheckDuplicates(-1, EQUIPMENTTYPE.CAP, maxCap)) return;
                }
                else if ((int)equipments[i].level == maxCap)
                {
                    if (CheckDuplicates(i, EQUIPMENTTYPE.CAP, maxCap)) return;
                }
            }
            if (equipments[i].type == EQUIPMENTTYPE.ARMOR)
            {
                if ((int)equipMains[3].level == maxClothes)
                {
                    if (CheckDuplicates(-1, EQUIPMENTTYPE.ARMOR, maxClothes)) return;
                }
                else if ((int)equipments[i].level == maxClothes)
                {
                    if (CheckDuplicates(i, EQUIPMENTTYPE.ARMOR, maxClothes)) return;
                }
            }
        }

        frameSellDuplicates.sprite = buttonNok;
        frameSellDuplicates.raycastTarget = false;
    }

    bool CheckDuplicates(int index, EQUIPMENTTYPE type, int max)
    {
        for (int j = 0; j < amoutEquip; j++)
        {
            if (index != j && type == equipments[j].type && (int)equipments[j].level <= max)
            {
                frameSellDuplicates.raycastTarget = true;
                frameSellDuplicates.sprite = sellDuplicates;
                return true;
            }
        }
        return false;
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
            playerInventory.ChangeGun();
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
            playerInventory.ChangeCap();
        }
        else
        {
            UpdateHealth();
            playerInventory.clothesLevel = (int)eq1.level;
            PlayerController.instance.player.playerSkiner.ClothesChange();
            PlayerController.instance.player.HpChange();
            playerInventory.ChangeClothes();
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

    void RemoveEquipDuplicates(int index, EQUIPMENTTYPE type, int max, int length)
    {
        for (int j = 0; j < length; j++)
        {
            if (equipments[j].gameObject.activeSelf && index != j && type == equipments[j].type && (int)equipments[j].level <= max)
            {
                equipments[j].gameObject.SetActive(false);
                amoutEquip--;
            }
        }
    }

    public void SellDuplicates()
    {
        int maxGun = GetEquipMax(EQUIPMENTTYPE.SHOTGUN, equipMains[0].level);
        int maxBoom = GetEquipMax(EQUIPMENTTYPE.GRENADE, equipMains[1].level);
        int maxCap = GetEquipMax(EQUIPMENTTYPE.CAP, equipMains[2].level);
        int maxClothes = GetEquipMax(EQUIPMENTTYPE.ARMOR, equipMains[3].level);

        int length = amoutEquip;

        for (int i = 0; i < length; i++)
        {
            if (equipments[i].gameObject.activeSelf)
            {
                if (equipments[i].type == EQUIPMENTTYPE.SHOTGUN)
                {
                    if ((int)equipMains[0].level == maxGun) RemoveEquipDuplicates(-1, EQUIPMENTTYPE.SHOTGUN, maxGun, length);
                    else if ((int)equipments[i].level == maxGun) RemoveEquipDuplicates(i, EQUIPMENTTYPE.SHOTGUN, maxGun, length);
                }
                if (equipments[i].type == EQUIPMENTTYPE.GRENADE)
                {
                    if ((int)equipMains[1].level == maxBoom) RemoveEquipDuplicates(-1, EQUIPMENTTYPE.GRENADE, maxBoom, length);
                    else if ((int)equipments[i].level == maxBoom) RemoveEquipDuplicates(i, EQUIPMENTTYPE.GRENADE, maxBoom, length);
                }
                if (equipments[i].type == EQUIPMENTTYPE.CAP)
                {
                    if ((int)equipMains[2].level == maxCap) RemoveEquipDuplicates(-1, EQUIPMENTTYPE.CAP, maxCap, length);
                    else if ((int)equipments[i].level == maxCap) RemoveEquipDuplicates(i, EQUIPMENTTYPE.CAP, maxCap, length);
                }
                if (equipments[i].type == EQUIPMENTTYPE.ARMOR)
                {
                    if ((int)equipMains[3].level == maxClothes) RemoveEquipDuplicates(-1, EQUIPMENTTYPE.ARMOR, maxClothes, length);
                    else if ((int)equipments[i].level == maxClothes) RemoveEquipDuplicates(i, EQUIPMENTTYPE.ARMOR, maxClothes, length);
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
        CheckDisplayDesign();
        CheckStateSellDuplicates();
        frameSellDuplicates.sprite = buttonNok;
        frameSellDuplicates.raycastTarget = false;
    }

    int GetEquipMax(EQUIPMENTTYPE type, EQUIPMENTLEVEL level)
    {
        int result = (int)level;
        for (int i = 0; i < equipments.Count; i++)
        {
            if (equipments[i].type == type && equipments[i].level > equipMains[(int)type].level && (int)equipments[i].level > result)
            {
                result = (int)equipments[i].level;
            }
        }
        return result;
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
        CheckStateEquipBest();
        CheckStateSellDuplicates();
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
        UIHandler.instance.uIEffect.ScalePopup(panelUpgradeEquip, popupUpgradeEquip, 222f / 255f, 0.1f, 1f, 0.5f);
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
        UIHandler.instance.uIEffect.ScalePopup(panelUpgradeEquip, popupUpgradeEquip, 0f, 0f, 0.8f, 0f);
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
        UIHandler.instance.uIEffect.ScalePopup(panelEquip, popupEquip, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HidePopupSwap()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelEquip, popupEquip, 0f, 0f, 0.8f, 0f);
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
