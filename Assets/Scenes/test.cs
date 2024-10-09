using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public int a;

    void Start()
    {
        string text = "ABCD";
        string coloredText = "<color=red>" + text.Substring(0, 2) + "</color>" + text.Substring(2);

        textMeshPro.text = coloredText;
    }
}
