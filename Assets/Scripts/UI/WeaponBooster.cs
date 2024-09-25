using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponBooster : ButtonClicker
{
    public int energy;
    public Booster booster;
    public TextMeshProUGUI num;
    public Image frame;

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
            frame.color = new Vector4(0.5f, 0.5f, 0.5f, 1);
            num.color = new Vector4(0.5f, 0.5f, 0.5f, 1);
        }
        else
        {
            frame.color = Vector4.one;
            num.color = Vector4.one;
        }
    }
}
