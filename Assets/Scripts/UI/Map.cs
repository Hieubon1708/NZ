using DG.Tweening;
using UnityEngine;

public class Map : MonoBehaviour
{
    public MapObject[] mapObjects;
    public GameObject map;

    public void LoadData()
    {
        int level = 0;
        if (DataManager.instance.dataStorage != null) level = DataManager.instance.dataStorage.level;
        for (int i = 0; i <= level; i++)
        {
            if (i < level)
            {
                mapObjects[i].tick.SetActive(true);
                mapObjects[i].line.fillAmount = 1;
            }
            mapObjects[i].mapCompleted.SetActive(true);
        }
    }

    public void ShowMap()
    {
        map.SetActive(true);
        UIHandler.instance.DoLayerCover(0f, 0.75f, delegate
        {
            DOVirtual.DelayedCall(0.25f, delegate
            {
                mapObjects[GameController.instance.level - 1].DoCompleted(mapObjects[GameController.instance.level].mapCompleted, delegate
                {
                    GameController.instance.MapGenerate(GameController.instance.level + 1);
                    DOVirtual.DelayedCall(1f, delegate
                    {
                        UIHandler.instance.DoLayerCover(1f, 0.75f, delegate
                        {
                            map.SetActive(false);
                            UIHandler.instance.DoLayerCover(0f, 0.75f, null);
                        });
                    });
                });
            });
        });
    }
}

