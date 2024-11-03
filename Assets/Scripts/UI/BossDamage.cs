using DG.Tweening;
using TMPro;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public RectTransform[] rectDamage;
    public TextMeshProUGUI[] textDamage;
    public Rigidbody2D[] rb;
    bool isShowDamage;
    public Color colorCritDamage;
    int currentCount;
    public int damageTaken;

    public void ShowDamage(int damage, BoxCollider2D col, bool isCritDamage)
    {
        damageTaken += damage;
        if (isShowDamage) return;
        float x = Random.Range(col.transform.position.x - (col.size.x / 2) + col.offset.x, col.transform.position.x + (col.size.x / 2) + col.offset.x);
        float y = Random.Range(col.transform.position.y + col.offset.y - 0.5f, col.transform.position.y + col.offset.y + 1.5f);
        Vector2 pos = new Vector2(x, y);
        UIHandler.instance.FlyGold(pos, damageTaken / 100);
        damageTaken = damageTaken % 100;
        int index = currentCount;
        if (isCritDamage)
        {
            textDamage[index].transform.localScale = Vector3.one * 1.1f;
            textDamage[index].color = colorCritDamage;
        }
        else
        {
            textDamage[index].transform.localScale = Vector3.one;
            textDamage[index].color = Color.white;
        }
        textDamage[index].text = UIHandler.instance.ConvertNumberAbbreviation(damage);
        isShowDamage = true;
        rb[index].isKinematic = false;
        rectDamage[index].transform.position = pos;
        textDamage[index].DOFade(1, 0);

        rb[index].AddForce(new Vector2(Random.Range(-0.55f, 0.55f), 4.25f), ForceMode2D.Impulse);
        textDamage[index].DOFade(0, 0.25f).SetDelay(0.25f).OnComplete(delegate
        {
            rb[index].isKinematic = true;
            rb[index].velocity = Vector2.zero;
        });
        DOVirtual.DelayedCall(0.025f, delegate
        {
            currentCount++;
            if(currentCount == textDamage.Length) currentCount = 0;
            isShowDamage = false;
        });
    }

    private void OnDestroy()
    {
        for (int i = 0; i < textDamage.Length; i++)
        {
            textDamage[i].DOKill();
        }
    }
}
