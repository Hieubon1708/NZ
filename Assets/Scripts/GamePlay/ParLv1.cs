using DG.Tweening;
using UnityEngine;

public class ParLv1 : MonoBehaviour
{
    public static ParLv1 instance;

    public void Awake()
    {
        instance = this;
    }
}
