using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class BlockController : MonoBehaviour
{
    public static BlockController instance;

    public List<GameObject> blockPools = new List<GameObject>();
    public List<Block> scBlocks = new List<Block>();
    public List<GameObject> blocks = new List<GameObject>();
    public List<GameObject> tempBlocks;
    public float startY;
    public float startYPlayer;
    public float distance;
    public Transform container;
    public Transform player;
    public GameObject preBlock;
    public GameObject goldReward;
    public ButtonBuyer blockBuyer;
    public EnergyUpgradeHandler energyUpgradee;
    public int count;

    public void Awake()
    {
        instance = this;
        Generate();
    }

    public void StartGame()
    {
        tempBlocks = new List<GameObject>(blocks);
    }

    public void ResetBlockSprites()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Block scBlock = GetScBlock(blocks[i]);
            scBlock.blockUpgradeHandler.UpgradeHandle();
        }
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);
        int goldReward = UIHandler.instance.progressHandler.gold;
        while (tempBlocks.Count > 0)
        {
            UIHandler.instance.uIEffect.EndFlyGold(tempBlocks[tempBlocks.Count - 1].transform.position);
            UIHandler.instance.uIEffect.EndFlyGold(tempBlocks[tempBlocks.Count - 1].transform.position);
            Block sc = GetScBlock(tempBlocks[tempBlocks.Count - 1]);
            DeleteBlockInGame(tempBlocks[tempBlocks.Count - 1]);
            goldReward += sc.sellingPrice;
            int gold = goldReward;
            int count = tempBlocks.Count;
            sc.sellingPrice = 0;
            DOVirtual.DelayedCall(0.65f, delegate
            {
                UIHandler.instance.progressHandler.textGold.text = gold.ToString();
                if (count == 0) UIHandler.instance.progressHandler.textGold.text = UIHandler.instance.goldTemp.ToString();
            });
            yield return new WaitForSeconds(0.25f);
        }
        for (int i = 0; i < blockPools.Count; i++)
        {
            Block sc = GetScBlock(blockPools[i]);
            sc.sellingPrice = 0;
            sc.blockUpgradeHandler.ResetData();
        }
        yield return new WaitForSeconds(1f);
        UIHandler.instance.progressHandler.ShowConvert(UIHandler.instance.goldTemp);
    }

    public void Restart()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].transform.DOKill();
            blocks[i].transform.localPosition = new Vector2(blocks[i].transform.localPosition.x, startY + distance * i);
            Block sc = GetScBlock(blocks[i]);
            sc.StopDeleteBlockAni();
            sc.blockHandler.Restart();
            BlockUpgradeHandler blockUpgradeHandler = sc.blockUpgradeHandler;
            if (blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.Restart();
            if (!blocks[i].activeSelf) blocks[i].SetActive(true);
        }
        player.transform.DOKill();
        player.transform.localPosition = new Vector2(player.transform.localPosition.x, startYPlayer + distance * blocks.Count);
    }

    public bool IsWeaponExistBlock(WEAPON type)
    {
        bool isExist = false;
        for (int i = 0; i < blocks.Count; i++)
        {
            Block sc = GetScBlock(blocks[i]);
            if (sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType == type)
            {
                isExist = true;
            }
        }
        return isExist;
    }

    public TutorialOject GetTutorialWeaponEvo(out int price)
    {
        price = 0;
        for (int i = 0; i < blocks.Count; i++)
        {
            Block scBlock = GetScBlock(blocks[i]);
            if (scBlock.blockUpgradeHandler.weaponUpgradeHandler != null)
            {
                TutorialOject tutorialOject = scBlock.blockUpgradeHandler.weaponUpgradeHandler.GetTutorialOject(out price);
                if (tutorialOject != null) return tutorialOject;
            }
        }
        return null;
    }

    public void LoadData()
    {
        for (int i = 0; i < DataManager.instance.blockDataStorage.Length; i++)
        {
            GameObject block = blockPools[0];
            blockPools.Remove(block);
            block.transform.localPosition = new Vector2(block.transform.localPosition.x, startY + distance * blocks.Count);
            blocks.Add(block);
            block.SetActive(true);
            Block scBlock = GetScBlock(block);
            scBlock.sellingPrice = DataManager.instance.blockDataStorage[i].sellingPrice;

            BlockUpgradeHandler blockUpgradeHandler = GetScBlock(block).blockUpgradeHandler;

            int blockLevel = DataManager.instance.blockDataStorage[i].level;
            WEAPON weaponType = DataManager.instance.blockDataStorage[i].weaponDataStorage.weaponType;
            int weaponLevel = DataManager.instance.blockDataStorage[i].weaponDataStorage.weaponLevel;
            int weaponLevelUpgrade = DataManager.instance.blockDataStorage[i].weaponDataStorage.weaponLevelUpgrade;

            blockUpgradeHandler.LoadData(blockLevel, weaponType, weaponLevel, weaponLevelUpgrade);
            if (blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.LoadData();
        }
        player.transform.localPosition = new Vector2(player.transform.localPosition.x, startYPlayer + distance * blocks.Count);
        energyUpgradee.LoadData();
        CheckButtonStateAll();
        if (UIHandler.instance.tutorial.isFirstTimeDestroyTower) goldReward.SetActive(true);
    }

    public void SetActiveUI(bool isActive)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Block scBlock = GetScBlock(blocks[i]);
            scBlock.blockUpgradeHandler.canvas.SetActive(isActive);
            if (!isActive && !GameController.instance.isPLayBoss) scBlock.blockUpgradeHandler.weaponUpgradeHandler.StartGame();
        }
        energyUpgradee.gameObject.SetActive(isActive);
        blockBuyer.gameObject.SetActive(isActive);
        if (UIHandler.instance.tutorial.isFirstTimeDestroyTower) goldReward.SetActive(isActive);
    }

    public void CheckButtonStateAll()
    {
        if (blockPools.Count == 0) blockBuyer.gameObject.SetActive(false);
        for (int i = 0; i < blocks.Count; i++)
        {
            GetScBlock(blocks[i]).blockUpgradeHandler.CheckButtonStateInBlock();
        }
        blockBuyer.CheckButtonState();
        energyUpgradee.CheckButtonState();
        DataManager.instance.SavePlayer();
        DataManager.instance.SaveBlock();
    }

    public void AddBlock()
    {
        if (blockPools.Count != 0)
        {
            GameObject block = blockPools[0];
            Block scBlock = GetScBlock(block);
            blockPools.Remove(block);
            block.transform.localPosition = new Vector2(block.transform.localPosition.x, startY + distance * blocks.Count);
            blocks.Add(block);
            block.SetActive(true);
            player.transform.localPosition = new Vector2(player.transform.localPosition.x, startYPlayer + distance * blocks.Count);
            scBlock.blockUpgradeHandler.UpgradeHandle();
            scBlock.blockUpgradeHandler.LoadData(false);
            scBlock.AddBlockAni();
            CarController.instance.AddBookAni();
            PlayerController.instance.AddBookAni();
            scBlock.PlusGold(DataManager.instance.blockConfig.startPrice);
        }
    }

    public void UseBooster(WEAPON weaponType)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (!blocks[i].activeSelf) continue;
            Block sc = GetScBlock(blocks[i]);
            if (weaponType == WEAPON.SAW && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is SawHandler) sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.UseBooster();
            else if (weaponType == WEAPON.FLAME && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is FlameHandler) sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.UseBooster();
            else if (weaponType == WEAPON.MACHINE_GUN && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is MachineGunHandler) sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.UseBooster();
            else if (weaponType == WEAPON.SHOCKER && sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter is ShockerHandler) sc.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.UseBooster();
        }
    }

    public void SetPositionNearest(GameObject block, GameObject frame)
    {
        int indexNearest = GetIndexNearest(block);
        if (block == blocks[indexNearest]) return;
        Swap(blocks.IndexOf(block), indexNearest);
        for (int i = 0; i < blocks.Count; i++)
        {
            float y = startY + distance * i;
            if (blocks[i] != block) blocks[i].transform.localPosition = new Vector2(blocks[i].transform.localPosition.x, y);
            else frame.transform.position = new Vector2(frame.transform.position.x, y + CarController.instance.transform.localPosition.y);
        }
    }

    void Swap(int index1, int index2)
    {
        GameObject temp = blocks[index1];
        blocks[index1] = blocks[index2];
        blocks[index2] = temp;
    }

    int GetIndexNearest(GameObject block)
    {
        int indexNearest = -1;
        float min = int.MaxValue;
        for (int i = 0; i < blocks.Count; i++)
        {
            float y = startY + distance * i + CarController.instance.transform.localPosition.y;
            float distanceY = Mathf.Abs(block.transform.position.y - y);
            if (distanceY < min)
            {
                min = distanceY;
                indexNearest = i;
            }
        }
        return indexNearest;
    }

    public void DeleteBlock(GameObject block)
    {
        if (blockPools.Count == 0) blockBuyer.gameObject.SetActive(true);
        block.SetActive(false);
        blockPools.Add(block);
        blocks.Remove(block);
        CarController.instance.DeleteMenuBookAni();
        PlayerController.instance.DeleteBookAni();
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].transform.localPosition = new Vector2(blocks[i].transform.localPosition.x, startY + distance * i);
        }
        player.transform.localPosition = new Vector2(player.transform.localPosition.x, startYPlayer + distance * blocks.Count);
    }

    public void SellAllBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Block sc = GetScBlock(blocks[i]);
            PlayerController.instance.player.gold += sc.sellingPrice;
        }
    }

    public BlockDataStorage[] GetBlocks()
    {
        BlockDataStorage[] blockDataStorages = new BlockDataStorage[blocks.Count];
        for (int i = 0; i < blocks.Count; i++)
        {
            Block scBlock = GetScBlock(blocks[i]);
            int blockLevel = scBlock.level;
            int blockGold = scBlock.sellingPrice;

            WEAPON weaponType = WEAPON.NONE;

            if (scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) weaponType = scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType;

            int weaponLevel = scBlock.blockUpgradeHandler.weaponUpgradeHandler.level;
            int weaponUpgradeLevel = scBlock.blockUpgradeHandler.weaponUpgradeHandler.levelUpgrade;

            WeaponDataStorage weaponDataStorage = new WeaponDataStorage(weaponType, weaponLevel, weaponUpgradeLevel);
            blockDataStorages[i] = new BlockDataStorage(blockLevel, blockGold, weaponDataStorage);
        }
        return blockDataStorages;
    }

    public void SaveData()
    {
        /*DataManager.instance.dataStorage.blockDataStorage = GetBlocks();
        DataManager.instance.dataStorage.energyDataStorage.level = energyUpgradee.level;
        DataManager.instance.dataStorage.playerDataStorage.gold = PlayerController.instance.player.gold;
        DataManager.instance.dataStorage.lastRewardTime = UIHandler.instance.lastRewardTime;
        DataManager.instance.dataStorage.goldRewardHighest = UIHandler.instance.goldRewardHighest;
        DataManager.instance.dataStorage.weaponEvolutionDataStorge = UpgradeEvolutionController.instance.GetData();*/
    }

    public void DeleteBlockInGame(GameObject block)
    {
        Block scBlock = GetScBlock(block);
        tempBlocks.Remove(block);
        scBlock.DeleteBlockAni();
        if (scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.DisableWeapon();
        ParController.instance.PlayBlockDestroyParticle(block.transform.position);
        if (!GameController.instance.isLose)
        {
            PlayerController.instance.DeleteBookAni();
            CarController.instance.DeleteGameBookAni();
        }
        for (int i = 0; i < tempBlocks.Count; i++)
        {
            tempBlocks[i].transform.DOComplete();
            tempBlocks[i].transform.DOLocalMoveY(startY + distance * i, 0.25f).SetEase(Ease.Linear);
        }
        player.transform.DOComplete();
        player.transform.DOLocalMoveY(startYPlayer + distance * tempBlocks.Count, 0.25f).SetEase(Ease.Linear).OnComplete(delegate { scBlock.gameObject.SetActive(false); });
    }

    public void ClearBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blockPools.Add(blocks[i]);
        }
        blocks.Clear();
    }

    public Block GetScBlock(GameObject block)
    {
        for (int i = 0; i < scBlocks.Count; i++)
        {
            if (scBlocks[i].gameObject == block)
            {
                return scBlocks[i];
            }
        }
        return null;
    }

    void Generate()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject blockIns = Instantiate(preBlock, container);
            blockIns.name = i.ToString();
            blockIns.SetActive(false);
            Block scBlock = blockIns.GetComponent<Block>();
            blockPools.Add(blockIns);
            scBlocks.Add(scBlock);
        }
    }

    public void LoadWeaponBuyButtonInCurrentLevel(int level)
    {
        for (int i = 0; i < scBlocks.Count; i++)
        {
            scBlocks[i].blockUpgradeHandler.LoadWeaponBuyButtonInCurrentLevel(level);
        }
    }

    public void DisableWeapons()
    {
        for (int i = 0; i < scBlocks.Count; i++)
        {
            if (scBlocks[i].blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null && scBlocks[i].gameObject.activeSelf)
            {
                scBlocks[i].blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.DisableWeapon();
            }
        }
    }
}
