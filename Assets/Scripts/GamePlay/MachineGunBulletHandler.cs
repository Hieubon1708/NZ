using DG.Tweening;
using UnityEngine;

public class MachineGunBulletHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    int indexRoadCollider;
    public bool isGunBooster;
    public CircleCollider2D colBooster;
    Tween endBump;

    public void Shot(float speed, Vector2 dir)
    {
        indexRoadCollider = Random.Range(0, RoadColliderGenerator.instance.count);
        rb.AddForce(dir * speed, ForceMode2D.Impulse);
        //Debug.DrawLine(transform.position, raycastDirection * 10, Color.red, 10);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (UpgradeEvolutionController.instance.machineGuns.Contains(UpgradeEvolutionController.MACHINEGUNEVO.PUSHESENEMIES))
            {
                int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(UpgradeEvolutionController.MACHINEGUNEVO.PUSHESENEMIES);
                int percentage = 0;

                if (level == 1)
                {
                    percentage = 10;
                }
                else if (level == 2)
                {
                    percentage = 15;
                }
                else if (level == 3)
                {
                    percentage = 20;
                }
                int random = Random.Range(0, 100);

                if (random <= percentage)
                {
                    EnemyHandler eSc = EnemyTowerController.instance.GetScEInTower(collision.attachedRigidbody.gameObject);
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
            gameObject.SetActive(false);
            if (!isGunBooster) ParController.instance.PlayRoadBulletHoleParticle(transform.position);
            else ParController.instance.PlayGunHitOnRoadParticle(transform.position);
        }

        if (colBooster != null && (collision.CompareTag("Enemy") || collision.CompareTag("Tower"))) colBooster.enabled = true;
    }

    private void OnDisable()
    {
        if (colBooster != null) colBooster.enabled = false;
    }
}
