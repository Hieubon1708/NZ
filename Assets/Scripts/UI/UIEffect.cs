using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour
{
    public GameObject preGold;
    public Image lightCircle;
    public GameObject[] golds;
    public RectTransform[] areaIn;
    public RectTransform[] areaOut;
    public GameObject targetGold;

    public void Start()
    {
        golds = new GameObject[7];
        for (int i = 0; i < 7; i++)
        {
            golds[i] = Instantiate(preGold, UIHandler.instance.poolUIs);
            golds[i].SetActive(false);
        }
    }

    public void FlyGold()
    {
        for (int i = 0; i < golds.Length; i++)
        {
            float xIn = Random.Range(areaIn[i].transform.position.x - areaIn[i].sizeDelta.x / 2, areaIn[i].transform.position.x + areaIn[i].sizeDelta.x / 2);
            float yIn = Random.Range(areaIn[i].transform.position.y - areaIn[i].sizeDelta.y / 2, areaIn[i].transform.position.y + areaIn[i].sizeDelta.y / 2);

            golds[i].transform.position = new Vector2(xIn, yIn);
            golds[i].SetActive(true);

            float xOut = Random.Range(areaOut[i].transform.position.x - areaOut[i].sizeDelta.x / 2, areaOut[i].transform.position.x + areaOut[i].sizeDelta.x / 2);
            float yOut = Random.Range(areaOut[i].transform.position.y - areaOut[i].sizeDelta.y / 2, areaOut[i].transform.position.y + areaOut[i].sizeDelta.y / 2);

            int index = i;

            lightCircle.DOFade(1f, 0.25f).OnComplete(delegate
            {
                lightCircle.DOFade(0f, 0.85f);
            });
            golds[index].transform.DOMove(new Vector2(xOut, yOut), 0.25f).OnComplete(delegate
            {
                DOVirtual.DelayedCall(Random.Range(0.25f, 0.35f), delegate
                {
                    golds[index].transform.DOMove(targetGold.transform.position, Random.Range(0.35f, 0.45f)).OnComplete(delegate { golds[index].SetActive(false); });
                });
            });
        }
    }

    public void FadeAll(CanvasGroup canvasGroup, float alpha, float duration)
    {
        canvasGroup.DOFade(alpha, duration);
    }

    public void ScalePopup(Image panel, RectTransform popup, float alpha, float durationAlpha, float scale, float durationScale)
    {
        panel.DOKill();
        popup.DOKill();

        panel.DOFade(alpha, durationAlpha);
        popup.DOScale(scale, durationScale).SetEase(Ease.OutBack);
    }

    public void ScalePopup(RectTransform popup, float scale, float durationScale)
    {
        popup.DOKill();

        popup.DOScale(scale, durationScale).SetEase(Ease.OutBack);
    }

    public void OnDestroy()
    {
        for (int i = 0; i < golds.Length; i++)
        {
            golds[i].transform.DOKill();
        }
        lightCircle.DOKill();
    }
}
