using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public RectTransform[] options;
    public Image[] optionImages;

    public GameObject[] notifOptions;
    public GameObject[] lockOptions;

    float startSizeX;
    float startSizeY;

    float bigSizeX;
    float bigSizeY;

    float minSizeX;

    bool isChangingOptions;

    Sprite ok;
    Sprite nOk;

    public GameObject battle;
    public GameObject inventory;
    public GameObject shop;
    public GameObject boss;
    public GameObject weapon;
    public GameObject battleGamePlay;
    public GameObject battleWorld;

    public SpriteAtlas spriteAtlas;

    private void Awake()
    {
        ok = spriteAtlas.GetSprite("Button_Chosen_section_1 1");
        nOk = spriteAtlas.GetSprite("Button_Chosen_section_2 1");
    }

    public void CheckDisplayButtonPage()
    {
        LoadData();
    }

    public void LoadData()
    {
        if (UIHandler.instance.tutorial.isUnlockInventory)
        {
            lockOptions[0].SetActive(false);
            optionImages[1].raycastTarget = true;
        }
        else optionImages[1].raycastTarget = false;
        if (UIHandler.instance.tutorial.isUnlockShop)
        {
            lockOptions[1].SetActive(false);
            optionImages[0].raycastTarget = true;
        }
        else optionImages[0].raycastTarget = false;
        if (UIHandler.instance.tutorial.isUnlockBoss)
        {
            lockOptions[2].SetActive(false);
            optionImages[3].raycastTarget = true;
        }
        else optionImages[3].raycastTarget = false;
        if (UIHandler.instance.tutorial.isUnlockWeapon)
        {
            lockOptions[3].SetActive(false);
            optionImages[4].raycastTarget = true;
        }
        else optionImages[4].raycastTarget = false;
    }

    public void Start()
    {
        startSizeX = 1080 / options.Length;
        if (GameController.instance.cam.aspect >= 0.55f)
        {
            startSizeX += startSizeX - startSizeX * ((float)1920 / Screen.height);
        }

        startSizeY = options[0].sizeDelta.y;

        bigSizeX = startSizeX * 1.2f;
        bigSizeY = startSizeY * 1f;

        minSizeX = (1080 - bigSizeX) / (options.Length - 1);

        if (GameController.instance.cam.aspect >= 0.55f)
        {
            float screen = 1080 + (1080 - 1080 * ((float)1920 / Screen.height));
            minSizeX = (screen - bigSizeX) / (options.Length - 1);
        }

        ScaleOption(2, 0f);
    }

    void ScaleOption(int index, float duration)
    {
        if (isChangingOptions) return;

        isChangingOptions = true;

        for (int i = 0; i < options.Length; i++)
        {
            if (i == index)
            {
                ChangeOption(true, i);
                options[i].DOSizeDelta(new Vector2(bigSizeX, bigSizeY), duration).SetEase(Ease.Linear).OnComplete(delegate { isChangingOptions = false; });
                optionImages[i].sprite = ok;
            }
            else
            {
                ChangeOption(false, i);
                options[i].DOSizeDelta(new Vector2(minSizeX, startSizeY), duration).SetEase(Ease.Linear);
                optionImages[i].sprite = nOk;
            }
        }
    }

    void ChangeOption(bool isActive, int index)
    {
        if (index == 2) BattleActive(isActive);
        else if (index == 1) InventoryActive(isActive);
        else if (index == 3) WeaponActive(isActive);
        else if (index == 4) BoosActive(isActive);
        else ShopActive(isActive);
    }

    public void BattleActive(bool isActive)
    {
        battle.SetActive(isActive);
        battleGamePlay.SetActive(isActive);
        battleWorld.SetActive(isActive);
    }

    public void InventoryActive(bool isActive)
    {
        inventory.SetActive(isActive);
        if (isActive)
        {
            UIHandler.instance.tutorial.TutorialButtonInventory(true);
            EquipmentController.instance.DesignUpdatePosition();
        }
    }

    public void WeaponActive(bool isActive)
    {

    }

    public void BoosActive(bool isActive)
    {
        boss.SetActive(isActive);
    }

    public void ShopActive(bool isActive)
    {
        shop.SetActive(isActive);
        if (isActive)
        {
            UIHandler.instance.summonEquipment.UpdateText();
            UIHandler.instance.summonEquipment.CheckButtonState();
            UIHandler.instance.tutorial.TutorialButtonShop(true);
        }
    }

    public void OnClick(int index)
    {
        ScaleOption(index, 0.15f);
    }
}
