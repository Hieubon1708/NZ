using UnityEngine;

public class PositionConstraintWorldSpace : MonoBehaviour
{
    public Transform target;
    public float x;
    public float y;

    private void OnEnable()
    {
        transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(target.position.x + x, target.position.y + y));
    }

    void FixedUpdate()
    {
        transform.position = GameController.instance.cam.WorldToScreenPoint(new Vector2(target.position.x + x, target.position.y + y));
    }
}
