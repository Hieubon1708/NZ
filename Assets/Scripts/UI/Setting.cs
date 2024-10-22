using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public SettingOption[] settingOptions;
    public Image panelSetting;
    public RectTransform settingPopup;
    public bool isSoundActive;
    public bool isMusicActive;

    public void LoadData()
    {
        if(DataManager.instance.dataStorage != null)
        {
            isMusicActive = DataManager.instance.dataStorage.isMusicActive;
            isSoundActive = DataManager.instance.dataStorage.isSoundActive;
        }
        settingOptions[0].LoadData(isMusicActive);
        settingOptions[1].LoadData(isSoundActive);
    }

    public void SetBoolType(TypeSetting type, bool isActive)
    {
        if(type == TypeSetting.Music) isMusicActive = isActive;
        if(type == TypeSetting.Sound) isSoundActive = isActive;
    }

    public void ShowPanelSetting()
    {
        panelSetting.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelSetting, settingPopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }
    
    public void HidePanelSetting()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelSetting, settingPopup, 0f, 0f, 0.8f, 0f);
        panelSetting.gameObject.SetActive(false);
    }

    public enum TypeSetting
    {
        None, Sound, Music
    }
}
