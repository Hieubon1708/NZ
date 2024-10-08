using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SummonEquipment : MonoBehaviour
{
    public static SummonEquipment instance;

    public Image panelChances;
    public RectTransform chancesPopup;

    public float[][] chancesData;

    public TextMeshProUGUI[] chancePercentages;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textLevelInPopUp;

    public int level;
    public int leveInPopUp;
    public int amout;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ShowChances();
        }
    }

    public void Start()
    {
        chancesData = DataManager.instance.chanceConfig.chances;
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

        PanelNPopupChange(1f, 0.1f, 1f, 0.5f);
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
        panelChances.DOKill();
        chancesPopup.DOKill();

        PanelNPopupChange(0f, 0f, 0.8f, 0f);

        panelChances.gameObject.SetActive(false);
    }

    void PanelNPopupChange(float alpha, float durationAlpha, float scale, float durationScale)
    {
        panelChances.DOFade(alpha, durationAlpha);
        chancesPopup.DOScale(scale, durationScale).SetEase(Ease.OutBack);
    }
}
