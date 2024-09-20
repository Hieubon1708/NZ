using System.Collections;
using UnityEngine;

public class RoadColliderGenerator : MonoBehaviour
{
    public static RoadColliderGenerator instance;

    public GameObject preRoadCollider;
    public Transform container;
    public GameObject[] listColliders;
    public int count;

    private void Awake()
    {
        instance = this;
        GenerateCollider();
    }

    public void Start()
    {

    }

    void GenerateCollider()
    {
        listColliders = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            GameObject roadColIns = Instantiate(preRoadCollider, container);
            roadColIns.tag = "Road_" + i;
            listColliders[i] = roadColIns;
            float x = i == 0 ? 0 : roadColIns.transform.localPosition.x; ;
            roadColIns.transform.localPosition = new Vector2(x, i * 0.05f);
        }
    }
}
