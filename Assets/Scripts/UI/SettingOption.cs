using DG.Tweening;
using UnityEngine;

public class SettingOption : MonoBehaviour
{
    public Setting.TypeSetting type;
    public GameObject[] icons;
    public GameObject buttonInactive;
    public RectTransform dot;
    float targetX = 38.5f;

    public void SwitchState()
    {
        UIHandler.instance.setting.SetBoolType(type, buttonInactive.activeSelf, false, 0.25f);
        SwitchStateHandle(buttonInactive.activeSelf, 0.25f);
    }

    public void SwitchStateHandle(bool isActive, float duration)
    {
        DoDot(isActive ? targetX : -targetX, duration);
        IconActive(isActive);
        buttonInactive.SetActive(!isActive);
    }

    void IconActive(bool isActive)
    {
        icons[0].SetActive(isActive);
        icons[1].SetActive(!isActive);
    }

    void DoDot(float x, float duration)
    {
        dot.DOKill();
        dot.DOAnchorPosX(x, duration);
    }
}
