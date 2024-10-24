using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponBooster : ButtonState
{
    public int energy;
    public Booster booster;
    public Image frame;
    public TextMeshProUGUI textEnergy;
    bool isUseBooster;

    public void UpdateTextEnergy()
    {
        textEnergy.text = energy.ToString();
    }

    public void SubtractEnergy(float percentage)
    {
        energy -= Mathf.RoundToInt(energy * percentage / 100);
        UpdateTextEnergy();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ScaleButton(localScale * 0.95f, 0.05f);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        CheckTutorial(false);
        isUseBooster = true;
        frame.sprite = booster.frameDelay;
        frame.raycastTarget = false;
        DOVirtual.DelayedCall(0.5f, delegate
        {
            frame.raycastTarget = true;
            isUseBooster = false;
            CheckBooterState();
        });
        ScaleButton(localScale, 0.05f);
        booster.amoutEnergy -= energy;
        booster.CheckBoosterState();
        UseBooster();
    }

    void CheckTutorial(bool isActive)
    {
        UIHandler.instance.tutorial.TutorialButtonBooserBoom(isActive, this);
        UIHandler.instance.tutorial.TutorialButtonBooserSaw(isActive, this);
        UIHandler.instance.tutorial.TutorialButtonBooserFlame(isActive, this);
        UIHandler.instance.tutorial.TutorialButtonBooserMachineGun(isActive, this);
    }

    public virtual void UseBooster() { }

    public void ButtonActive(bool isActive)
    {
        frame.raycastTarget = isActive;
    }

    public void CheckBooterState()
    {
        if (isUseBooster) return;
        if (booster.amoutEnergy < energy)
        {
            UIHandler.instance.BoosterButtonChangeState(frame, false, this is BoomBooster);
        }
        else
        {
            CheckTutorial(true);
            UIHandler.instance.BoosterButtonChangeState(frame, true, this is BoomBooster);
        }
    }
}
