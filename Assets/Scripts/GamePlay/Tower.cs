using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int hp;
    public TextMeshProUGUI textHp;

    public void Start()
    {
        ChangeTextHp();
    }

    public void ChangeTextHp()
    {
        textHp.text = UIHandler.instance.ConvertNumberAbbreviation(hp).ToString();
    }

    public float SubstractHp(int hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        ChangeTextHp();
        return this.hp;
    }
}
