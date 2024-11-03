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
    public RectTransform rectIn;
    public RectTransform rectOut;
    public RectTransform targetGold;
    public RectTransform targetGoldEnd;
    Tween[] delayCalls;
    Tween delayGoldUpdate;
    Tween delayGemUpdate;
    float startScaleGem;
    int indexGold;

    public void Start()
    {
        golds = new GameObject[84];
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
        FlyGoldHandle(true, 7, GameController.instance.cam.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2)), targetGold.position, 1f, 0.35f, 0.35f, 0.45f, 0.45f, 0.45f);
        delayGoldUpdate = DOVirtual.DelayedCall(1.5f, delegate
        {
            UIHandler.instance.GoldUpdatee();
            if (!GameController.instance.isPLayBoss)
            {
                UIHandler.instance.progressHandler.ShowReward();
                UIHandler.instance.tutorial.TutorialButtonInventory(false);
            }
        });
    }

    void FlyGoldHandle(bool isUseLight, int amountGold, Vector2 pos, Vector2 target, float scale, float timeMoveInToOut, float randomTimeDelayMoveTarget1, float randomTimeDelayMoveTarget2, float randomTimeMoveToTarget1, float randomTimeMoveToTarget2)
    {
        rectIn.position = pos;
        rectOut.position = pos;
        for (int i = 0; i < amountGold; i++)
        {
            int index = indexGold;
            float xIn = Random.Range(areaIn[i].transform.position.x - 0.5f, areaIn[i].transform.position.x + 0.5f);
            float yIn = Random.Range(areaIn[i].transform.position.y - 0.5f, areaIn[i].transform.position.y + 0.5f);

            golds[index].transform.position = new Vector2(xIn, yIn);
            golds[index].transform.localScale = Vector3.one * scale;
            golds[index].SetActive(true);

            float xOut = Random.Range(areaOut[i].transform.position.x - 1, areaOut[i].transform.position.x + 1);
            float yOut = Random.Range(areaOut[i].transform.position.y - 1, areaOut[i].transform.position.y + 1);

            if (isUseLight) lightCircle.DOFade(1f, 0.35f).OnComplete(delegate
             {
                 lightCircle.DOFade(0f, 0.85f);
             });
            golds[index].transform.DOMove(new Vector2(xOut, yOut), timeMoveInToOut).OnComplete(delegate
            {
                delayCalls[index] = DOVirtual.DelayedCall(Random.Range(randomTimeDelayMoveTarget1, randomTimeDelayMoveTarget2), delegate
                {
                    golds[index].transform.DOMove(target, Random.Range(randomTimeMoveToTarget1, randomTimeMoveToTarget2)).OnComplete(delegate { golds[index].SetActive(false); });
                });
            });
            indexGold++;
            if (indexGold == golds.Length) indexGold = 0;
        }
    }

    public void EndFlyGold(Vector2 pos)
    {
        FlyGoldHandle(false, 10, pos, targetGoldEnd.position, 0.65f, 0.35f, 0.05f, 0.25f, 0.25f, 0.35f);
    }

    public void FlyGem()
    {
        UIHandler.instance.daily.HideDaily();
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i].transform.position = pointSpawnGems.position;
            gems[i].SetActive(true);

            float x = Random.Range(areaGems[i].transform.position.x - 0.5f, areaGems[i].transform.position.x + 0.5f);
            float y = Random.Range(areaGems[i].transform.position.y - 0.5f, areaGems[i].transform.position.y + 0.5f);

            gems[i].transform.DOScale(startScaleGem + (i % 2 == 0 ? 0.15f : 0.5f), 0.45f).SetEase(Ease.OutQuad);
            gems[i].transform.DORotate(new Vector3(0, 0, 360), 0.45f, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
            int index = i;
            gems[index].transform.DOMove(new Vector2(x, y), 0.45f).OnComplete(delegate
            {
                gems[index].transform.DOScale(startScaleGem, 0.35f).SetEase(Ease.InQuad);
                gems[index].transform.DORotate(new Vector3(0, 0, 360), 0.35f, RotateMode.FastBeyond360).SetEase(Ease.InQuad);
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
        //KillTw();
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
