using DG.Tweening;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingOrder = 999; 
        renderer.sortingLayerName = "UI"; 
    }
}
