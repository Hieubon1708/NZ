using System.Collections;
using UnityEngine;

public class DesignContraint : MonoBehaviour
{
    public RectTransform content;
    public float startY;

    private void Start()
    {
        startY = transform.position.y;
        StartCoroutine(SetPosition());
    }

    IEnumerator SetPosition()
    {
        yield return new WaitForFixedUpdate();
        transform.position = new Vector2(Screen.width / 2, content.transform.position.y - content.sizeDelta.y - 180);
    }
}
