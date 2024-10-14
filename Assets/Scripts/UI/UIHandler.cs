using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public ProgressHandler progressHandler;
    public SummonEquipment summonEquipment;

    public GameObject gold;
    public TextMeshProUGUI textGold;

    public Sprite[] frameButtonWeaponBuyers;
    public Sprite[] frameButtonWeaponUpgradees;
    public Sprite[] frameButtonWeaponEvoUpgradees;

    public Sprite[] frameButtonBlockUpgradees;
    public Sprite[] frameButtonEnergyUpgradees;

    public Sprite[] frameButtonBooster;
    public Sprite[] frameButtonBoomBooster;

    public Color framePriceNok;
    public Color framePriceOk;
    public Color framePriceMax;
    public Color textOk;
    public Color textNOk;
    public Color boxOk;
    public Color boxNOk;
    public Color arrowOk;
    public Color arrowNOk;

    public void Awake()
    {
        instance = this;
    }

    public void LoadData()
    {
        GoldUpdatee();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void AddGold()
    {
        PlayerController.instance.player.gold += 100000;
        GoldUpdatee();
    }

    public void GoldUpdatee()
    {
        textGold.text = ConvertNumberAbbreviation(PlayerController.instance.player.gold);
    }

    public enum Type
    {
        ENOUGH_MONEY, NOT_ENOUGH_MONEY
    }

    void BoxColorChange(Image[] boxes, Color color)
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].color = color;
        }
    }

    public void BoosterButtonChangeState(Image frame, bool isOk, bool isBoom)
    {
        if (isBoom)
        {
            if (isOk) frame.sprite = frameButtonBoomBooster[0];
            else frame.sprite = frameButtonBoomBooster[1];
        }
        else
        {
            if (isOk) frame.sprite = frameButtonBooster[0];
            else frame.sprite = frameButtonBooster[1];
        }
    }

    public void WeaponEvoButtonChangeState(Type type, Image frame, Image framePrice, Image arrow)
    {
        int index;
        GetIndexNSetColorNAlpha(type, out index, framePrice);
        if (type == Type.NOT_ENOUGH_MONEY) arrow.color = arrowNOk;
        else arrow.color = arrowNOk;
        frame.sprite = frameButtonWeaponEvoUpgradees[index];
    }

    public void WeaponButtonChangeState(Type type, Image frame, Image framePrice)
    {
        int index = 0;
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            index = 1;
            framePrice.color = framePriceNok;
        }
        else
        {
            framePrice.color = framePriceOk;
        }
        frame.sprite = frameButtonWeaponBuyers[index];
    }

    public void WeaponButtonChangeState(Type type, Image frame, Image framePrice, TextMeshProUGUI textLv, TextMeshProUGUI textDamageL, TextMeshProUGUI textDamageR, Image[] boxes, Image arrow)
    {
        int index;
        GetIndexNSetColorNAlpha(type, out index, framePrice);
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            TextChangeColor(textLv, textDamageL, textDamageR, textNOk);
            BoxColorChange(boxes, boxNOk);
            arrow.color = arrowNOk;
        }
        else
        {
            TextChangeColor(textLv, textDamageL, textDamageR, textOk);
            BoxColorChange(boxes, boxOk);
            arrow.color = arrowOk;
        }
        frame.sprite = frameButtonWeaponUpgradees[index];
    }

    public void WeaponButtonChangeState(Image frame, TextMeshProUGUI textPrice, TextMeshProUGUI textMax)
    {
        textPrice.gameObject.SetActive(false);
        textMax.gameObject.SetActive(true);
        frame.sprite = frameButtonWeaponUpgradees[2];
    }

    public void BlockButtonChangeState(Image frame, Image framePrice, TextMeshProUGUI textPrice, TextMeshProUGUI textMax, Image iconGold, TextMeshProUGUI textLv, TextMeshProUGUI textHpL, TextMeshProUGUI textHpR)
    {
        textPrice.gameObject.SetActive(false);
        iconGold.gameObject.SetActive(false);
        textMax.gameObject.SetActive(true);

        frame.sprite = frameButtonBlockUpgradees[2];

        framePrice.color = framePriceMax;

        TextChangeColor(textLv, textHpL, textHpR, textNOk);
    }

    public void BlockButtonChangeState(Type type, Image frame, Image framePrice)
    {
        int index;
        GetIndexNSetColorNAlpha(type, out index, framePrice);
        frame.sprite = frameButtonBlockUpgradees[index];
    }

    public void BlockButtonChangeState(Type type, Image frame, Image framePrice, TextMeshProUGUI textLv, TextMeshProUGUI textHpL, TextMeshProUGUI textHpR)
    {
        int index;
        GetIndexNSetColorNAlpha(type, out index, framePrice);
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            TextChangeColor(textLv, textHpL, textHpR, textNOk);
        }
        else
        {
            TextChangeColor(textLv, textHpL, textHpR, textOk);
        }
        frame.sprite = frameButtonBlockUpgradees[index];
    }

    public void EnergyButtonChangeState(Type type, Image frame, Image framePrice, Image arrow)
    {
        int index;
        GetIndexNSetColorNAlpha(type, out index, framePrice);
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            arrow.color = arrowNOk;
        }
        else
        {
            arrow.color = arrowOk;
        }
        frame.sprite = frameButtonEnergyUpgradees[index];
    }

    void GetIndexNSetColorNAlpha(Type type, out int index, Image framePrice)
    {
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            index = 1;
            framePrice.color = framePriceNok;
        }
        else
        {
            index = 0;
            framePrice.color = framePriceOk;
        }
    }

    void TextColorChange(TextMeshProUGUI text, Color color)
    {
        text.color = new Vector4(color.r, color.g, color.b, text.color.a);
    }

    void TextChangeColor(TextMeshProUGUI textLv, TextMeshProUGUI textHpL, TextMeshProUGUI textHpR, Color color)
    {
        TextColorChange(textLv, color);
        TextColorChange(textHpL, color);
        TextColorChange(textHpR, color);
    }

    public string ConvertNumberAbbreviation(int number)
    {
        if (number >= 1000000000)
        {
            return (number / 1000000f).ToString(number >= 1100000 ? "F1" : "F0") + "G";
        }
        else if (number >= 1000000)
        {
            return (number / 1000000f).ToString(number >= 1100000 ? "F1" : "F0") + "M";
        }
        else if (number >= 1000)
        {
            return (number / 1000f).ToString(number >= 1100 ? "F1" : "F0") + "K";
        }
        else
        {
            return number.ToString();
        }
    }
}
