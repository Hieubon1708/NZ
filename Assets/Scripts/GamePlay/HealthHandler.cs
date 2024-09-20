using UnityEngine;
using DG.Tweening;

public class HealthHandler : MonoBehaviour
{
    public SpriteRenderer layer_1;
    public SpriteRenderer layer_2;
    float startWidth;
    public float startHp;
    public float timeSubtractHp_1;
    public float timeSubtractHp_2;
    Tween delayCallLayer_1;
    Tween delayCallLayer_2;

    public void Awake()
    {
        startWidth = layer_2.size.x;
    }

    public void SetDefaultInfo(Enemy e)
    {
        KillDelay();
        e.hp = startHp;
        layer_1.size = new Vector2(startWidth, layer_1.size.y);
        layer_2.size = new Vector2(startWidth, layer_2.size.y);
    }

    public void SetTotalHp(float totalHp)
    {
        startHp = totalHp;
    }

    public void SubtractHp(float currentHp)
    {
        KillDelay();
        float percentage = GetPercentageOfTotal(currentHp, startHp);
        float value = GetValueOfPercentage(percentage, startWidth);
        delayCallLayer_2 = DOVirtual.Float(layer_2.size.x, value, timeSubtractHp_2, (x) =>
        {
            layer_2.size = new Vector2(x, layer_2.size.y);
        });
        delayCallLayer_1 = DOVirtual.DelayedCall(0.25f, delegate
        {
            DOVirtual.Float(layer_1.size.x, value, timeSubtractHp_1, (x) =>
            {
                layer_1.size = new Vector2(x, layer_1.size.y);
            });
        });
    }

    float GetPercentageOfTotal(float value, float total)
    {
        return value / total * 100;
    }

    float GetValueOfPercentage(float percentage, float total)
    {
        return percentage * total / 100;
    }

    void KillDelay()
    {
        delayCallLayer_1.Kill();
        delayCallLayer_2.Kill();
    }

    private void OnDestroy()
    {
        KillDelay();
    }
}
