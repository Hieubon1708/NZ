using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHandler : MonoBehaviour
{
    public GameObject parent;
    public Image progress;
    public TextMeshProUGUI gold;

    public void StartGame()
    {
        gold.text = "0";
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
}
