using System.Collections.Generic;
using UnityEngine;
using static UpgradeEvolutionController;

public class UIUpgradeEvolution : MonoBehaviour
{
    public static UIUpgradeEvolution instance;

    public CanvasGroup sawCanvasGroup;
    public CanvasGroup flameCanvasGroup;
    public CanvasGroup machineGunCanvasGroup;

    public List<SAWEVO> sawEvoTypes = new List<SAWEVO>();
    public List<FLAMEEVO> flameEvoTypes = new List<FLAMEEVO>();
    public List<MACHINEGUNEVO> machineGunEvoTypes = new List<MACHINEGUNEVO>();

    public GameObject panelSawEvo;
    public GameObject panelFlameEvo;
    public GameObject panelMachineGunEvo;

    public SawEvo[] sawEvos;
    public SawSlotEvo[] sawSlotEvos;
    public FlameEvo[] flameEvos;
    public FlameSlotEvo[] flameSlotEvos;
    public MachineGunEvo[] machineGunEvos;
    public MachineGunSlotEvo[] machineGunSlotEvos;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSawEvo()
    {
        if (IsSawMaxType(SAWEVO.STUNENEMY)) sawEvoTypes.Remove(SAWEVO.STUNENEMY);
        if (IsSawMaxType(SAWEVO.PUSHESENEMIES)) sawEvoTypes.Remove(SAWEVO.PUSHESENEMIES);
        if (IsSawMaxType(SAWEVO.ADDFROMBOOSTER)) sawEvoTypes.Remove(SAWEVO.ADDFROMBOOSTER);
        if (IsSawMaxType(SAWEVO.DECREASEENERGY)) sawEvoTypes.Remove(SAWEVO.DECREASEENERGY);
        if (IsSawMaxType(SAWEVO.INCREASEDAMAGE)) sawEvoTypes.Remove(SAWEVO.INCREASEDAMAGE);

        for (int i = 0; i < sawEvoTypes.Count; i++)
        {
            for (int j = 0; j < sawEvos.Length; j++)
            {
                string text = "";
                if (sawEvoTypes[i] == sawEvos[j].type)
                {
                    if (sawEvos[j].type == SAWEVO.STUNENEMY)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutSawEvo(SAWEVO.STUNENEMY) + 1;
                        int percentage = 0; float time = 0;
                        if (level == 1)
                        {
                            percentage = 20;
                            time = 1.5f;
                        }
                        else if (level == 2)
                        {
                            percentage = 25;
                            time = 2f;
                        }
                        else if (level == 3)
                        {
                            percentage = 30;
                            time = 2.5f;
                        }
                        text = "Has " + percentage + "% chance to stund enemy for " + time.ToString("#0.##", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " seconds";
                    }
                    else if (sawEvos[j].type == SAWEVO.ADDFROMBOOSTER)
                    {
                        text = "+1 saw from ability";
                    }
                    else if (sawEvos[j].type == SAWEVO.PUSHESENEMIES)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutSawEvo(SAWEVO.PUSHESENEMIES) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 5;
                        else if (level == 2) percentage = 10;
                        else if (level == 3) percentage = 15;
                        text = "Pushes enemies with a " + percentage + "% chance";
                    }
                    else if (sawEvos[j].type == SAWEVO.DECREASEENERGY)
                    {
                        text = "-25% to ability cost";
                    }
                    else if (sawEvos[j].type == SAWEVO.INCREASEDAMAGE)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutSawEvo(SAWEVO.INCREASEDAMAGE) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 20;
                        else if (level == 2) percentage = 40;
                        else if (level == 3) percentage = 60;
                        text = "+" + percentage + "% damage";
                    }
                    sawEvos[j].content.text = text;
                    break;
                }
            }
        }
    }

    public void UpdateFlameEvo()
    {
        if (IsFlameMaxType(FLAMEEVO.ATTACKCOOLDOWN)) flameEvoTypes.Remove(FLAMEEVO.ATTACKCOOLDOWN);
        if (IsFlameMaxType(FLAMEEVO.ATTACKRADIUS)) flameEvoTypes.Remove(FLAMEEVO.ATTACKRADIUS);
        if (IsFlameMaxType(FLAMEEVO.DECREASEENERGY)) flameEvoTypes.Remove(FLAMEEVO.DECREASEENERGY);
        if (IsFlameMaxType(FLAMEEVO.BURNING)) flameEvoTypes.Remove(FLAMEEVO.BURNING);
        if (IsFlameMaxType(FLAMEEVO.INCREASEDAMAGE)) flameEvoTypes.Remove(FLAMEEVO.INCREASEDAMAGE);
        if (IsFlameMaxType(FLAMEEVO.ATTACKDURATION)) flameEvoTypes.Remove(FLAMEEVO.ATTACKDURATION);

        for (int i = 0; i < flameEvoTypes.Count; i++)
        {
            for (int j = 0; j < flameEvos.Length; j++)
            {
                string text = "";
                if (flameEvoTypes[i] == flameEvos[j].type)
                {
                    if (flameEvos[j].type == FLAMEEVO.ATTACKCOOLDOWN)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKCOOLDOWN) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 30;
                        else if (level == 2) percentage = 50;
                        else if (level == 3) percentage = 70;

                        text = "-" + percentage + "% attack cooldown";
                    }
                    else if (flameEvos[j].type == FLAMEEVO.ATTACKRADIUS)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKRADIUS) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 30;
                        else if (level == 2) percentage = 60;
                        else if (level == 3) percentage = 90;

                        text = "+" + percentage + "% attack radius";
                    }
                    else if (flameEvos[j].type == FLAMEEVO.ATTACKDURATION)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.ATTACKDURATION) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 25;
                        else if (level == 2) percentage = 60;
                        else if (level == 3) percentage = 90;

                        text = "+" + percentage + "% attack duration";
                    }
                    else if (flameEvos[j].type == FLAMEEVO.DECREASEENERGY)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.DECREASEENERGY) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 15;
                        else if (level == 2) percentage = 30;

                        text = "-" + percentage + "% to ability cost";
                    }
                    else if (flameEvos[j].type == FLAMEEVO.INCREASEDAMAGE)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.INCREASEDAMAGE) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 20;
                        else if (level == 2) percentage = 40;
                        else if (level == 3) percentage = 60;
                        text = "+" + percentage + "% damage";
                    }
                    else if (flameEvos[j].type == FLAMEEVO.BURNING)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutFlameEvo(FLAMEEVO.BURNING) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 75;
                        else if (level == 2) percentage = 150;
                        else if (level == 3) percentage = 225;
                        text = "Applies a buring effect to enemies, dealing " + percentage + "% DMG of hero ATK";
                    }
                    flameEvos[j].content.text = text;
                    break;
                }
            }
        }
    }

    public void UpdateMachineGunEvo()
    {
        if (IsMachineGunMaxType(MACHINEGUNEVO.ADDBULLET)) machineGunEvoTypes.Remove(MACHINEGUNEVO.ADDBULLET);
        if (IsMachineGunMaxType(MACHINEGUNEVO.INCREASEDAMAGE)) machineGunEvoTypes.Remove(MACHINEGUNEVO.INCREASEDAMAGE);
        if (IsMachineGunMaxType(MACHINEGUNEVO.ATTACKCOOLDOWN)) machineGunEvoTypes.Remove(MACHINEGUNEVO.ATTACKCOOLDOWN);
        if (IsMachineGunMaxType(MACHINEGUNEVO.DECREASEENERGY)) machineGunEvoTypes.Remove(MACHINEGUNEVO.DECREASEENERGY);
        if (IsMachineGunMaxType(MACHINEGUNEVO.PUSHESENEMIES)) machineGunEvoTypes.Remove(MACHINEGUNEVO.PUSHESENEMIES);
        if (IsMachineGunMaxType(MACHINEGUNEVO.ATTACKDURATION)) machineGunEvoTypes.Remove(MACHINEGUNEVO.ATTACKDURATION);

        for (int i = 0; i < machineGunEvoTypes.Count; i++)
        {
            for (int j = 0; j < machineGunEvos.Length; j++)
            {
                string text = "";
                if (machineGunEvoTypes[i] == machineGunEvos[j].type)
                {
                    if (machineGunEvos[j].type == MACHINEGUNEVO.ADDBULLET)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.ADDBULLET) + 1;
                        int amout = 0;
                        int percentage = 0;

                        if (level == 1)
                        {
                            amout = 1;
                            percentage = 40;
                        }
                        else if (level == 2)
                        {
                            amout = 2;
                            percentage = 50;
                        }

                        text = "Fires an " + amout + " extra bullet, but deals " + percentage + "% less damage per hit";
                    }
                    else if (machineGunEvos[j].type == MACHINEGUNEVO.ATTACKCOOLDOWN)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.ATTACKCOOLDOWN) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 30;
                        else if (level == 2) percentage = 50;
                        else if (level == 3) percentage = 70;

                        text = "-" + percentage + "% attack cooldown";
                    }
                    else if (machineGunEvos[j].type == MACHINEGUNEVO.ATTACKDURATION)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.ATTACKDURATION) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 25;
                        else if (level == 2) percentage = 50;
                        else if (level == 3) percentage = 75;

                        text = "+" + percentage + "% attack duration";
                    }
                    else if (machineGunEvos[j].type == MACHINEGUNEVO.DECREASEENERGY)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.DECREASEENERGY) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 10;
                        else if (level == 2) percentage = 20;

                        text = "-" + percentage + "% to ability cost";
                    }
                    else if (machineGunEvos[j].type == MACHINEGUNEVO.INCREASEDAMAGE)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.INCREASEDAMAGE) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 20;
                        else if (level == 2) percentage = 40;
                        else if (level == 3) percentage = 60;
                        text = "+" + percentage + "% damage";
                    }
                    else if (machineGunEvos[j].type == MACHINEGUNEVO.PUSHESENEMIES)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(MACHINEGUNEVO.PUSHESENEMIES) + 1;
                        int percentage = 0;

                        if (level == 1) percentage = 10;
                        else if (level == 2) percentage = 15;
                        else if (level == 3) percentage = 20;
                        text = "Pushes enemies with " + percentage + "% chance";
                    }
                    machineGunEvos[j].content.text = text;
                    break;
                }
            }
        }
    }

    SawEvo GetSawEvo(SAWEVO type)
    {
        for (int i = 0; i < sawEvos.Length; i++)
        {
            if (sawEvos[i].type == type)
            {
                return sawEvos[i];
            }
        }
        return null;
    }

    FlameEvo GetFlameEvo(FLAMEEVO type)
    {
        for (int i = 0; i < flameEvos.Length; i++)
        {
            if (flameEvos[i].type == type)
            {
                return flameEvos[i];
            }
        }
        return null;
    }

    MachineGunEvo GetMachineGunEvo(MACHINEGUNEVO type)
    {
        for (int i = 0; i < machineGunEvos.Length; i++)
        {
            if (machineGunEvos[i].type == type)
            {
                return machineGunEvos[i];
            }
        }
        return null;
    }

    public bool IsSawMaxType(SAWEVO type)
    {
        SawEvo sawEvo = GetSawEvo(type);
        return UpgradeEvolutionController.instance.GetAmoutSawEvo(type) == sawEvo.maxLevel;
    }

    public bool IsFlameMaxType(FLAMEEVO type)
    {
        FlameEvo flameEvo = GetFlameEvo(type);
        return UpgradeEvolutionController.instance.GetAmoutFlameEvo(type) == flameEvo.maxLevel;
    }

    public bool IsMachineGunMaxType(MACHINEGUNEVO type)
    {
        MachineGunEvo machineGunEvo = GetMachineGunEvo(type);
        return UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(type) == machineGunEvo.maxLevel;
    }

    SawSlotEvo GetSlotSawEvo(SAWEVO type)
    {
        for (int i = 0; i < sawSlotEvos.Length; i++)
        {
            if (sawSlotEvos[i].type == type)
            {
                return sawSlotEvos[i];
            }
        }
        return null;
    }

    FlameSlotEvo GetSlotFlameEvo(FLAMEEVO type)
    {
        for (int i = 0; i < flameSlotEvos.Length; i++)
        {
            if (flameSlotEvos[i].type == type)
            {
                return flameSlotEvos[i];
            }
        }
        return null;
    }

    MachineGunSlotEvo GetSlotMachineGunEvo(MACHINEGUNEVO type)
    {
        for (int i = 0; i < machineGunSlotEvos.Length; i++)
        {
            if (machineGunSlotEvos[i].type == type)
            {
                return machineGunSlotEvos[i];
            }
        }
        return null;
    }

    bool IsActiveSawSlot(SAWEVO type)
    {
        for (int i = 0; i < sawSlotEvos.Length; i++)
        {
            if (sawSlotEvos[i].type == type && sawSlotEvos[i].gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    bool IsActiveFlameSlot(FLAMEEVO type)
    {
        for (int i = 0; i < flameSlotEvos.Length; i++)
        {
            if (flameSlotEvos[i].type == type && flameSlotEvos[i].gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    bool IsActiveMachineGunSlot(MACHINEGUNEVO type)
    {
        for (int i = 0; i < machineGunSlotEvos.Length; i++)
        {
            if (machineGunSlotEvos[i].type == type && machineGunSlotEvos[i].gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowPanelSawEvo()
    {
        UpdateSawEvo();

        for (int i = 0; i < UpgradeEvolutionController.instance.saws.Count; i++)
        {
            SawSlotEvo sawSlotEvo = GetSlotSawEvo(UpgradeEvolutionController.instance.saws[i]);
            if (!IsActiveSawSlot(UpgradeEvolutionController.instance.saws[i]))
            {
                sawSlotEvo.gameObject.SetActive(true);
                sawSlotEvo.transform.SetAsLastSibling();
            }
            sawSlotEvo.SetAmout(UpgradeEvolutionController.instance.GetAmoutSawEvo(UpgradeEvolutionController.instance.saws[i]));
        }

        List<SAWEVO> temp = new List<SAWEVO>(sawEvoTypes);

        SAWEVO type1 = temp[Random.Range(0, temp.Count)];
        temp.Remove(type1);
        SAWEVO type2 = temp[Random.Range(0, temp.Count)];

        for (int i = 0; i < sawEvos.Length; i++)
        {
            if (sawEvos[i].type == type1 || sawEvos[i].type == type2)
            {
                sawEvos[i].gameObject.SetActive(true);
            }
            else
            {
                sawEvos[i].gameObject.SetActive(false);
            }
        }

        panelSawEvo.SetActive(true);
        UIEffect.instance.FadeAll(sawCanvasGroup, 1, 0.15f);
    }

    public void ShowPanelFlameEvo()
    {
        UpdateFlameEvo();

        for (int i = 0; i < UpgradeEvolutionController.instance.flames.Count; i++)
        {
            FlameSlotEvo flameSlotEvo = GetSlotFlameEvo(UpgradeEvolutionController.instance.flames[i]);
            if (!IsActiveFlameSlot(UpgradeEvolutionController.instance.flames[i]))
            {
                flameSlotEvo.gameObject.SetActive(true);
                flameSlotEvo.transform.SetAsLastSibling();
            }
            flameSlotEvo.SetAmout(UpgradeEvolutionController.instance.GetAmoutFlameEvo(UpgradeEvolutionController.instance.flames[i]));
        }

        List<FLAMEEVO> temp = new List<FLAMEEVO>(flameEvoTypes);

        FLAMEEVO type1 = temp[Random.Range(0, temp.Count)];
        temp.Remove(type1);
        FLAMEEVO type2 = temp[Random.Range(0, temp.Count)];

        for (int i = 0; i < flameEvos.Length; i++)
        {
            if (flameEvos[i].type == type1 || flameEvos[i].type == type2)
            {
                flameEvos[i].gameObject.SetActive(true);
            }
            else
            {
                flameEvos[i].gameObject.SetActive(false);
            }
        }

        panelFlameEvo.SetActive(true);
        UIEffect.instance.FadeAll(flameCanvasGroup, 1, 0.15f);
    }

    public void ShowPanelMachineGunEvo()
    {
        UpdateMachineGunEvo();

        for (int i = 0; i < UpgradeEvolutionController.instance.machineGuns.Count; i++)
        {
            MachineGunSlotEvo machineGunSlotEvo = GetSlotMachineGunEvo(UpgradeEvolutionController.instance.machineGuns[i]);
            if (!IsActiveMachineGunSlot(UpgradeEvolutionController.instance.machineGuns[i]))
            {
                machineGunSlotEvo.gameObject.SetActive(true);
                machineGunSlotEvo.transform.SetAsLastSibling();
            }
            machineGunSlotEvo.SetAmout(UpgradeEvolutionController.instance.GetAmoutMachineGunEvo(UpgradeEvolutionController.instance.machineGuns[i]));
        }

        List<MACHINEGUNEVO> temp = new List<MACHINEGUNEVO>(machineGunEvoTypes);

        MACHINEGUNEVO type1 = temp[Random.Range(0, temp.Count)];
        temp.Remove(type1);
        MACHINEGUNEVO type2 = temp[Random.Range(0, temp.Count)];

        for (int i = 0; i < machineGunEvos.Length; i++)
        {
            if (machineGunEvos[i].type == type1 || machineGunEvos[i].type == type2)
            {
                machineGunEvos[i].gameObject.SetActive(true);
            }
            else
            {
                machineGunEvos[i].gameObject.SetActive(false);
            }
        }

        panelMachineGunEvo.SetActive(true);
        UIEffect.instance.FadeAll(machineGunCanvasGroup, 1, 0.15f);
    }

    public void HidePanelSawEvo()
    {
        sawCanvasGroup.alpha = 0;
        panelSawEvo.SetActive(false);
    }

    public void HidePanelFlameEvo()
    {
        flameCanvasGroup.alpha = 0;
        panelFlameEvo.SetActive(false);
    }

    public void HidePanelMachineGunEvo()
    {
        machineGunCanvasGroup.alpha = 0;
        panelMachineGunEvo.SetActive(false);
    }
}
