using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonEquipment : MonoBehaviour
{
    public Image panelChances;
    public RectTransform chancePopup;

    public TextMeshProUGUI textGem;
    public TextMeshProUGUI textKey;
    public Image progress;
    public Image frameRollX1;
    public Image frameRollX10;
    public TextMeshProUGUI textFrameRollX1;
    public TextMeshProUGUI textFrameRollX10;
    public Image framePriceRollX1;
    public Image framePriceRollX10;
    public GameObject[] frameInactive;

    public float[][] chanceDatas;

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

    public void Start()
    {
        framePriceX1Original = framePriceRollX1.color;
        framePriceX10Original = framePriceRollX10.color;
    }

    public void LoadData()
    {
        chanceDatas = DataManager.instance.chanceConfig.chances;
        chanceSorts = new float[chanceDatas[level].Length];
        if (DataManager.instance.dataStorage.chanceDataStorage != null)
        {
            level = DataManager.instance.dataStorage.chanceDataStorage.level;
            amout = DataManager.instance.dataStorage.chanceDataStorage.amout;
        }
        textLevel.text = "Lv." + (level + 1);
        int amoutUpgradeLevel = DataManager.instance.chanceConfig.amoutUpgradeLevel[level];
        SetTextNFillAmout(amoutUpgradeLevel);
        ChanceSort(level);
    }

    public void UpdateText()
    {
        textGem.text = UIHandler.instance.ConvertNumberAbbreviation(EquipmentController.instance.playerInventory.gem);
    }

    public void CheckButtonState()
    {
        if(EquipmentController.instance.playerInventory.gem < 5)
        {
            frameRollX1.raycastTarget = false;
            frameInactive[0].SetActive(true);
            framePriceRollX1.color = notEnoughMoney;
            textFrameRollX1.color = Color.white;
        }
        else
        {
            frameRollX1.raycastTarget = true;
            frameInactive[0].SetActive(false);
            framePriceRollX1.color = framePriceX1Original;
            textFrameRollX1.color = textFrameOriginal;
        }
        if (EquipmentController.instance.playerInventory.gem < 45)
        {
            frameRollX10.raycastTarget = false;
            frameInactive[1].SetActive(true);
            framePriceRollX10.color = notEnoughMoney;
            textFrameRollX10.color = Color.white;
        }
        else
        {
            frameRollX10.raycastTarget = true;
            frameInactive[1].SetActive(false);
            framePriceRollX10.color = framePriceX10Original;
            textFrameRollX10.color = textFrameOriginal;
        }
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
        EquipmentController.instance.playerInventory.gem -= 5;
        Roll();
        SortEquip();
        CheckButtonState();
        UpdateText();
    }

    public void RollX10()
    {
        EquipmentController.instance.playerInventory.gem -= 45;
        for (int i = 0; i < 10; i++)
        {
            Roll();
        }
        SortEquip();
        CheckButtonState();
        UpdateText();
    }

    void SortEquip()
    {
        if (EquipmentController.instance.isQuality) EquipmentController.instance.QualitySort();
        else EquipmentController.instance.ClassSort();
        EquipmentController.instance.CheckStateButtonEquipBestNSellDuplicates();
    }

    void Roll()
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
                EquipmentController.instance.AddEquip(type, GetRateLevel(chanceSorts[i]));
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
        LoadChances(++leveInPopUp);
    }

    public void ChanceBackLevel()
    {
        if (leveInPopUp - 1 == -1) return;
        LoadChances(--leveInPopUp);
    }

    public void HideChances()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelChances, chancePopup, 0f, 0f, 0.8f, 0f);
        panelChances.gameObject.SetActive(false);
    }
}
