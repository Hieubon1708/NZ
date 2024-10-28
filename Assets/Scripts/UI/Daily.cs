using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Daily : MonoBehaviour
{
    public GameObject daily;
    public GameObject dailyCompleted;
    public CanvasGroup canvasDaily;
    public TextMeshProUGUI content;
    public TextMeshProUGUI textGemReward;
    public TextMeshProUGUI indexCurrentDaily;

    List<DailyConfig> dailyOfDate = new List<DailyConfig>();
    public DateTime lastUpdateDate;
    public int indexDaily;
    public int amount;

    public DailyDataStorage GetData()
    {
        return new DailyDataStorage(dailyOfDate, lastUpdateDate, indexDaily, amount);
    }

    public void CheckDaily(DailyType type)
    {
        if (!daily.activeSelf || type != dailyOfDate[indexDaily].dailyType || amount == dailyOfDate[indexDaily].amountTarget) return;
        amount++;
        if(amount == dailyOfDate[indexDaily].amountTarget)
        {
            dailyCompleted.SetActive(true);
        }
    }

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.dailyDataStorage != null)
        {
            dailyOfDate = DataManager.instance.dataStorage.dailyDataStorage.dailyOfDate;
            amount = DataManager.instance.dataStorage.dailyDataStorage.amount;
            lastUpdateDate = DataManager.instance.dataStorage.dailyDataStorage.lastUpdateDate;
        }
        InvokeRepeating("CheckAndUpdateDaily", 0f, 60f);
    }

    void CheckAndUpdateDaily()
    {
        DateTime currentDate = DateTime.Now;
        if (currentDate.Date > lastUpdateDate.Date)
        {
            RandomDaily();
            lastUpdateDate = currentDate;
        }
        UpdateDaily();
    }

    void RandomDaily()
    {
        dailyOfDate.Clear();
        List<DailyConfig> dailyConfigs = new List<DailyConfig>(DataManager.instance.dailyConfigs);
        int i = 0;
        while (i < 3)
        {
            int indexRandom = Random.Range(0, dailyConfigs.Count);
            dailyOfDate.Add(dailyConfigs[indexRandom]);
            dailyConfigs.RemoveAt(indexRandom);
            i++;
        }
        dailyOfDate.Add(new DailyConfig(DailyType.WatchAds, 5, "Watch Ads", 10));
        dailyOfDate.Add(new DailyConfig(DailyType.CompleteLevel, 1, "Complete Level", 10));
        amount = 0;
        indexDaily = 0;
    }

    public enum DailyType
    {
        None, DestroyEnemy, CumulativeEnergy, UseGrenade, DestroyTower, WatchAds, CompleteLevel
    }

    public void RewardDaily()
    {
        EquipmentController.instance.playerInventory.gem += dailyOfDate[indexDaily].gemReward;
        UIHandler.instance.uIEffect.FlyGem();
    }

    void UpdateDaily()
    {
        indexCurrentDaily.text = "daily " + (indexDaily + 1) + "/" + dailyOfDate.Count;
        textGemReward.text = dailyOfDate[indexDaily].gemReward.ToString();
        content.text = dailyOfDate[indexDaily].content + " " + amount + "/" + dailyOfDate[indexDaily].amountTarget;
    }

    public void HideDaily()
    {
        canvasDaily.alpha = 0;
        dailyCompleted.SetActive(false);
        if(indexDaily < dailyOfDate.Count)
        {
            amount = 0;
            indexDaily++;
            UpdateDaily();
        }
    }

    public void ChangeDaily()
    {
        if(indexDaily == dailyOfDate.Count)
        {
            canvasDaily.DOFade(1, 0.5f);
        }
    }

    private void OnDestroy()
    {
        //canvasDaily.DOKill();
    }
}
