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
    public GameObject parent;
    public Image progress;

    public EquipmentInfo[] equips;
    public TextMeshProUGUI[] textDesigns;
    public TextMeshProUGUI textDush;
    public TextMeshProUGUI textGem;
    public TextMeshProUGUI textKey;
    public TextMeshProUGUI textCogwheel;

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
        if (DataManager.instance.dataStorage != null)
        {
            progresses = DataManager.instance.dataStorage.progresses.ToList();
            Restart();
            if (GameController.instance.level == 0) chests[0].SetActive(false);
        }
    }

    public void ShowReward()
    {
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
        if (dush > 0)
        {
            textDush.text = dush.ToString(); textDush.transform.parent.gameObject.SetActive(true);
            EquipmentController.instance.playerInventory.dush += dush;
        }
        if (gem > 0)
        {
            textGem.text = gem.ToString(); textGem.transform.parent.gameObject.SetActive(true);
            UIHandler.instance.PlusGem(dush);
        }
        if (cogwheel > 0)
        {
            textCogwheel.text = cogwheel.ToString(); textCogwheel.transform.parent.gameObject.SetActive(true);
            EquipmentController.instance.playerInventory.cogwheel += cogwheel;
        }
        if (key > 0)
        {
            textKey.text = key.ToString(); textKey.transform.parent.gameObject.SetActive(true);
            EquipmentController.instance.playerInventory.key += key;
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
    }

    public void ChestReward(int indexTower)
    {
        indexTower += 1;
        if (indexTower % 2 != 0 && indexTower > (progresses.Count != 0 ? progresses[progresses.Count - 1] : 0))
        {
            chestCompleteds[indexTower / 2].SetActive(true);
            progresses.Add(indexTower);
            if(indexTower / 2 == 0 && GameController.instance.level == 0) return;
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
        }
    }

    public void Restart()
    {
        for (int i = 0; i < progresses.Count; i++)
        {
            if (progresses[i] % 2 != 0)
            {
                chests[i].SetActive(false);
            }
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
        textGold.text = this.gold.ToString();
    }
}
