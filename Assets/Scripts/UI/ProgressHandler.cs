using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHandler : MonoBehaviour
{
    public Image panelReward;
    public RectTransform rewardPopup;
    public GameObject[] chestsInPopup;
    public Image panelConvert;
    public RectTransform convertPopup;
    public Image panelLose;
    public RectTransform losePopup;
    public GameObject parent;
    public Image progress;

    float[] progressValueBar = new float[5] { 0.178f, 0.37f, 0.56f, 0.753f, 0.948f };

    public EquipmentInfo[] equips;
    public TextMeshProUGUI[] textDesigns;
    public TextMeshProUGUI textDush;
    public TextMeshProUGUI textGem;
    public TextMeshProUGUI textKey;
    public TextMeshProUGUI textCogwheel;
    public TextMeshProUGUI textGoldReward;
    public TextMeshProUGUI textGoldConvert;
    public TextMeshProUGUI textDushConverted;

    public int gold;
    public TextMeshProUGUI textGold;
    public GameObject[] chests;
    public GameObject[] chestCompleteds;
    public List<int> progresses;

    public List<EquipRewardConfig> equipRewards = new List<EquipRewardConfig>();
    public int[] des = new int[4];
    public int dush, gem, key, cogwheel;

    Coroutine lauchProgress;

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.progresses != null)
        {
            progresses = DataManager.instance.dataStorage.progresses.ToList();
            CheckChestCompleted();
        }
    }

    void CheckChestCompleted()
    {
        for (int i = 0; i < progresses.Count; i++)
        {
            if (progresses[i] % 2 == 0)
            {
                chests[i].SetActive(false);
            }
        }
        if (GameController.instance.level == 0) chests[0].SetActive(false);
    }

    public void ShowLose()
    {
        textGoldReward.text = gold.ToString();
        panelLose.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelLose, losePopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HideLose()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelLose, losePopup, 0f, 0f, 0.8f, 0f);
        panelLose.gameObject.SetActive(false);
        UIHandler.instance.DoLayerCover(1f, 0.75f, delegate
        {
            UIHandler.instance.DoLayerCover(0f, 0.75f, delegate
            {
                UIHandler.instance.uIEffect.FlyGold();
                GameController.instance.isLose = false;
            });
            GameController.instance.Restart();
        });
    }

    public void X2Gold()
    {
        PlayerController.instance.player.PlusGold(gold);
        HideLose();
    }

    public void ShowConvert(int totalGold)
    {
        textGoldConvert.text = totalGold.ToString();
        textDushConverted.text = (totalGold / 1000).ToString();
        panelConvert.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelConvert, convertPopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HideConvert()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelConvert, convertPopup, 0f, 0f, 0.8f, 0f);
        panelConvert.gameObject.SetActive(false);
        DOVirtual.DelayedCall(0.5f, delegate
        {
            UIHandler.instance.DoLayerCover(1f, 0.75f, delegate
            {
                UIHandler.instance.map.ShowMap();
            });
        });
    }

    void ChestsInpopupActive(bool isActive)
    {
        chestsInPopup[0].SetActive(isActive);
        chestsInPopup[1].SetActive(!isActive);
    }

    public void ShowReward()
    {
        if (equipRewards.Count == 0) return;
        if (!UIHandler.instance.tutorial.isUnlockInventory)
        {
            UIHandler.instance.tutorial.isUnlockInventory = true;
            UIHandler.instance.menu.CheckDisplayButtonPage();
        }
        if (EnemyTowerController.instance.indexTower == EnemyTowerController.instance.towers.Length - 1) ChestsInpopupActive(false);
        else ChestsInpopupActive(true);
        for (int i = 0; i < equips.Length; i++)
        {
            if (i < equipRewards.Count)
            {
                equips[i].gameObject.SetActive(true);
            }
        }
        EquipmentController.instance.SortEquip();
        for (int i = 0; i < textDesigns.Length; i++)
        {
            if (des[i] != 0 && i < des.Length)
            {
                textDesigns[i].transform.parent.gameObject.SetActive(true);
                textDesigns[i].text = des[i].ToString();
            }
        }

        if (dush > 0)
        {
            textDush.text = dush.ToString();
            textDush.transform.parent.gameObject.SetActive(true);
        }
        if (gem > 0)
        {
            textGem.text = gem.ToString();
            textGem.transform.parent.gameObject.SetActive(true);
        }
        if (cogwheel > 0)
        {
            textCogwheel.text = cogwheel.ToString();
            textCogwheel.transform.parent.gameObject.SetActive(true);
        }
        if (key > 0)
        {
            textKey.text = key.ToString();
            textKey.transform.parent.gameObject.SetActive(true);
        }

        panelReward.gameObject.SetActive(true);
        UIHandler.instance.uIEffect.ScalePopup(panelReward, rewardPopup, 222f / 255f, 0.1f, 1f, 0.5f);
    }

    public void HideReward()
    {
        UIHandler.instance.uIEffect.ScalePopup(panelReward, rewardPopup, 0f, 0f, 0.8f, 0f);
        panelReward.gameObject.SetActive(false);
        for (int i = 0; i < equips.Length; i++)
        {
            equips[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textDesigns.Length; i++)
        {
            textDesigns[i].transform.parent.gameObject.SetActive(false);
        }
        textKey.transform.parent.gameObject.SetActive(false);
        textCogwheel.transform.parent.gameObject.SetActive(false);
        textGem.transform.parent.gameObject.SetActive(false);
        textDush.transform.parent.gameObject.SetActive(false);
        equipRewards.Clear();
        des = new int[4];
        dush = 0; gem = 0; key = 0; cogwheel = 0;
        if (EnemyTowerController.instance.indexTower == EnemyTowerController.instance.towers.Length - 1)
        {
            //UIHandler.instance.SetActiveProgressNGem(true);
            StartCoroutine(BlockController.instance.EndGame());
        }
    }

    public void ChestReward(int indexTower)
    {
        if (indexTower % 2 == 0 && !progresses.Contains(indexTower))
        {
            chestCompleteds[indexTower / 2].SetActive(true);
            progresses.Add(indexTower);
            if (indexTower / 2 == 0 && GameController.instance.level == 0) return;
            RewardConfig rewardConfig = DataManager.instance.rewardConfigs[GameController.instance.level];
            RewardLevelConfig rewardLevelConfig = rewardConfig.rewardLevelConfigs[indexTower / 2];

            for (int i = 0; i < rewardLevelConfig.equips.Length; i++)
            {
                EquipmentController.instance.SetEquip(rewardLevelConfig.equips[i].type, rewardLevelConfig.equips[i].level, equips[equipRewards.Count]);
                EquipmentController.instance.AddEquip(rewardLevelConfig.equips[i].type, rewardLevelConfig.equips[i].level);
                equipRewards.Add(rewardLevelConfig.equips[i]);
            }
            for (int i = 0; i < rewardLevelConfig.desgins.Length; i++)
            {
                des[i] += rewardLevelConfig.desgins[i];
            }

            dush += rewardLevelConfig.dush;
            gem += rewardLevelConfig.gem;
            key += rewardLevelConfig.key;
            cogwheel += rewardLevelConfig.cogwheel;

            EquipmentController.instance.playerInventory.dush += rewardLevelConfig.dush;
            UIHandler.instance.PlusGem(rewardLevelConfig.gem);
            EquipmentController.instance.playerInventory.cogwheel += rewardLevelConfig.cogwheel;
            EquipmentController.instance.playerInventory.key += rewardLevelConfig.key;
        }
    }

    public void Restart()
    {
        StopProgress();
        for (int i = 0; i < chests.Length; i++)
        {
            chests[i].SetActive(true);
            chestCompleteds[i].SetActive(false);
        }
    }

    public void StartGame()
    {
        gold = 0;
        textGold.text = gold.ToString();
        if (!GameController.instance.isPLayBoss)
        {
            progress.fillAmount = 0;
            StartLauchProgress();
        }
        CheckChestCompleted();
    }

    public void StartLauchProgress()
    {
        if(lauchProgress != null) StopCoroutine(lauchProgress);
        lauchProgress = StartCoroutine(LaunchProgress());
    }

    public void StopProgress()
    {
        if (lauchProgress != null) StopCoroutine(lauchProgress);
    }

    IEnumerator LaunchProgress()
    {
        Transform target = EnemyTowerController.instance.scTowers[EnemyTowerController.instance.indexTower].col.transform;
        float distanceCol = Mathf.Abs(CarController.instance.transform.position.x - GameController.instance.colStopTower.transform.position.x);
        float distance = Mathf.Abs(CarController.instance.transform.position.x - target.position.x) - distanceCol;
        float startPos = target.position.x - distanceCol + 2f;
        float targetFill = progressValueBar[EnemyTowerController.instance.indexTower];
        float previousValueBar = progress.fillAmount;
        float totalValueBar = progressValueBar[EnemyTowerController.instance.indexTower] - previousValueBar;
        while (progress.fillAmount <= targetFill)
        {
            float distanceLaunch = Mathf.Abs(target.position.x - distanceCol - startPos);
            float percentage = distanceLaunch / distance * 100;
            progress.fillAmount = Mathf.Lerp(progress.fillAmount, percentage * totalValueBar / 100 + previousValueBar, 0.1f);
            yield return new WaitForFixedUpdate();
        }
    }

    public void PlusGoldInProgress(int gold)
    {
        this.gold += gold;
        if (UIHandler.instance.goldRewardHighest < this.gold) UIHandler.instance.goldRewardHighest = this.gold;
        PlayerController.instance.player.PlusGold(gold);
    }
}
