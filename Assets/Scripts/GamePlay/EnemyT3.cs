using System.Collections;
using UnityEngine;

// con bắn đạn, bay ở trên
public class EnemyT3 : EnemyHandler
{
    public float xMin, xMax;
    public float yMin, yMax;
    public Vector2 targetPos;
    float yRandomAfterLevingCave;

    protected override void OnEnable()
    {
        base.OnEnable();
        isWalk = true;
        yRandomAfterLevingCave = Random.Range(yMin, yMax);
    }

    protected override IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColDisplay")) content.SetActive(true);
        if (!content.activeSelf) yield break;
        StartCoroutine(base.OnTriggerEnter2D(collision));
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (collision.CompareTag("Tower")) StartCoroutine(LevingCave());
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {

    }

    protected override void FixedUpdate()
    {
        if (transform.position.x < xMax)
        {
            if (isWalk)
            {
                rb.velocity = Vector2.zero;
                isWalk = false;
                targetPos = RandomTarget();
            }
            if (Vector2.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position, targetPos, 0.1f);
                Debug.DrawLine(transform.position, targetPos, Color.red);
            }
            else
            {
                targetPos = RandomTarget();
            }
        }
        base.FixedUpdate();
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
    }

    Vector2 RandomTarget()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        Vector2 pos = (Vector2)transform.position + new Vector2(x, y) * 1.5f;
        float clampX = Mathf.Clamp(pos.x, xMin, xMax);
        float clampY = Mathf.Clamp(pos.y, yMin, yMax);
        return new Vector2(clampX, clampY);
    }

    IEnumerator LevingCave()
    {
        rb.velocity = new Vector2(rb.velocity.x, 1.5f);
        yield return new WaitWhile(() => transform.position.y <= yRandomAfterLevingCave);
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }
}
