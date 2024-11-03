using Coffee.UIExtensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TutorialOject : MonoBehaviour
{
    public string targetbuttonImageName;
    public string targetOtherImageName;
    public Sprite targetButtonImage;
    public RectTransform targetButton;
    public RectTransform targetHand;
    public GameObject hand;
    public GameObject lig;
    public RectTransform otherTarget;
    Sprite targetOtherImage;
    Tween delayFadeXUnmask;
    public SpriteAtlas SpriteAtlas;

    private void Awake()
    {
        if (targetbuttonImageName != "") targetButtonImage = SpriteAtlas.GetSprite(targetbuttonImageName);
        if (targetOtherImageName != "") targetOtherImage = SpriteAtlas.GetSprite(targetOtherImageName);
    }

    public void EnabledTutorial(bool isEnable, Unmask unmaskButton, Unmask unmaskHand
        , Unmask unmaskOther, GameObject unmaskParent, Image xUnmask, Image spButton, Image spOther)
    {
        hand.SetActive(isEnable);
        if (lig != null)
        {
            xUnmask.gameObject.SetActive(isEnable);
            if (isEnable)
            {
                delayFadeXUnmask = DOVirtual.DelayedCall(2f, delegate
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
