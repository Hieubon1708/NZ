using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour
{
    public GameObject preGold;
    public GameObject preGem;
    public Transform pointSpawnGems;
    public Transform targetGems;
    public Image lightCircle;
    public GameObject[] golds;
    public GameObject[] gems;
    public RectTransform[] areaGems;
    public RectTransform[] areaIn;
    public RectTransform[] areaOut;
    public GameObject targetGold;
    Tween[] delayCalls;
    Tween delayGoldUpdate;
    Tween delayGemUpdate;
    Tween delayChangeDaily;
    float startScaleGem;

    public void Start()
    {
        golds = new GameObject[7];
        delayCalls = new Tween[golds.Length];
        for (int i = 0; i < golds.Length; i++)
        {
            golds[i] = Instantiate(preGold, UIHandler.instance.poolUIs);
            golds[i].SetActive(false);
        }
        gems = new GameObject[3];
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i] = Instantiate(preGem, UIHandler.instance.poolUIs);
            gems[i].SetActive(false);
        }
        startScaleGem = gems[0].transform.localScale.x;
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

            lightCircle.DOFade(1f, 0.35f).OnComplete(delegate
            {
                lightCircle.DOFade(0f, 0.85f);
            });
            golds[index].transform.DOMove(new Vector2(xOut, yOut), 0.35f).OnComplete(delegate
            {
                delayCalls[index] = DOVirtual.DelayedCall(Random.Range(0.35f, 0.45f), delegate
                {
                    golds[index].transform.DOMove(targetGold.transform.position, Random.Range(0.35f, 0.45f)).OnComplete(delegate { golds[index].SetActive(false); });
                });

            });
        }
        delayGoldUpdate = DOVirtual.DelayedCall(1.05f, delegate
        {
            UIHandler.instance.GoldUpdatee();
            BlockController.instance.CheckButtonStateAll();
            UIHandler.instance.tutorial.TutorialButtonBuyBlock(false);
            UIHandler.instance.progressHandler.ShowReward();
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            UIHandler.instance.daily.RewardDaily();
        }
    }

    public void FlyGem()
    {
        UIHandler.instance.daily.HideDaily();
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i].transform.position = pointSpawnGems.position;
            gems[i].SetActive(true);

            float x = Random.Range(areaGems[i].transform.position.x - areaGems[i].sizeDelta.x / 2, areaGems[i].transform.position.x + areaGems[i].sizeDelta.x / 2);
            float y = Random.Range(areaGems[i].transform.position.y - areaGems[i].sizeDelta.y / 2, areaGems[i].transform.position.y + areaGems[i].sizeDelta.y / 2);

            gems[i].transform.DOScale(startScaleGem + (i % 2 == 0 ? 0.15f : 0.5f), 0.45f).SetEase(Ease.OutQuad);
            gems[i].transform.DORotate(new Vector3(0, 0, 360), 0.4f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Incremental);
            int index = i;
            gems[index].transform.DOMove(new Vector2(x, y), 0.45f).OnComplete(delegate
            {
                gems[index].transform.DOScale(startScaleGem, 0.35f).SetEase(Ease.InQuad);
                gems[index].transform.DOMove(targetGems.transform.position, 0.35f).OnComplete(delegate { gems[index].SetActive(false); }).SetEase(Ease.InQuad);
            }).SetEase(Ease.OutQuad);
        }
        delayGemUpdate = DOVirtual.DelayedCall(0.9f, delegate
        {
            UIHandler.instance.GemUpdatee();
            UIHandler.instance.daily.ChangeDaily();
        });
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
        KillTw();
    }

    public void KillTw()
    {
        for (int i = 0; i < golds.Length; i++)
        {
            delayCalls[i].Kill();
            golds[i].transform.DOKill();
            golds[i].SetActive(false);
        }
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i].transform.DOKill();
            gems[i].SetActive(false);
        }
        Color color = lightCircle.color;
        color.a = 0;
        lightCircle.color = color;
        lightCircle.DOKill();
        delayGoldUpdate.Kill();
        delayGemUpdate.Kill();
    }
}
