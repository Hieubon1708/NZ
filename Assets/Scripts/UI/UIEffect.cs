using DG.Tweening;
using UnityEngine;

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
}
