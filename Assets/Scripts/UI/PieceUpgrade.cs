using TMPro;
using UnityEngine;

public class PieceUpgrade : MonoBehaviour
{
    public int amout;
    public TextMeshProUGUI textAmout;

    public void SetAmout(int amout)
    {
        this.amout = amout;
        textAmout.text = amout.ToString();
    }
}
