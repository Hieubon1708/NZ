using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public GameObject mapCompleted;
    public Image line;
    public GameObject tick;

    public void DoCompleted(GameObject mapTarget, Action callback)
    {
        tick.SetActive(true);
        line.DOFillAmount(1f, 2f).OnComplete(delegate
        {
            mapTarget.gameObject.SetActive(true);
            if(callback != null) callback.Invoke();
        });
    }
}
