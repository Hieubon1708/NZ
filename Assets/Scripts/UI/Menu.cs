using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public RectTransform[] options;
    public Image[] optionImages;

    float startSizeX;
    float startSizeY;

    float bigSizeX;
    float bigSizeY;

    float minSizeX;

    bool isChangingOptions;

    public Sprite ok;
    public Sprite nOk;

    public GameObject battle;
    public GameObject inventory;
    public GameObject shop;
    public GameObject battleGamePlay;

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
    }

    public void InventoryActive(bool isActive)
    {
        inventory.SetActive(isActive);
        EquipmentController.instance.DesignUpdatePosition();
    }

    public void WeaponActive(bool isActive)
    {

    }

    public void BoosActive(bool isActive)
    {

    }

    public void ShopActive(bool isActive)
    {
        shop.SetActive(isActive);
        UIHandler.instance.summonEquipment.UpdateText();
        UIHandler.instance.summonEquipment.CheckButtonState();
    }

    public void OnClick(int index)
    {
        ScaleOption(index, 0.15f);
    }
}
