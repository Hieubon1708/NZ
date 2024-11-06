using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public SettingOption[] settingOptions;
    public Image panelSetting;
    public RectTransform settingPopup;

    public void LoadData()
    {
        SetBoolType(TypeSetting.Sound, DataManager.instance.dataStorage.isSoundActive);
        settingOptions[1].SwitchStateHandle(DataManager.instance.dataStorage.isSoundActive, 0.25f);
        SetBoolType(TypeSetting.Music, DataManager.instance.dataStorage.isMusicActive);
        settingOptions[0].SwitchStateHandle(DataManager.instance.dataStorage.isMusicActive, 0.25f);
    }

    public void SetBoolType(TypeSetting type, bool isActive)
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        if (type == TypeSetting.Music)
        {
            DataManager.instance.dataStorage.isMusicActive = isActive;
            AudioController.instance.EnableMusic(isActive, 0.25f);
        }
        if (type == TypeSetting.Sound)
        {
            DataManager.instance.dataStorage.isSoundActive = isActive;
            AudioController.instance.EnableSound(isActive, 0.25f);
        }
        DataManager.instance.SaveData();
    }

    public void ShowPanelSetting()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        panelSetting.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelSetting, settingPopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }
    
    public void HidePanelSetting()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        UIHandler.instance.uIEffect.ScalePopup(panelSetting, settingPopup, 0f, 0f, 0.8f, 0f);
        panelSetting.gameObject.SetActive(false);
    }

    public enum TypeSetting
    {
        None, Sound, Music
    }
}
