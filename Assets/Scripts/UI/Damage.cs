using DG.Tweening;
using TMPro;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Transform parent;
    public RectTransform canvas;
    public RectTransform rectDamage;
    public TextMeshProUGUI textDamage;
    public Rigidbody2D rb;
    bool isShowDamage;

    public void ShowDamage(string text)
    {
        if (isShowDamage) return;
        canvas.SetParent(GameController.instance.poolDamages);
        textDamage.text = text;
        isShowDamage = true;
        rb.isKinematic = false;
        textDamage.DOFade(1, 0);
        rb.AddForce(new Vector2(Random.Range(-0.55f, 0.55f), 4.25f), ForceMode2D.Impulse);
        textDamage.DOFade(0, 0.25f).SetDelay(0.25f).OnComplete(delegate
        {
            canvas.SetParent(parent);
            isShowDamage = false;
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            canvas.anchoredPosition = Vector2.zero;
            rectDamage.anchoredPosition = Vector2.zero;
        });
    }

    private void OnDestroy()
    {
        textDamage.DOKill();
    }
}
