using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutorialOject : MonoBehaviour
{
    public Sprite targetButtonImage;
    public RectTransform targetButton;
    public RectTransform targetHand;
    public GameObject hand;
    public GameObject lig;
    public RectTransform otherTarget;
    public Sprite targetOtherImage;
    Tween delayFadeXUnmask;

    public void EnabledTutorial(bool isEnable, Unmask unmaskButton, Unmask unmaskHand
        , Unmask unmaskOther, GameObject unmaskParent, Image xUnmask, Image spButton, Image spOther)
    {
        hand.SetActive(isEnable);
        if (lig != null)
        {
            xUnmask.gameObject.SetActive(isEnable);
            if (isEnable)
            {
                delayFadeXUnmask = DOVirtual.DelayedCall(1.5f, delegate
                {
                    xUnmask.DOFade(1f, 0.35f);
                });
            }
            else
            {
                delayFadeXUnmask.Kill();
                xUnmask.DOKill();
                xUnmask.DOFade(0f, 0f);
            }
            spButton.sprite = targetButtonImage;
            if (otherTarget != null)
            {
                unmaskOther.gameObject.SetActive(isEnable);
                unmaskOther.fitTarget = otherTarget;
                spOther.sprite = targetOtherImage;
            }
            lig.SetActive(isEnable);
            unmaskButton.fitTarget = targetButton;
            unmaskHand.fitTarget = targetHand;
            unmaskParent.SetActive(isEnable);
        }
    }
}
