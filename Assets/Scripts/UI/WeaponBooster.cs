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
        if (booster.amoutEnergy < energy) return;
        currentObjectSelected = eventData.pointerCurrentRaycast.gameObject;
        ScaleButton(localScale * 0.95f, 0.05f);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (booster.amoutEnergy < energy) return;
        ScaleButton(localScale, 0.05f);
        if (eventData.pointerCurrentRaycast.gameObject == currentObjectSelected)
        {
            booster.amoutEnergy -= energy;
            booster.CheckBoosterState();
            UseBooster();
        }
    }

    public virtual void UseBooster() { }

    public void CheckBooterState()
    {
        if (booster.amoutEnergy < energy)
        {
            UIHandler.instance.BoosterButtonChangeState(frame, false, this is BoomBooster);
        }
        else
        {
            UIHandler.instance.BoosterButtonChangeState(frame, true, this is BoomBooster);
        }
    }
}
