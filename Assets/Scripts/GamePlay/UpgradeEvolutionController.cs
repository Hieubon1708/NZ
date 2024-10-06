using System.Collections.Generic;
using UnityEngine;

public class UpgradeEvolutionController : MonoBehaviour
{
    public static UpgradeEvolutionController instance;

    public List<SAWEVO> saws;
    public List<int> flames;
    public List<int> machineGuns;

    public enum SAWEVO
    {
        PUSHESENEMIES = 3, INCREASEDAMAGE = 3, ADDFROMBOOSTER = 1, DECREASEENERGY = 1, STUNENEMY = 3
    }

    private void Awake()
    {
        instance = this;
    }


    public void LoadData()
    {
        UpgradeEvolution.instance.UpdateSawEvo();
    }

    public void SawAddEvolution(SAWEVO type)
    {
        saws.Add(type);
        UpgradeEvolution.instance.UpdateSawEvo();
    }

    public int GetAmoutSawEvo(SAWEVO type)
    {
        int count = 0;
        for (int i = 0; i < saws.Count; i++)
        {
            if (saws[i] == type)
            {
                count++;
            }
        }
        return count;
    }

    public bool IsSawMaxType(SAWEVO type)
    {
        int max = (int)type;
        return GetAmoutSawEvo(type) == max;
    }
}
