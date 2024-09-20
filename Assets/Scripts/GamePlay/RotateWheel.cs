using UnityEngine;

public class RotateWheel : MonoBehaviour
{
    public GameObject[] wheels;
    public float multiplier;

    void FixedUpdate()
    {
        if (!GameController.instance.isStart) return;
        wheels[0].transform.Rotate(new Vector3(0, 0, -1), Time.deltaTime * GameController.instance.backgroundSpeed * multiplier);
        wheels[1].transform.Rotate(new Vector3(0, 0, -1), Time.deltaTime * GameController.instance.backgroundSpeed * multiplier);
    }
}
