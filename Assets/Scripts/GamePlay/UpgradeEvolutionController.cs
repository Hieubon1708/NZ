using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeEvolutionController : MonoBehaviour
{
    public static UpgradeEvolutionController instance;

    public UIUpgradeEvolution uIUpgradeEvolution;

    public List<SAWEVO> saws;
    public List<FLAMEEVO> flames;
    public List<MACHINEGUNEVO> machineGuns;
    public List<SHOCKEREVO> shockers;

    public bool isShowEvoSaw;
    public bool isShowEvoShocker;
    public bool isShowEvoFlame;
    public bool isShowEvoMachineGun;

    public WeaponUpgradeHandler weaponUpgradeHandler;

    public enum SAWEVO
    {
        PUSHESENEMIES, INCREASEDAMAGE, ADDFROMBOOSTER, DECREASEENERGY, STUNENEMY
    }

    public enum SHOCKEREVO
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

    public void SetData(WeaponEvolutionDataStorge weaponEvolutionDataStorge)
    {
        saws = weaponEvolutionDataStorge.sawEvos != null ? weaponEvolutionDataStorge.sawEvos.ToList() : new List<SAWEVO>();
        shockers = weaponEvolutionDataStorge.shockerEvos != null ? weaponEvolutionDataStorge.shockerEvos.ToList() : new List<SHOCKEREVO>();
        flames = weaponEvolutionDataStorge.flameEvos != null ? weaponEvolutionDataStorge.flameEvos.ToList() : new List<FLAMEEVO>();
        machineGuns = weaponEvolutionDataStorge.machineGunEvos != null ? weaponEvolutionDataStorge.machineGunEvos.ToList() : new List<MACHINEGUNEVO>();
    }

    public WeaponEvolutionDataStorge GetData()
    {
        return new WeaponEvolutionDataStorge(saws.ToArray(), flames.ToArray(), machineGuns.ToArray(), shockers.ToArray());
    }

    private void OnDestroy()
    {
        if (isShowEvoSaw) GetSAWEVOS();
        if (isShowEvoMachineGun) GetMACHINEGUNEVOS();
        if (isShowEvoFlame) GetFLAMEVOS();
        if (isShowEvoShocker) GetSHOCKEREVOS();

        DataManager.instance.SaveEvo();
    }

    public SAWEVO[] GetSAWEVOS()
    {
        if (isShowEvoSaw == true)
        {
            int random = Random.Range(0, 2);
            if (random == 0) saws.Add(uIUpgradeEvolution.sawRamdom1);
            else saws.Add(uIUpgradeEvolution.sawRamdom2);
        }
        return saws.ToArray();
    }
    public FLAMEEVO[] GetFLAMEVOS()
    {
        if (isShowEvoFlame == true)
        {
            int random = Random.Range(0, 2);
            if (random == 0) flames.Add(uIUpgradeEvolution.flameRandom1);
            else flames.Add(uIUpgradeEvolution.flameRandom2);
        }
        return flames.ToArray();
    }

    public MACHINEGUNEVO[] GetMACHINEGUNEVOS()
    {
        if (isShowEvoMachineGun == true)
        {
            int random = Random.Range(0, 2);
            if (random == 0) machineGuns.Add(uIUpgradeEvolution.machineGunRandom1);
            else machineGuns.Add(uIUpgradeEvolution.machineGunRandom2);
        }
        return machineGuns.ToArray();
    }
    public SHOCKEREVO[] GetSHOCKEREVOS()
    {
        if (isShowEvoShocker == true)
        {
            int random = Random.Range(0, 2);
            if (random == 0) shockers.Add(uIUpgradeEvolution.shockerRandom1);
            else shockers.Add(uIUpgradeEvolution.shockerRandom2);
        }
        return shockers.ToArray();
    }

    public void LoadData()
    {
        saws = DataManager.instance.weaponEvolutionDataStorge != null ? DataManager.instance.weaponEvolutionDataStorge.sawEvos.ToList() : new List<SAWEVO>();
        flames = DataManager.instance.weaponEvolutionDataStorge != null ? DataManager.instance.weaponEvolutionDataStorge.flameEvos.ToList() : new List<FLAMEEVO>();
        machineGuns = DataManager.instance.weaponEvolutionDataStorge != null ? DataManager.instance.weaponEvolutionDataStorge.machineGunEvos.ToList() : new List<MACHINEGUNEVO>();
        shockers = DataManager.instance.weaponEvolutionDataStorge != null ? DataManager.instance.weaponEvolutionDataStorge.shockerEvos.ToList() : new List<SHOCKEREVO>();

        uIUpgradeEvolution.UpdateShockerEvo();
        uIUpgradeEvolution.UpdateSawEvo();
        uIUpgradeEvolution.UpdateFlameEvo();
        uIUpgradeEvolution.UpdateMachineGunEvo();
    }

    public bool IsSawContains(SAWEVO type, int level)
    {
        for (int i = 0; i < saws.Count; i++)
        {
            if (saws[i] == type && i < level)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsShockerContains(SHOCKEREVO type, int level)
    {
        for (int i = 0; i < shockers.Count; i++)
        {
            if (shockers[i] == type && i < level)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFlameContains(FLAMEEVO type, int level)
    {
        for (int i = 0; i < flames.Count; i++)
        {
            if (flames[i] == type && i < level)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsMachineGunContains(MACHINEGUNEVO type, int level)
    {
        for (int i = 0; i < machineGuns.Count; i++)
        {
            if (machineGuns[i] == type && i < level)
            {
                return true;
            }
        }
        return false;
    }

    public void SawAddEvolution(int type)
    {
        isShowEvoSaw = false;
        saws.Add((SAWEVO)type);
        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is SawHandler)
            {
                SawHandler sawHandler = (SawHandler)sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter;
                sawHandler.LoadData();
            }
        }
        uIUpgradeEvolution.UpdateSawEvo();
        uIUpgradeEvolution.HidePanelSawEvo();
        SetActiveButtonUpgradeWeapon();
    }

    void SetActiveButtonUpgradeWeapon()
    {
        AudioController.instance.PlaySound(AudioController.instance.buttonClick);
        weaponUpgradeHandler.EvoAccept();
        weaponUpgradeHandler.gameObject.SetActive(true);
        weaponUpgradeHandler.aniShowNewEvo.Play();
        DataManager.instance.SaveEvo();
    }

    public void ShockerAddEvolution(int type)
    {
        isShowEvoShocker = false;
        shockers.Add((SHOCKEREVO)type);
        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is ShockerHandler)
            {
                ShockerHandler shockerHandler = (ShockerHandler)sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter;
                shockerHandler.LoadData();
            }
        }
        uIUpgradeEvolution.UpdateShockerEvo();
        uIUpgradeEvolution.HidePanelShockerEvo();
        SetActiveButtonUpgradeWeapon();
    }

    public void FlameAddEvolution(int type)
    {
        isShowEvoFlame = false;
        flames.Add((FLAMEEVO)type);
        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is FlameHandler)
            {
                FlameHandler flameHandler = (FlameHandler)sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter;
                if (type == 0)
                {
                    flameHandler.AttackRadiusChange();
                }
                else if (type == 3)
                {
                    flameHandler.AttackDurationChange();
                }
                else if (type == 4)
                {
                    flameHandler.AttackCooldownChange();
                }
                else if (type == 5)
                {
                    Booster.instance.DecreaseEnergyFlame(flameHandler.level);
                }
            }
        }
        uIUpgradeEvolution.UpdateFlameEvo();
        uIUpgradeEvolution.HidePanelFlameEvo();
        SetActiveButtonUpgradeWeapon();
    }

    public void MachineGunAddEvolution(int type)
    {
        isShowEvoMachineGun = false;
        machineGuns.Add((MACHINEGUNEVO)type);
        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is MachineGunHandler)
            {
                MachineGunHandler machineGunHandler = (MachineGunHandler)sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter;
                if (type == 0)
                {
                    machineGunHandler.AddBullet();
                }
                else if (type == 2)
                {
                    machineGunHandler.AttackCooldownChange();
                }
                else if (type == 3)
                {
                    Booster.instance.DecreaseEnergyMachineGun(machineGunHandler.level);
                }
                else if (type == 5)
                {
                    machineGunHandler.AttackCooldownChange();
                }
            }
        }
        uIUpgradeEvolution.UpdateMachineGunEvo();
        uIUpgradeEvolution.HidePanelMachineGunEvo();
        SetActiveButtonUpgradeWeapon();
    }

    public int GetAmoutSawEvo(SAWEVO type, int level)
    {
        int count = 0;
        for (int i = 0; i < saws.Count; i++)
        {
            if (saws[i] == type && i < level)
            {
                count++;
            }
        }
        return count;
    }

    public int GetAmoutShockerEvo(SHOCKEREVO type, int level)
    {
        int count = 0;
        for (int i = 0; i < shockers.Count; i++)
        {
            if (shockers[i] == type && i < level)
            {
                count++;
            }
        }
        return count;
    }

    public int GetAmoutFlameEvo(FLAMEEVO type, int level)
    {
        int count = 0;
        for (int i = 0; i < flames.Count; i++)
        {
            if (flames[i] == type && i < level)
            {
                count++;
            }
        }
        return count;
    }

    public int GetAmoutMachineGunEvo(MACHINEGUNEVO type, int level)
    {
        int count = 0;
        for (int i = 0; i < machineGuns.Count; i++)
        {
            if (machineGuns[i] == type && i < level)
            {
                count++;
            }
        }
        return count;
    }
}
