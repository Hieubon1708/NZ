using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHandler : MonoBehaviour
{
    public GameObject parent;
    public Image progress;
    public int gold;
    public TextMeshProUGUI textGold;
    public GameObject[] chests;
    public GameObject[] chestCompleteds;
    public List<int> progresses;
    public List<EquipmentInfo> equipRewards = new List<EquipmentInfo>();

    public void LoadData()
    {
        if (DataManager.instance.dataStorage != null)
        {
            progresses = DataManager.instance.dataStorage.progresses.ToList();
            Restart();
            if (DataManager.instance.dataStorage.level == 0) chests[0].SetActive(false);
        }
    }

    public void ChestReward(int indexTower)
    {
        indexTower += 1;
        if (indexTower % 2 != 0 && indexTower > progresses[progresses.Count - 1])
        {
            chestCompleteds[indexTower % 2].SetActive(true);
            progresses.Add(indexTower);
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
