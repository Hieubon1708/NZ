using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeEvolutionController : MonoBehaviour
{
    public static UpgradeEvolutionController instance;

    public List<SAWEVO> saws;
    public List<FLAMEEVO> flames;
    public List<MACHINEGUNEVO> machineGuns;

    public enum SAWEVO
    {
        PUSHESENEMIES, INCREASEDAMAGE, ADDFROMBOOSTER, DECREASEENERGY, STUNENEMY
    }
    
    public enum FLAMEEVO
    {
        ATTACKRADIUS, BURNING, INCREASEDAMAGE, ATTACKDURATION, ATTACKCOOLDOWN, DECREASEENERGY
    }

    public enum MACHINEGUNEVO
    {
        ADDBULLET, INCREASEDAMAGE, ATTACKCOOLDOWN, PUSHESENEMIES, DECREASEENERGY, ATTACKDURATION
    }

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            UIUpgradeEvolution.instance.ShowPanelSawEvo();
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            UIUpgradeEvolution.instance.ShowPanelFlameEvo();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            UIUpgradeEvolution.instance.ShowPanelMachineGunEvo();
        }
    }

    public void LoadData()
    {
        saws = DataManager.instance.dataStorage.weaponEvolutionDataStorge != null ? DataManager.instance.dataStorage.weaponEvolutionDataStorge.sawEvos.ToList() : new List<SAWEVO>();
        flames = DataManager.instance.dataStorage.weaponEvolutionDataStorge != null ? DataManager.instance.dataStorage.weaponEvolutionDataStorge.flameEvos.ToList() : new List<FLAMEEVO>();
        machineGuns = DataManager.instance.dataStorage.weaponEvolutionDataStorge != null ? DataManager.instance.dataStorage.weaponEvolutionDataStorge.machineGunEvos.ToList() : new List<MACHINEGUNEVO>();

        UIUpgradeEvolution.instance.UpdateSawEvo();
        UIUpgradeEvolution.instance.UpdateFlameEvo();
        UIUpgradeEvolution.instance.UpdateMachineGunEvo();
    }

    public void SawAddEvolution(int type)
    {
        saws.Add((SAWEVO)type);
        UIUpgradeEvolution.instance.UpdateSawEvo();
        UIUpgradeEvolution.instance.HidePanelSawEvo();
    }
    
    public void FlameAddEvolution(int type)
    {
        flames.Add((FLAMEEVO)type);
        UIUpgradeEvolution.instance.UpdateFlameEvo();
        UIUpgradeEvolution.instance.HidePanelFlameEvo();
    }
    
    public void MachineGunAddEvolution(int type)
    {
        machineGuns.Add((MACHINEGUNEVO)type);
        UIUpgradeEvolution.instance.UpdateMachineGunEvo();
        UIUpgradeEvolution.instance.HidePanelMachineGunEvo();
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

    public int GetAmoutFlameEvo(FLAMEEVO type)
    {
        int count = 0;
        for (int i = 0; i < flames.Count; i++)
        {
            if (flames[i] == type)
            {
                count++;
            }
        }
        return count;
    }

    public int GetAmoutMachineGunEvo(MACHINEGUNEVO type)
    {
        int count = 0;
        for (int i = 0; i < machineGuns.Count; i++)
        {
            if (machineGuns[i] == type)
            {
                count++;
            }
        }
        return count;
    }
}
