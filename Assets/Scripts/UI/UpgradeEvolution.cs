using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UpgradeEvolutionController;

public class UpgradeEvolution : MonoBehaviour
{
    public static UpgradeEvolution instance;

    public CanvasGroup sawCanvasGroup;
    public CanvasGroup flameCanvasGroup;
    public CanvasGroup machineGunCanvasGroup;

    List<SAWEVO> sawEvoTypes = new List<SAWEVO>() { SAWEVO.INCREASEDAMAGE, SAWEVO.STUNENEMY, SAWEVO.ADDFROMBOOSTER, SAWEVO.DECREASEENERGY, SAWEVO.PUSHESENEMIES };

    public GameObject panelSawEvo;
    public GameObject panelFlameEvo;
    public GameObject panelMachineGunEvo;

    public SawEvo[] sawEvos;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSawEvo()
    {
        if (UpgradeEvolutionController.instance.IsSawMaxType(SAWEVO.STUNENEMY)) sawEvoTypes.Remove(SAWEVO.STUNENEMY);
        if (UpgradeEvolutionController.instance.IsSawMaxType(SAWEVO.PUSHESENEMIES)) sawEvoTypes.Remove(SAWEVO.PUSHESENEMIES);
        if (UpgradeEvolutionController.instance.IsSawMaxType(SAWEVO.ADDFROMBOOSTER)) sawEvoTypes.Remove(SAWEVO.ADDFROMBOOSTER);
        if (UpgradeEvolutionController.instance.IsSawMaxType(SAWEVO.DECREASEENERGY)) sawEvoTypes.Remove(SAWEVO.DECREASEENERGY);
        if (UpgradeEvolutionController.instance.IsSawMaxType(SAWEVO.INCREASEDAMAGE)) sawEvoTypes.Remove(SAWEVO.INCREASEDAMAGE);

        for (int i = 0; i < sawEvoTypes.Count; i++)
        {
            for (int j = 0; j < sawEvos.Length; j++)
            {
                string text = "";
                if (sawEvoTypes[i] == sawEvos[j].type)
                {
                    if (sawEvos[j].type == SAWEVO.STUNENEMY)
                    {
                        int level = UpgradeEvolutionController.instance.GetAmoutSawEvo(SAWEVO.STUNENEMY);
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
                        int level = UpgradeEvolutionController.instance.GetAmoutSawEvo(SAWEVO.PUSHESENEMIES);
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
                        int level = UpgradeEvolutionController.instance.GetAmoutSawEvo(SAWEVO.INCREASEDAMAGE);
                        int percentage = 0;

                        if (level == 1) percentage = 20;
                        else if (level == 2) percentage = 40;
                        else if (level == 3) percentage = 60;
                        text = "+" + percentage + "% damage";
                    }
                }
                sawEvos[j].content.text = text;
                break;
            }
        }
    }

    public void ShowPanelSawEvo()
    {
        List<SAWEVO> temp = sawEvoTypes;

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
        UIEffect.instance.FadeAll(sawCanvasGroup, 1, 0.25f);
    }

    public void ShowPanelFlameEvo()
    {

    }

    public void ShowPanelMachineGunEvo()
    {

    }
}
