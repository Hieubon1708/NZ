using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public TextMeshProUGUI textGold;

    public Sprite[] frameButtonWeaponBuyers;
    public Sprite[] frameGoldWeaponBuyers;

    public Sprite[] frameButtonBlockUpgradees;
    public Sprite[] frameButtonWeaponUpgradees;
    public Sprite[] frameButtonWeaponLastUpgradees;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        LoadData();
    }

    void LoadData()
    {
        GoldUpdatee();
    }

    public void GoldUpdatee()
    {
        textGold.text = DataManager.instance.playerData.gold.ToString();
    }

    public enum Type
    {
        ENOUGH_MONEY, NOT_ENOUGH_MONEY
    }

    public void ChangeSpriteWeaponBuyer(Type type, Image frame, Image frameGold)
    {
        int index = 0;
        if (type == Type.NOT_ENOUGH_MONEY) index = 1;
        frame.sprite = frameButtonWeaponBuyers[index];
        frameGold.sprite = frameGoldWeaponBuyers[index];
    }
    
    public void ChangeSpriteWeaponUpgradee(Type type, Image frame)
    {
        int index = 0;
        if (type == Type.NOT_ENOUGH_MONEY) index = 1;
        frame.sprite = frameButtonWeaponUpgradees[index];
    }
    
    public void ChangeSpriteWeaponLastUpgradee(Type type, Image frame)
    {
        int index = 0;
        if (type == Type.NOT_ENOUGH_MONEY) index = 1;
        frame.sprite = frameButtonWeaponLastUpgradees[index];
    }

    public void ChangeSpriteWeaponUpgradee(Image frame, TextMeshProUGUI textPrice, TextMeshProUGUI textMax)
    {
        textPrice.gameObject.SetActive(false);
        textMax.gameObject.SetActive(true);
        frame.sprite = frameButtonWeaponUpgradees[2];
    }

    public void ChangeSpriteBlockUpgradee(Image frame, TextMeshProUGUI textPrice, TextMeshProUGUI textMax)
    {
        textPrice.gameObject.SetActive(false);
        textMax.gameObject.SetActive(true);
        frame.sprite = frameButtonBlockUpgradees[2];
    }

    public void ChangeSpriteBlockUpgradee(Type type, Image frame)
    {
        int index = 0;
        if (type == Type.NOT_ENOUGH_MONEY) index = 1;
        frame.sprite = frameButtonBlockUpgradees[index];
    }
}
