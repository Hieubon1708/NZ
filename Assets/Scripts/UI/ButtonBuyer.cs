using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBuyer : MonoBehaviour
{
    public TextMeshProUGUI textPrice;
    public Image frameButton;

    public abstract void Buy();

    public abstract void LoadData();

    public abstract void CheckButtonState();
}
