using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public RectTransform[] options;
    public Image[] optionImages;
    public RectTransform canvas;

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
    public GameObject battleGameplay;
    public GameObject inventory;
    public GameObject shop;
    public GameObject boss;
    public GameObject weapon;
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

    public void CheckNotifAll()
    {
        EquipmentController.instance.CheckStateNofi();
        UIHandler.instance.summonEquipment.CheckNotif();
        UIBoss.instance.CheckNotif();
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

    public void ScaleOption(int index, float duration)
    {
        if(startSizeX == 0)
        {
            startSizeX = canvas.sizeDelta.x / options.Length;
            startSizeY = options[0].sizeDelta.y;

            bigSizeX = startSizeX * 1.2f;
            bigSizeY = startSizeY * 1f;

            minSizeX = (canvas.sizeDelta.x - bigSizeX) / (options.Length - 1);
        }

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
        battleGameplay.SetActive(isActive);
        battleWorld.SetActive(isActive);
        if (UIHandler.instance.tutorial.isTutorialDragBlock) UIHandler.instance.tutorial.blockDragTutorial.SetActive(isActive);
    }

    public void InventoryActive(bool isActive)
    {
        if (isActive)
        {
            EquipmentController.instance.playerInventory.UpdateTextCogwheel();
            EquipmentController.instance.playerInventory.UpdateTextDush();
        }
        inventory.SetActive(isActive);
        if (isActive)
        {
            UIHandler.instance.tutorial.TutorialButtonInventory(true);
        }
    }

    public void WeaponActive(bool isActive)
    {

    }

    public void BoosActive(bool isActive)
    {
        if (isActive)
        {
            UIBoss.instance.UpdatePanel();
        }
        boss.SetActive(isActive);
        if (isActive)
        {
            UIHandler.instance.tutorial.TutorialButtonBoss(true);
        }
    }

    public void ShopActive(bool isActive)
    {
        if (isActive)
        {
            UIHandler.instance.summonEquipment.UpdateText();
            UIHandler.instance.summonEquipment.CheckButtonState();
        }
        shop.SetActive(isActive);
        if (isActive)
        {
            UIHandler.instance.tutorial.TutorialButtonShop(true);
        }
    }

    public void OnClick(int index)
    {
        AudioController.instance.PlaySoundButton(AudioController.instance.buttonClick);
        ScaleOption(index, 0.15f);
    }
}
