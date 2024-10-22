using System.Collections;
using UnityEngine;

public class DesignContraint : MonoBehaviour
{
    public RectTransform content;
    public GameObject label;
    public float startY;

    public void Start()
    {
        EquipmentController.instance.DesignUpdatePosition();
    }
}
