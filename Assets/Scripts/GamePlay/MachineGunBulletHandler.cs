using DG.Tweening;
using UnityEngine;

public class MachineGunBulletHandler : MonoBehaviour
{
    public int level;
    public Rigidbody2D rb;
    int indexRoadCollider;
    public bool isGunBooster;
    public CircleCollider2D colBooster;
    Tween endBump;

    public void Shot(float speed, Vector2 dir)
    {
        if (isGunBooster) colBooster.enabled = false;
        indexRoadCollider = Random.Range(0, RoadColliderGenerator.instance.count);
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        //Debug.DrawLine(transform.position, raycastDirection * 10, Color.red, 10);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (UpgradeEvolutionController.instance.IsMachineGunContains(UpgradeEvolutionController.MACHINEGUNEVO.PUSHESENEMIES, level))
            {
                int amout = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(UpgradeEvolutionController.MACHINEGUNEVO.PUSHESENEMIES, level);
                int percentage = 0;

                if (amout == 1)
                {
                    percentage = 10;
                }
                else if (amout == 2)
                {
                    percentage = 15;
                }
                else if (amout == 3)
                {
                    percentage = 20;
                }
                int random = Random.Range(0, 100);

                if (random <= percentage)
                {
                    EnemyHandler eSc = EnemyTowerController.instance.GetScE(collision.attachedRigidbody.gameObject);
                    eSc.StartBumpByWeapon();
                    if (endBump != null && endBump.IsActive()) endBump.Kill();
                    endBump = DOVirtual.DelayedCall(0.125f, delegate
                    {
                        eSc.EndBumpByWeapon();
                    });
                }
            }
        }
        if (collision.CompareTag("Road_" + indexRoadCollider))
        {
            if (!isGunBooster) ParController.instance.PlayRoadBulletHoleParticle(transform.position);
            else
            {
                ParController.instance.PlayGunHitOnRoadParticle(transform.position);
                colBooster.enabled = true;
            }
        }

        if (isGunBooster && (collision.CompareTag("Enemy") || collision.CompareTag("Tower")))
        {
            ParController.instance.PlayGunHitOnEnemyParticle(transform.position);
            colBooster.enabled = true;
        }
    }

    private void OnDisable()
    {
        if (colBooster != null) colBooster.enabled = false;
    }
}
