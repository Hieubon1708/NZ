using TMPro;
using UnityEngine;

public class QualityNClass : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void OnClick(bool isQuality)
    {
        isQuality = !isQuality;
        if (isQuality)
        {
            text.text = "Quality";
        }
        else
        {
            text.text = "Class";
        }
    }
}
