using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour
{
    public Image energyBar;
    public int amoutEnergy;
    public TextMeshProUGUI textAmoutEnergy;
    public GameObject sawBooster;
    public GameObject flameBooster;
    public GameObject machineGunBooster;
    public WeaponBooster[] weaponBoosters;

    private void OnEnable()
    {
        ResetBooster();
        EnableBooster();
        CheckBoosterState();
        DoFill();
    }

    void DoFill()
    {
        //1f / DataManager.instance.energyData.times[DataManager.instance.playerData.indexEnergy]
        energyBar.DOFillAmount(1, 1).SetEase(Ease.Linear).OnComplete(delegate
        {
            amoutEnergy++;
            energyBar.fillAmount = 0;
            CheckBoosterState();
            DoFill();
        });
    }

    public void CheckBoosterState()
    {
        for (int i = 0; i < weaponBoosters.Length; i++)
        {
            weaponBoosters[i].CheckBooterState();
        }
        textAmoutEnergy.text = amoutEnergy.ToString();
    }

    void ResetBooster()
    {
        if (sawBooster.activeSelf) sawBooster.SetActive(false);
        if (flameBooster.activeSelf) flameBooster.SetActive(false);
        if (machineGunBooster.activeSelf) machineGunBooster.SetActive(false);
    }

    void EnableBooster()
    {
        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block sc = BlockController.instance.GetScBlock(BlockController.instance.blocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponType == GameController.WEAPON.SAW) sawBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponType == GameController.WEAPON.FLAME) flameBooster.SetActive(true);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponType == GameController.WEAPON.MACHINE_GUN) machineGunBooster.SetActive(true);
        }
    }

    private void OnDisable()
    {
        energyBar.DOKill();
        energyBar.fillAmount = 0;
    }
}
