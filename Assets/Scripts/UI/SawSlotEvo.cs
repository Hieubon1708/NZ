using TMPro;
using UnityEngine;
using static UpgradeEvolutionController;

public class SawSlotEvo : MonoBehaviour
{
    public SAWEVO type;
    public int maxLevel;
    public TextMeshProUGUI textAmout;

    public void SetAmout(int amout)
    {
        if(amout > 1)
        {
            if(!textAmout.transform.parent.gameObject.activeSelf) textAmout.transform.parent.gameObject.SetActive(true);
        }
        
        amout = Mathf.Clamp(amout, 0, maxLevel);
        textAmout.text = amout.ToString();
    }
}
