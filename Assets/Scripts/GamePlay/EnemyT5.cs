using System.Collections;
using UnityEngine;

// con nhện treo
public class EnemyT5 : EnemyHandler
{
    public float xMin, xMax;
    float targetX;
    float targetY;

    protected override void OnEnable()
    {
        base.OnEnable();
        targetX = Random.Range(xMin,xMax);
        targetY = GameController.instance.cam.ScreenToWorldPoint(new Vector2(0, Random.Range(Screen.height * 0.75f, Screen.height))).y;
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
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(transform.position.x <= targetX)
        {
            isWalk = false;
            if(transform.position.y >= targetY)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetY), 0.1f);
            }
        }
    }

    protected override void DeathHandle()
    {
        base.DeathHandle();
    }
}
