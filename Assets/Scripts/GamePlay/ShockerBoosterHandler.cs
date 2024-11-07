using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShockerBoosterHandler : MonoBehaviour
{
    public SpriteRenderer[] electricities;
    public Rigidbody2D rb;
    public GameObject topBall, botBall;
    public BoxCollider2D col;
    public GameObject parBooster;
    public float targetHeightElectricity;
    public float targetScaleParBooster;
    public float time;
    Coroutine moveBall;
    public TrailRenderer trailRenderer;

    float startSizeElectricity;
    float startSizeParBooster;
    float startSizetopBall;
    float startSizebotBall;
    float startSizecol;
    float startSizeTrail;

    public void ZoomInBooster()
    {
        if (startSizecol == 0)
        {
            startSizecol = col.size.y;
            startSizeElectricity = electricities[0].size.y;
            startSizeParBooster = parBooster.transform.localScale.x;
            startSizetopBall = topBall.transform.position.y;
            startSizebotBall = botBall.transform.position.y;
            startSizeTrail = trailRenderer.widthMultiplier;
        }
        col.size = new Vector2(col.size.x, startSizecol);
        topBall.transform.position = new Vector2(topBall.transform.position.x, startSizetopBall);
        botBall.transform.position = new Vector2(botBall.transform.position.x, startSizebotBall);
        parBooster.transform.localScale = new Vector3(startSizeParBooster, parBooster.transform.localScale.y, parBooster.transform.localScale.z);
        trailRenderer.widthMultiplier = startSizeTrail;

        for (int i = 0; i < electricities.Length; i++)
        {
            electricities[i].size = new Vector2(electricities[i].size.x, startSizeElectricity);
            int index = i;
            DOVirtual.Float(electricities[index].size.y, targetHeightElectricity, time, (y) =>
            {
                electricities[index].size = new Vector2(electricities[index].size.x, y);
            }).SetEase(Ease.Linear);
        }
        parBooster.transform.DOScaleX(targetScaleParBooster, time).SetEase(Ease.Linear);

        DOVirtual.Float(col.size.y, 5.81f * targetHeightElectricity, time, (y) =>
        {
            col.size = new Vector2(col.size.x, y);
        }).SetEase(Ease.Linear);
        DOVirtual.Float(trailRenderer.widthMultiplier, 5.81f * targetHeightElectricity, time, (w) =>
        {
            for (int i = 0; i < trailRenderer.positionCount; i++)
            {
                trailRenderer.SetPosition(i, new Vector2(trailRenderer.transform.position.x - 1.5f, trailRenderer.transform.position.y));
            }
            trailRenderer.widthMultiplier = w;
        }).SetEase(Ease.Linear).OnComplete(delegate
        {
            if (moveBall != null) StopCoroutine(moveBall);
            rb.velocity = new Vector2(rb.velocity.x, 0);
        });

        moveBall = StartCoroutine(MoveBall());
    }

    IEnumerator MoveBall()
    {
        while (true)
        {
            topBall.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + col.size.y / 2);
            botBall.transform.position = new Vector2(col.transform.position.x, col.transform.position.y - col.size.y / 2);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDisable()
    {
        if (moveBall != null)
        {
            StopCoroutine(moveBall);
        }
        for (int i = 0; i < electricities.Length; i++)
        {
            electricities[i].DOKill();
        }
        parBooster.transform.DOKill();
        col.DOKill();
    }
}
