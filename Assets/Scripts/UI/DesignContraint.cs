using System.Collections;
using UnityEngine;

public class DesignContraint : MonoBehaviour
{
    public RectTransform content;
    public float startY;

    private void Start()
    {
        StartCoroutine(SetPosition());
    }

    IEnumerator SetPosition()
    {
        yield return new WaitForFixedUpdate();
        float y = content.transform.position.y - content.sizeDelta.y - 180;
        transform.position = new Vector2(Screen.width / 2, y > startY ? startY : y);
    }
}
