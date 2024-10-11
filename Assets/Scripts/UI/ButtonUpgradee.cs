using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonUpgradee : MonoBehaviour
{
    public TextMeshProUGUI textPriceUpgrade;
    public Image frame;
    public Image framePrice;    
    public Image arrow;

    public abstract void Upgrade();
    public abstract void UpgradeHandle();
    public abstract void CheckButtonState();
}
