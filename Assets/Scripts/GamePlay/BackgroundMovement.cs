using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float multiplier;
    public List<GameObject> parts;
    public float distancePart;
    int count = 1;

    void FixedUpdate()
    {
        if(!GameController.instance.isStart) return;
        transform.Translate(-transform.right * Time.fixedDeltaTime * GameController.instance.backgroundSpeed * multiplier);
        if(-transform.localPosition.x >= distancePart * count)
        {
            SwapPart();
            count++;
        }
    }

    void SwapPart()
    {
        parts[0].transform.localPosition = new Vector2(parts[parts.Count - 1].transform.localPosition.x + distancePart, parts[parts.Count - 1].transform.localPosition.y);
        parts.Add(parts[0]);
        parts.RemoveAt(0);
    }

    public void Restart()
    {
        transform.localPosition = new Vector2(0, transform.localPosition.y);
        parts[1].transform.localPosition = new Vector2(-distancePart, parts[1].transform.localPosition.y);
        parts[0].transform.localPosition = new Vector2(0, parts[0].transform.localPosition.y);
        parts[2].transform.localPosition = new Vector2(distancePart, parts[2].transform.localPosition.y);
        count = 1;
    }
}
