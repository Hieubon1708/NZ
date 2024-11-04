using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBooster : MonoBehaviour
{
    public int energy;
    public Booster booster;
    public Image frame;
    public TextMeshProUGUI textEnergy;
    public bool isUseBooster;

    public void UpdateTextEnergy()
    {
        textEnergy.text = energy.ToString();
    }

    public void SubtractEnergy(float percentage)
    {
        energy -= Mathf.RoundToInt(energy * percentage / 100);
        UpdateTextEnergy();
    }


    public void OnClick()
    {
        CheckTutorial(false, true);
        isUseBooster = true;
        frame.sprite = booster.frameDelay;
        frame.raycastTarget = false;
        booster.amoutEnergy -= energy;
        booster.CheckBoosterState();
        UseBooster();
    }

    void CheckTutorial(bool isActive, bool isEnoughEnergy)
    {
        UIHandler.instance.tutorial.TutorialButtonBooserBoom(isActive, this, isEnoughEnergy);
        UIHandler.instance.tutorial.TutorialButtonBooserSaw(isActive, this, isEnoughEnergy);
        UIHandler.instance.tutorial.TutorialButtonBooserFlame(isActive, this, isEnoughEnergy);
        UIHandler.instance.tutorial.TutorialButtonBooserMachineGun(isActive, this, isEnoughEnergy);
    }

    public virtual void UseBooster() { }
    public virtual void Restart()
    {
        isUseBooster = false;
    }

    public void ButtonActive(bool isActive)
    {
        frame.raycastTarget = isActive;
    }

    public void CheckBooterState()
    {
        if (isUseBooster) return;
        if (booster.amoutEnergy < energy)
        {
            CheckTutorial(false, false);
            UIHandler.instance.BoosterButtonChangeState(frame, false, this is BoomBooster);
        }
        else
        {
            CheckTutorial(true, true);
            UIHandler.instance.BoosterButtonChangeState(frame, true, this is BoomBooster);
        }
    }
}
