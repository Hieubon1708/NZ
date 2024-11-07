using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public SettingOption[] settingOptions;
    public Image panelSetting;
    public RectTransform settingPopup;

    public void LoadData()
    {
        if (!UIHandler.instance.tutorial.isFirstTimePlay)
        {
            DataManager.instance.dataStorage.isMusicActive = true;
            DataManager.instance.dataStorage.isSoundActive = true;
        }
        SetBoolType(TypeSetting.Sound, DataManager.instance.dataStorage.isSoundActive, true, 0);
        settingOptions[1].SwitchStateHandle(DataManager.instance.dataStorage.isSoundActive, 0);
        SetBoolType(TypeSetting.Music, DataManager.instance.dataStorage.isMusicActive, true, 0);
        settingOptions[0].SwitchStateHandle(DataManager.instance.dataStorage.isMusicActive, 0);
    }

    public void SetBoolType(TypeSetting type, bool isActive, bool isLoadData, float time)
    {
        if(!isLoadData) AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        if (type == TypeSetting.Music)
        {
            DataManager.instance.dataStorage.isMusicActive = isActive;
            AudioController.instance.EnableMusic(isActive, time);
        }
        if (type == TypeSetting.Sound)
        {
            DataManager.instance.dataStorage.isSoundActive = isActive;
            AudioController.instance.EnableSound(isActive, time);
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
