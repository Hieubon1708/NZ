using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class BlockController : MonoBehaviour
{
    public static BlockController instance;

    public List<GameObject> blockPools = new List<GameObject>();
    public List<Block> scBlocks = new List<Block>();
    public List<GameObject> blocks = new List<GameObject>();
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddBlock();
        }
        if (Input.GetMouseButtonDown(1))
        {
            /*foreach (KeyValuePair<GameObject, int> item in GameController.instance.listDamages)
            {
                Debug.LogWarning(item.Value);
            }*/
        }
    }

    public void ResetBlockSprites()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Block scBlock = GetScBlock(blocks[i]);
            scBlock.blockUpgradeHandler.UpgradeHandle();
        }
    }

    public void LoadData()
    {
        IngameData[] ingameDatas = DataManager.instance.ingameDatas;
        for (int i = 0; i < ingameDatas.Length; i++)
        {
            GameObject block = blockPools[0];
            blockPools.Remove(block);
            block.transform.localPosition = new Vector2(block.transform.localPosition.x, startY + distance * blocks.Count);
            blocks.Add(block);
            block.SetActive(true);
            player.transform.localPosition = new Vector2(player.transform.localPosition.x, startYPlayer + distance * blocks.Count);
            Block scBlock = GetScBlock(block);
            scBlock.gold = ingameDatas[i].blockGold;

            BlockUpgradeHandler blockUpgradeHandler = GetScBlock(block).blockUpgradeHandler;

            int blockLevel = ingameDatas[i].blockLevel;
            WEAPON weaponType = ingameDatas[i].weaponType;
            int weaponLevel = ingameDatas[i].weaponLevel;
            int weaponLevelUpgrade = ingameDatas[i].weaponLevelUpgrade;

            blockUpgradeHandler.LoadData(blockLevel, weaponType, weaponLevel, weaponLevelUpgrade);
        }
        energyUpgradee.LoadData();
        CheckButtonStateAll();
    }

    public void SetActiveUI(bool isActive)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            Block scBlock = GetScBlock(blocks[i]);
            scBlock.blockUpgradeHandler.canvas.SetActive(isActive);
        }
        energyUpgradee.gameObject.SetActive(isActive);
        blockBuyer.gameObject.SetActive(isActive);
        goldReward.SetActive(isActive);
    }

    public void CheckButtonStateAll()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            GetScBlock(blocks[i]).blockUpgradeHandler.CheckButtonStateInBlock();
        }
        energyUpgradee.CheckButtonState();
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
            scBlock.blockUpgradeHandler.LoadData();
            scBlock.AddBlockAni();
            CarController.instance.AddBookAni();
            PlayerController.instance.AddBookAni();
            CheckButtonStateAll();
            scBlock.PlusGold(DataManager.instance.blockData.price);
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
        Block scBlock = GetScBlock(block);
        block.SetActive(false);
        blockPools.Add(block);
        blocks.Remove(block);
        scBlock.DeleteBlockAni();
        CarController.instance.DeleteMenuBookAni();
        PlayerController.instance.DeleteBookAni();
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].transform.localPosition = new Vector2(blocks[i].transform.localPosition.x, startY + distance * i);
        }
        player.transform.localPosition = new Vector2(player.transform.localPosition.x, startYPlayer + distance * blocks.Count);
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
}
