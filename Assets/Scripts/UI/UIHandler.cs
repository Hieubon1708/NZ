using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public RectTransform poolUIs;
    public ProgressHandler progressHandler;
    public SummonEquipment summonEquipment;
    public UIEffect uIEffect;
    public Setting setting;
    public Tutorial tutorial;
    public Menu menu;
    public Daily daily;
    public Map map;

    public int goldRewardHighest;
    public TextMeshProUGUI textRewardGold;
    public DateTime lastRewardTime;
    public Image frameRewardGold;
    public TextMeshProUGUI mapInfo;
    public TextMeshProUGUI mapInfoBot;
    public GameObject goldFlyPrefab;
    public GameObject[] goldFlies;
    public int amout;
    public Transform targetFlyGold;
    int curentCountFlyGold;

    public GameObject gold;
    public GameObject iconGold;
    public GameObject goldStart;
    public GameObject iconGoldStart;
    public GameObject gem;
    public GameObject adsReward;
    public TextMeshProUGUI textGold;
    public TextMeshProUGUI textGem;

    Sprite[] frameButtonWeaponBuyers = new Sprite[2];
    Sprite[] frameButtonWeaponUpgradees = new Sprite[3];
    Sprite[] frameButtonWeaponEvoUpgradees = new Sprite[2];
    Sprite[] frameButtonBlockUpgradees = new Sprite[3];
    Sprite[] frameButtonEnergyUpgradees = new Sprite[2];
    Sprite[] frameButtonRewardGold = new Sprite[2];
    Sprite[] frameButtonBooster = new Sprite[2];
    Sprite[] frameButtonBoomBooster = new Sprite[2];

    public Color framePriceNok;
    public Color framePriceOk;
    public Color framePriceMax;
    public Color textOk;
    public Color textNOk;
    public Color boxOk;
    public Color boxNOk;
    public Color arrowOk;
    public Color arrowNOk;

    public Image layerCover;

    public SpriteAtlas spriteAtlas;
    public Coroutine countdownRewardGold;
    public int goldTemp;

    public void Awake()
    {
        instance = this;
        LoadSprites();
        Generate();
    }

    public void Start()
    {
        layerCover.gameObject.SetActive(true);
        DoLayerCover(0f, 0.75f, null);
    }

    public void SetActiveProgressNGem(bool isActive)
    {
        progressHandler.parent.SetActive(isActive);
        gem.SetActive(isActive);
        goldStart.SetActive(isActive);
    }

    void LoadSprites()
    {
        frameButtonWeaponEvoUpgradees[0] = spriteAtlas.GetSprite("Button_Upgrade_WEAPON_Evolution_1");
        frameButtonWeaponEvoUpgradees[1] = spriteAtlas.GetSprite("Button_Upgrade_WEAPON_2");
        frameButtonBoomBooster[0] = spriteAtlas.GetSprite("Button_1");
        frameButtonBoomBooster[1] = spriteAtlas.GetSprite("Button_Booster_2");
        frameButtonRewardGold[0] = spriteAtlas.GetSprite("Button_ADS_COIN");
        frameButtonRewardGold[1] = spriteAtlas.GetSprite("Button_Energy_Upgrade_2");

        for (int i = 0; i < 2; i++)
        {
            frameButtonWeaponBuyers[i] = spriteAtlas.GetSprite("Button_Weapon_buy_" + (i + 1));
            frameButtonEnergyUpgradees[i] = spriteAtlas.GetSprite("Button_Energy_Upgrade_" + (i + 1));
            frameButtonBooster[i] = spriteAtlas.GetSprite("Button_Booster_" + (i + 1));
        }
        for (int i = 0; i < 3; i++)
        {
            frameButtonBlockUpgradees[i] = spriteAtlas.GetSprite("Button_Upgrade_TOWER_" + (i + 1));
            frameButtonWeaponUpgradees[i] = spriteAtlas.GetSprite("Button_Upgrade_WEAPON_" + (i + 1));
        }
    }

    public IEnumerator CountdownRewardGold(int time)
    {
        frameRewardGold.raycastTarget = false;
        frameRewardGold.sprite = frameButtonRewardGold[1];
        adsReward.SetActive(false);
        while (time != -1)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            textRewardGold.text = timeString;
            yield return new WaitForSeconds(1);
            time--;
        }
        ActiveButtonReward();
    }

    public void UpdateTextRewardHoldHighest()
    {
        textRewardGold.text = "+" + ConvertNumberAbbreviation(goldRewardHighest);
    }

    void ActiveButtonReward()
    {
        UpdateTextRewardHoldHighest();
        frameRewardGold.raycastTarget = true;
        frameRewardGold.sprite = frameButtonRewardGold[0];
        adsReward.SetActive(true);
    }

    public void EndGame()
    {
        if (!tutorial.isFirstTimePlay) tutorial.isFirstTimePlay = true;
    }

    public void Restart()
    {
        if (!daily.daily.activeSelf && !GameController.instance.isPLayBoss)
        {
            if (tutorial.isSecondTimeDestroyTower)
            {
                daily.LoadData();
                daily.daily.SetActive(true);
            }
        }
        textRewardGold.text = "+" + ConvertNumberAbbreviation(goldRewardHighest);
        menu.CheckNotifAll();
    }

    public void DoLayerCover(float alpha, float duration, Action callback)
    {
        layerCover.DOFade(alpha, duration).OnComplete(delegate
        {
            if (callback != null) callback.Invoke();
        }).SetEase(Ease.Linear);
    }

    public void UpdateTextMap()
    {
        if (GameController.instance.level == 0) mapInfo.text = "1. Forbidden Jungle";
        else if (GameController.instance.level == 1) mapInfo.text = "2. stempunk bugs invasion";
        mapInfoBot.text = "difficulty 1." + (GameController.instance.level + 1);
    }

    public void LoadData()
    {
        UpdateTextMap();
        GoldUpdatee();
        tutorial.LoadData();
        map.LoadData();
        daily.LoadData();
        summonEquipment.LoadData();
        progressHandler.LoadData();
        setting.LoadData();
        menu.LoadData();
        GemUpdatee();
        if (daily.daily.activeSelf) gem.SetActive(true);
        if (DataManager.instance.dataStorage != null)
        {
            goldRewardHighest = DataManager.instance.dataStorage.goldRewardHighest;
            lastRewardTime = DataManager.instance.dataStorage.lastRewardTime;
        }
        CheckRewardGold();
        menu.ScaleOption(2, 0f);
    }

    public void CheckRewardGold()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeSinceLastReward = currentTime - lastRewardTime;
        if (countdownRewardGold != null) StopCoroutine(countdownRewardGold);
        if (timeSinceLastReward.TotalMinutes >= 5)
        {
            ActiveButtonReward();
        }
        else
        {
            countdownRewardGold = StartCoroutine(CountdownRewardGold(5 * 60 - (int)timeSinceLastReward.TotalSeconds));
        }
    }

    public void StartGame()
    {
        progressHandler.StartGame();
        tutorial.StartGame();
        uIEffect.KillTw();
        layerCover.DOKill();
        DoLayerCover(0f, 0f, null);
    }

    void Generate()
    {
        goldFlies = new GameObject[amout];
        for (int i = 0; i < amout; i++)
        {
            goldFlies[i] = Instantiate(goldFlyPrefab, poolUIs);
            goldFlies[i].SetActive(false);
        }
    }

    public void SetValue(bool isActive)
    {
        if(!GameController.instance.isPLayBoss)
        {
            progressHandler.parent.SetActive(isActive);
            mapInfo.transform.parent.gameObject.SetActive(!isActive);
        }
        gold.SetActive(!isActive);
        goldStart.SetActive(isActive);
    }

    public void FlyGold(Vector2 pos, int gold)
    {
        GameObject g = goldFlies[curentCountFlyGold];
        g.transform.position = pos;
        g.SetActive(true);
        curentCountFlyGold++;
        progressHandler.PlusGoldInProgress(gold);
        if (curentCountFlyGold == goldFlies.Length) curentCountFlyGold = 0;
        g.transform.DOMove(targetFlyGold.position, 0.55f).OnComplete(delegate
        {
            uIEffect.Scale(iconGoldStart);
            g.SetActive(false);
            progressHandler.textGold.text = progressHandler.gold.ToString();
        });
    }

    public void RewardGold()
    {
        lastRewardTime = DateTime.Now;
        PlayerController.instance.player.gold += goldRewardHighest;
        GoldUpdatee();
        BlockController.instance.CheckButtonStateAll();
        countdownRewardGold = StartCoroutine(CountdownRewardGold(5 * 60));
        daily.CheckDaily(Daily.DailyType.WatchAds);
    }

    public void GoldUpdatee()
    {
        textGold.text = ConvertNumberAbbreviation(PlayerController.instance.player.gold);
    }

    public void GemUpdatee()
    {
        textGem.text = ConvertNumberAbbreviation(EquipmentController.instance.playerInventory.gem);
        if(!gem.activeSelf) gameObject.SetActive(true);
    }

    public void PlusGem(int gem)
    {
        EquipmentController.instance.playerInventory.gem += gem;
        GemUpdatee();
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
        if (GameController.instance.isLose) return;
        if (isOk) frame.raycastTarget = true;
        else frame.raycastTarget = false;
    }

    public void WeaponEvoButtonChangeState(Type type, Image frame, Image framePrice, Image arrow)
    {
        int index;
        GetIndexNSetColorNAlpha(type, out index, framePrice);
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            frame.raycastTarget = false;
            arrow.color = arrowNOk;
        }
        else
        {
            arrow.color = arrowNOk;
            frame.raycastTarget = true;
        }
        frame.sprite = frameButtonWeaponEvoUpgradees[index];
    }

    public void WeaponButtonChangeState(Type type, Image frame, Image framePrice)
    {
        int index = 0;
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            frame.raycastTarget = false;
            index = 1;
            framePrice.color = framePriceNok;
        }
        else
        {
            frame.raycastTarget = true;
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
            frame.raycastTarget = false;
        }
        else
        {
            TextChangeColor(textLv, textDamageL, textDamageR, textOk);
            BoxColorChange(boxes, boxOk);
            arrow.color = arrowOk;
            frame.raycastTarget = true;
        }
        frame.sprite = frameButtonWeaponUpgradees[index];
    }

    public void WeaponButtonChangeState(Image frame, TextMeshProUGUI textPrice, TextMeshProUGUI textMax, Image framePrice, Image arrow)
    {
        textPrice.gameObject.SetActive(false);
        textMax.gameObject.SetActive(true);
        arrow.gameObject.SetActive(false);
        frame.sprite = frameButtonWeaponUpgradees[2];
        frame.raycastTarget = false;
        framePrice.color = framePriceMax;
    }

    public void BlockButtonChangeState(Image frame, Image framePrice, TextMeshProUGUI textPrice, TextMeshProUGUI textMax, Image iconGold, TextMeshProUGUI textLv, TextMeshProUGUI textHpL, TextMeshProUGUI textHpR)
    {
        textPrice.gameObject.SetActive(false);
        iconGold.gameObject.SetActive(false);
        textMax.gameObject.SetActive(true);

        frame.sprite = frameButtonBlockUpgradees[2];

        framePrice.color = framePriceMax;
        frame.raycastTarget = false;

        TextChangeColor(textLv, textHpL, textHpR, textNOk);
    }

    public void BlockButtonChangeState(Type type, Image frame, Image framePrice)
    {
        int index;
        if (type == Type.NOT_ENOUGH_MONEY)
        {
            frame.raycastTarget = false;
        }
        else
        {
            frame.raycastTarget = true;
        }
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
            frame.raycastTarget = false;
        }
        else
        {
            TextChangeColor(textLv, textHpL, textHpR, textOk);
            frame.raycastTarget = true;
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
            frame.raycastTarget = false;
        }
        else
        {
            arrow.color = arrowOk;
            frame.raycastTarget = true;
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
