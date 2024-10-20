using DG.Tweening;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        transform.DOMoveX(5,2).OnComplete(delegate
        {
            Debug.LogWarning("ok");
        });
    }
}
