using UnityEngine;

// con nhện treo
public class EnemyT5 : EnemyHandler
{
    public float xMin, xMax;
    float targetX;
    float targetY;

    public override void Start()
    {
        base.Start();
        targetX = Random.Range(xMin, xMax);
        targetY = GameController.instance.cam.ScreenToWorldPoint(new Vector2(0, Random.Range(Screen.height * 0.75f, Screen.height))).y;
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
