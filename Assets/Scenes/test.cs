using DG.Tweening;
using TMPro;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject a;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning("En");

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Debug.LogWarning("Ex");
    }
}
