using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonEquipment : MonoBehaviour
{
    public Image panelChances;
    public RectTransform chancePopup;

    public Image panelRoll;
    public RectTransform rollPopup;

    public TextMeshProUGUI textGem;
    public TextMeshProUGUI textKey;
    public Image progress;
    public Image frameRollX1;
    public Image frameRollX10;
    public Image frameRollX1InPopup;
    public Image frameRollX10InPopup;
    public TextMeshProUGUI textFrameRollX1;
    public TextMeshProUGUI textFrameRollX10;
    public TextMeshProUGUI textFrameRollX1InPopup;
    public TextMeshProUGUI textFrameRollX10InPopup;
    public Image framePriceRollX1;
    public Image framePriceRollX10;
    public Image framePriceRollX1InPopup;
    public Image framePriceRollX10InPopup;
    public GameObject[] frameInactive;
    public GameObject[] frameInactiveInPopup;

    public float[][] chanceDatas;
    public EquipmentInfo equipmentInfoX1;
    public EquipmentInfo[] equipmentInfosX10;

    public TextMeshProUGUI[] chancePercentages;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textAmoutProgress;
    public TextMeshProUGUI textLevelInPopUp;

    public float[] chanceSorts;

    public Color framePriceX1Original;
    public Color framePriceX10Original;
    public Color textFrameOriginal;
    public Color notEnoughMoney;
    public int level;
    public int leveInPopUp;
    public int amout;

    Coroutine scattered;

    public void Start()
    {
        framePriceX1Original = framePriceRollX1.color;
        framePriceX10Original = framePriceRollX10.color;
    }

    public void LoadData()
    {
        chanceDatas = DataManager.instance.chanceConfig.chances;
        chanceSorts = new float[chanceDatas[level].Length];
        if (DataManager.instance.chanceDataStorage != null)
        {
            level = DataManager.instance.chanceDataStorage.level;
            amout = DataManager.instance.chanceDataStorage.amout;
        }
        textLevel.text = "Lv." + (level + 1);
        int amoutUpgradeLevel = DataManager.instance.chanceConfig.amoutUpgradeLevel[level];
        SetTextNFillAmout(amoutUpgradeLevel);
        ChanceSort(level);
        CheckNotif();
    }

    public void CheckNotif()
    {
        if (EquipmentController.instance.playerInventory.gem >= 5 || EquipmentController.instance.playerInventory.gem >= 45) UIHandler.instance.menu.notifOptions[1].SetActive(true);
        else UIHandler.instance.menu.notifOptions[1].SetActive(false);
        DataManager.instance.SaveChance();
        DataManager.instance.SavePlayer();
    }

    void ShowRoll()
    {
        panelRoll.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelRoll, rollPopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HideRoll()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        UIHandler.instance.uIEffect.ScalePopup(panelRoll, rollPopup, 0f, 0f, 0.8f, 0f);
        panelRoll.gameObject.SetActive(false);
        UIHandler.instance.tutorial.TutorialButtonRoll(false);
    }

    void ActiveEquipsFrame(bool isActive)
    {
        equipmentInfoX1.gameObject.SetActive(false);
        for (int i = 0; i < equipmentInfosX10.Length; i++)
        {
            equipmentInfosX10[i].gameObject.SetActive(isActive);
        }
    }

    public void UpdateText()
    {
        textGem.text = UIHandler.instance.ConvertNumberAbbreviation(EquipmentController.instance.playerInventory.gem);
        textKey.text = UIHandler.instance.ConvertNumberAbbreviation(EquipmentController.instance.playerInventory.key);
    }

    void ButtonStateX1Handle(bool isActive, Color colorFrame, Color colorText)
    {
        frameRollX1.raycastTarget = isActive;
        frameRollX1InPopup.raycastTarget = isActive;
        frameInactive[0].SetActive(!isActive);
        frameInactiveInPopup[0].SetActive(!isActive);
        framePriceRollX1.color = colorFrame;
        framePriceRollX1InPopup.color = colorFrame;
        textFrameRollX1.color = colorText;
        textFrameRollX1InPopup.color = colorText;
    }

    void ButtonStateX10Handle(bool isActive, Color colorFrame, Color colorText)
    {
        frameRollX10.raycastTarget = isActive;
        frameRollX10InPopup.raycastTarget = isActive;
        frameInactive[1].SetActive(!isActive);
        frameInactiveInPopup[1].SetActive(!isActive);
        framePriceRollX10.color = colorFrame;
        framePriceRollX10InPopup.color = colorFrame;
        textFrameRollX10.color = colorText;
        textFrameRollX10InPopup.color = colorText;
    }

    public void RewardGem()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        EquipmentController.instance.playerInventory.gem += 10;
        UpdateText();
        CheckButtonState();
    }

    public void CheckButtonState()
    {
        if (EquipmentController.instance.playerInventory.gem < 5) ButtonStateX1Handle(false, notEnoughMoney, Color.white);
        else ButtonStateX1Handle(true, framePriceX1Original, textFrameOriginal);
        if (EquipmentController.instance.playerInventory.gem < 45) ButtonStateX10Handle(false, notEnoughMoney, Color.white);
        else ButtonStateX10Handle(true, framePriceX1Original, textFrameOriginal);
        UIHandler.instance.menu.CheckNotifAll();
    }

    void ChanceSort(int level)
    {
        for (int i = 0; i < chanceSorts.Length; i++)
        {
            chanceSorts[i] = chanceDatas[level][i];
        }
        for (int i = 0; i < chanceSorts.Length - 1; i++)
        {
            for (int j = i + 1; j < chanceSorts.Length; j++)
            {
                if (chanceSorts[i] > chanceSorts[j])
                {
                    float temp = chanceSorts[j];
                    chanceSorts[j] = chanceSorts[i];
                    chanceSorts[i] = temp;
                }
            }
        }
    }

    public void RollX1()
    {
        //AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        GetMax();
        ActiveEquipsFrame(false);
        SubtractGem(5);
        Roll(0, false);
        if ((int)equipmentInfoX1.level > GetMaxType((int)equipmentInfoX1.type)) equipmentInfoX1.frameLight.SetActive(true);
        else equipmentInfoX1.frameLight.SetActive(false);
        equipmentInfoX1.gameObject.SetActive(true);
        EquipmentController.instance.SortEquip();
        CheckButtonState();
        if (!panelRoll.gameObject.activeSelf) ShowRoll();
        EquipmentController.instance.UpdateDesignPosition();
    }

    void SubtractGem(int gem)
    {
        EquipmentController.instance.playerInventory.gem -= gem;
        UpdateText();
    }

    int GetMaxType(int type)
    {
        if (type == 0) return maxGun;
        if (type == 1) return maxBoom;
        if (type == 2) return maxCap;
        if (type == 3) return maxClothes;
        else return -1;
    }

    int maxGun;
    int maxBoom;
    int maxCap;
    int maxClothes;

    void GetMax()
    {
        maxGun = EquipmentController.instance.GetEquipMax(EquipmentController.instance.equipMains[0].type, EquipmentController.instance.equipMains[0].level);
        maxBoom = EquipmentController.instance.GetEquipMax(EquipmentController.instance.equipMains[1].type, EquipmentController.instance.equipMains[1].level);
        maxCap = EquipmentController.instance.GetEquipMax(EquipmentController.instance.equipMains[2].type, EquipmentController.instance.equipMains[2].level);
        maxClothes = EquipmentController.instance.GetEquipMax(EquipmentController.instance.equipMains[3].type, EquipmentController.instance.equipMains[3].level);
    }

    public void RollX10()
    {
        //AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        GetMax();
        ActiveEquipsFrame(false);
        SubtractGem(45);
        for (int i = 0; i < 10; i++)
        {
            Roll(i, true);
        }
        if (scattered != null) StopCoroutine(scattered);
        scattered = StartCoroutine(Scattered(10));
        EquipmentController.instance.SortEquip();
        CheckButtonState();
        if (!UIHandler.instance.tutorial.isFirstTimeClickButtonRoll) UIHandler.instance.tutorial.TutorialButtonRoll(true);
        if (!panelRoll.gameObject.activeSelf) ShowRoll();
        EquipmentController.instance.UpdateDesignPosition();
    }

    IEnumerator Scattered(int amount)
    {
        int i = 0;
        while (i < amount)
        {
            if ((int)equipmentInfosX10[i].level > GetMaxType((int)equipmentInfosX10[i].type)) equipmentInfosX10[i].frameLight.SetActive(true);
            else equipmentInfosX10[i].frameLight.SetActive(false);
            equipmentInfosX10[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            i++;
        }
    }

    void Roll(int index, bool isX10)
    {
        float rate = Random.Range(0f, 100f);
        amout++;

        float totalPercent = 0;
        Debug.LogWarning("rate " + rate);
        for (int i = 0; i < chanceSorts.Length; i++)
        {
            totalPercent += chanceSorts[i];
            if (rate <= totalPercent)
            {
                Debug.LogWarning("totalPercent " + totalPercent);
                int type = Random.Range(0, EquipmentController.instance.equipMains.Length);
                Debug.LogWarning("weapon type " + type + " level " + GetRateLevel(chanceSorts[i]));
                int level = GetRateLevel(chanceSorts[i]);
                if (isX10) EquipmentController.instance.SetEquip(type, level, equipmentInfosX10[index]);
                else EquipmentController.instance.SetEquip(type, level, equipmentInfoX1);
                EquipmentController.instance.AddEquip(type, level);
                break;
            }
        }

        int amoutUpgradeLevel = DataManager.instance.chanceConfig.amoutUpgradeLevel[level];
        if (amout == amoutUpgradeLevel)
        {
            amout = 0;
            level++;
            textLevel.text = "Lv." + (level + 1);
            ChanceSort(level);
        }
        SetTextNFillAmout(amoutUpgradeLevel);
    }

    int GetRateLevel(float value)
    {
        for (int i = 0; i < chanceDatas[level].Length; i++)
        {
            if (chanceDatas[level][i] == value) return i;
        }
        return -1;
    }

    void SetTextNFillAmout(int amoutUpgradeLevel)
    {
        textAmoutProgress.text = amout + "/" + amoutUpgradeLevel;
        float percent = (float)amout / amoutUpgradeLevel;
        progress.fillAmount = percent;
    }

    public void ShowChances()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        leveInPopUp = level;
        LoadChances(leveInPopUp);
        panelChances.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelChances, chancePopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    void LoadChances(int level)
    {
        textLevelInPopUp.text = "Lv." + (level + 1);

        for (int i = 0; i < chancePercentages.Length; i++)
        {
            chancePercentages[i].text = chanceDatas[level][i] + "%";
        }
    }

    public void ChanceNextLevel()
    {
        if (leveInPopUp + 1 == chanceDatas.Length) return;
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        LoadChances(++leveInPopUp);
    }

    public void ChanceBackLevel()
    {
        if (leveInPopUp - 1 == -1) return;
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        LoadChances(--leveInPopUp);
    }

    public void HideChances()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        UIHandler.instance.uIEffect.ScalePopup(panelChances, chancePopup, 0f, 0f, 0.8f, 0f);
        panelChances.gameObject.SetActive(false);
    }
}
