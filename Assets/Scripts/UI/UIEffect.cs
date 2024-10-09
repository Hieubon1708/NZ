using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIEffect : MonoBehaviour
{
   public static UIEffect instance;

    public void Awake()
    {
        instance = this;
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
    
    public void ScalePopup(RectTransform popup,float scale, float durationScale)
    {
        popup.DOKill();

        popup.DOScale(scale, durationScale).SetEase(Ease.OutBack);
    }
}
