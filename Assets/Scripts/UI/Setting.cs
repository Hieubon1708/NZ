using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public SettingOption[] settingOptions;
    public Image panelSetting;
    public RectTransform settingPopup;

    public void LoadData()
    {
        settingOptions[0].LoadData(DataManager.instance.dataStorage.isMusicActive);
        settingOptions[1].LoadData(DataManager.instance.dataStorage.isMusicActive);
    }

    public void SetBoolType(TypeSetting type, bool isActive)
    {
        AudioController.instance.PlaySoundButton(AudioController.instance.buttonClick);
        if (type == TypeSetting.Music) DataManager.instance.dataStorage.isMusicActive = isActive;
        if(type == TypeSetting.Sound) DataManager.instance.dataStorage.isSoundActive = isActive;
        Debug.LogWarning(DataManager.instance.dataStorage.isSoundActive);
        DataManager.instance.SaveData();
    }

    public void ShowPanelSetting()
    {
        AudioController.instance.PlaySoundButton(AudioController.instance.buttonClick);
        panelSetting.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelSetting, settingPopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }
    
    public void HidePanelSetting()
    {
        AudioController.instance.PlaySoundButton(AudioController.instance.buttonClick);
        UIHandler.instance.uIEffect.ScalePopup(panelSetting, settingPopup, 0f, 0f, 0.8f, 0f);
        panelSetting.gameObject.SetActive(false);
    }

    public enum TypeSetting
    {
        None, Sound, Music
    }
}
