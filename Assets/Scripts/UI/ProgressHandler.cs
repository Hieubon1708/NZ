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
    public Image panelLose;
    public RectTransform losePopup;
    public GameObject parent;
    public Image progress;

    public EquipmentInfo[] equips;
    public TextMeshProUGUI[] textDesigns;
    public TextMeshProUGUI textDush;
    public TextMeshProUGUI textGem;
    public TextMeshProUGUI textKey;
    public TextMeshProUGUI textCogwheel;
    public TextMeshProUGUI textGoldReward;

    public int gold;
    public TextMeshProUGUI textGold;
    public GameObject[] chests;
    public GameObject[] chestCompleteds;
    public List<int> progresses;

    public List<EquipRewardConfig> equipRewards = new List<EquipRewardConfig>();
    public int[] des = new int[4];
    int dush, gem, key, cogwheel;

    public void LoadData()
    {
        if (DataManager.instance.dataStorage.progresses != null)
        {
            progresses = DataManager.instance.dataStorage.progresses.ToList();
            for (int i = 0; i < progresses.Count; i++)
            {
                if (progresses[i] % 2 == 0)
                {
                    chests[i].SetActive(false);
                }
            }
            if (GameController.instance.level == 0) chests[0].SetActive(false);
        }
        else progresses = new List<int>();
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
            });
            GameController.instance.Restart();
        });
    }

    public void X2Gold()
    {
        PlayerController.instance.player.PlusGold(gold);
        HideLose();
    }

    public void ShowReward()
    {
        if (equipRewards.Count == 0) return;
        for (int i = 0; i < equips.Length; i++)
        {
            if (i < equipRewards.Count)
            {
                EquipmentController.instance.SetEquip(equipRewards[i].type, equipRewards[i].level, equips[i]);
                EquipmentController.instance.AddEquip(equipRewards[i].type, equipRewards[i].level);
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

        if (dush > 0) textDush.text = dush.ToString(); textDush.transform.parent.gameObject.SetActive(true);
        if (gem > 0) textGem.text = gem.ToString(); textGem.transform.parent.gameObject.SetActive(true);
        if (cogwheel > 0) textCogwheel.text = cogwheel.ToString(); textCogwheel.transform.parent.gameObject.SetActive(true);
        if (key > 0) textKey.text = key.ToString(); textKey.transform.parent.gameObject.SetActive(true);

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
            UIHandler.instance.SetActiveProgressNGem(true);
            StartCoroutine(BlockController.instance.EndGame());
        }
    }

    public void ChestReward(int indexTower)
    {
        if (indexTower % 2 == 0 && chests[indexTower / 2].activeSelf)
        {
            Debug.LogWarning(indexTower);
            chestCompleteds[indexTower / 2].SetActive(true);
            progresses.Add(indexTower);
            if (indexTower / 2 == 0 && GameController.instance.level == 0) return;
            RewardConfig rewardConfig = DataManager.instance.rewardConfigs[GameController.instance.level];
            RewardLevelConfig rewardLevelConfig = rewardConfig.rewardLevelConfigs[indexTower / 2];

            for (int i = 0; i < rewardLevelConfig.equips.Length; i++)
            {
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

            EquipmentController.instance.playerInventory.dush += dush;
            UIHandler.instance.PlusGem(gem);
            EquipmentController.instance.playerInventory.cogwheel += cogwheel;
            EquipmentController.instance.playerInventory.key += key;
        }
    }

    public void Restart()
    {
        progresses.Clear();
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
        progress.fillAmount = 0;
        Transform target = EnemyTowerController.instance.scTowers[EnemyTowerController.instance.towers.Length - 1].col.transform;
        float distance = Mathf.Abs(PlayerController.instance.transform.position.x - target.position.x);
        StartCoroutine(LaunchProgress(distance, target));
    }

    IEnumerator LaunchProgress(float distance, Transform target)
    {
        float startPos = target.position.x;
        while (progress.fillAmount <= 1 && GameController.instance.isStart)
        {
            float distanceLaunch = Mathf.Abs(target.position.x - startPos);
            float percentage = distanceLaunch / distance;
            progress.fillAmount = Mathf.Lerp(progress.fillAmount, percentage, 0.1f);
            yield return new WaitForFixedUpdate();
        }
    }

    public void PlusGold(int gold)
    {
        this.gold += gold;
        PlayerController.instance.player.PlusGold(gold);
        textGold.text = this.gold.ToString();
    }
}
