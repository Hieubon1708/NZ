using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float hp;
    public TextMeshProUGUI textHp;

    public void Start()
    {
        ChangeTextHp();
    }

    void ChangeTextHp()
    {
        textHp.text = hp >= 1000 ? Mathf.Floor(hp / 100) / 10 + " K" : hp.ToString();
    }

    public float SubstractHp(float hp)
    {
        this.hp -= hp;
        if (this.hp < 0) this.hp = 0;
        ChangeTextHp();
        return this.hp;
    }
}
