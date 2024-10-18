using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonEquipment : MonoBehaviour
{
    public Image panelChances;
    public RectTransform chancePopup;

    public float[][] chancesData;

    public TextMeshProUGUI[] chancePercentages;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textLevelInPopUp;

    public int level;
    public int leveInPopUp;
    public int amout;

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.S))
        {
            ShowChances();
        }*/
    }

    public void LoadData()
    {
        chancesData = DataManager.instance.chanceConfig.chances;
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
        textLevelInPopUp.text = "Lv." + level;

        for (int i = 0; i < chancePercentages.Length; i++)
        {
            chancePercentages[i].text = chancesData[level - 1][i] + "%";
        }
    }

    public void ChanceNextLevel()
    {
        if (leveInPopUp + 1 == chancesData.Length + 1) return;
        LoadChances(++leveInPopUp);
    }

    public void ChanceBackLevel()
    {
        if (leveInPopUp - 1 == 0) return;
        LoadChances(--leveInPopUp);
    }

    public void HideChances()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelChances, chancePopup, 0f, 0f, 0.8f, 0f);
        panelChances.gameObject.SetActive(false);
    }
}
