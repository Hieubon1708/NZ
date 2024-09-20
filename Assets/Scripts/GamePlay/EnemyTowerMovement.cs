using UnityEngine;

public class EnemyTowerMovement : MonoBehaviour
{
    public float multiplier;

    void FixedUpdate()
    {
        if (!GameController.instance.isStart) return;
        transform.Translate(-transform.right * Time.fixedDeltaTime * GameController.instance.backgroundSpeed * multiplier);
    }

}
