using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public DataManager dataManager;
    public CarController carController;

    public List<GameObject> listEnemies;
    public List<GameObject> listEVisible = new List<GameObject>();

    public GameObject[] mapLevels;

    public Transform poolDamages;
    public Transform poolWeapons;
    public Transform poolBullets;
    public Transform poolEnemies;
    public Transform poolDynamics;
    public Transform poolPars;
    public Transform defaultDir;

    public float timeSawDamage;
    public float timeFlameDamage;
    public float backgroundSpeed;

    public GameObject menuCamera;
    public GameObject gameCamera;
    public GameObject booster;
    public GameObject buttonStart;
    public GameObject touchScreen;
    public GameObject colDisplay;

    public bool isStart;
    public Camera cam;

    public GameObject v;

    private void Awake()
    {
        instance = this;
        DOTween.SetTweensCapacity(200, 1000);
        Resize();
        MapGenerate(dataManager.playerData.gameLevel);
        GenerateColDisplay();
    }

    void GenerateColDisplay()
    {
        Vector2 sceenR = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height / 2));
        Instantiate(colDisplay, new Vector2(sceenR.x + gameCamera.transform.position.x, sceenR.y), Quaternion.identity, transform);
    }

    void MapGenerate(int index)
    {
        Instantiate(mapLevels[index], transform);
    }

    public void Start()
    {
        ChangeBlockSprites(dataManager.playerData.gameLevel);
        ChangeCarSprites(dataManager.playerData.gameLevel);
        BlockController.instance.LoadData();
        Instantiate(v, new Vector2(CarController.instance.transform.position.x + 7, CarController.instance.transform.position.x + 3), Quaternion.identity);
        Instantiate(v, new Vector2(CarController.instance.transform.position.x + 3, CarController.instance.transform.position.y + 7), Quaternion.identity);
    }

    public void EDeathAll(GameObject tower)
    {
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (listEVisible[i] != tower)
            {
                ParController.instance.PlayZomDieParticle(listEVisible[i].transform.position);
            }
        }
        listEVisible.Clear();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            DataManager.instance.playerData.gameLevel = Mathf.Clamp(++DataManager.instance.playerData.gameLevel, 0, 5);
            ChangeBlockSprites(DataManager.instance.playerData.gameLevel);
            ChangeCarSprites(DataManager.instance.playerData.gameLevel);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DataManager.instance.playerData.gameLevel = Mathf.Clamp(--DataManager.instance.playerData.gameLevel, 0, 5);
            ChangeBlockSprites(DataManager.instance.playerData.gameLevel);
            ChangeCarSprites(DataManager.instance.playerData.gameLevel);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Restart();
        }
    }

    public void ChangeBlockSprites(int level)
    {
        dataManager.SetBlockSprites(level);
        BlockController.instance.ResetBlockSprites();
    }

    public void ChangeCarSprites(int level)
    {
        Sprite[] carSprites = dataManager.GetCarSprites(level);

        if (carSprites != null)
        {
            carController.tent_part_3.sprite = carSprites[0];
            carController.tent_part_2.sprite = carSprites[1];
            carController.tent_part_1.sprite = carSprites[2];
            carController.chassis.sprite = carSprites[3];
            carController.wheelLeft.sprite = carSprites[4];
            carController.wheelRight.sprite = carSprites[4];
            carController.shadow.sprite = carSprites[5];
        }
        else
        {
            Debug.LogError("!");
        }
    }

    public void Restart()
    {
        SetValue(false);
        EnemyTowerController.instance.Restart();
        BlockController.instance.Restart();
    }

    void Resize()
    {
        float defaultSize = cam.orthographicSize;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 10.8f / 19.2f;
        if (screenRatio < targetRatio)
        {
            float changeSize = targetRatio / screenRatio;
            cam.orthographicSize = defaultSize * changeSize;
        }
    }

    public void StartGame()
    {
        SetValue(true);
        BlockController.instance.StartGame();
        EnemyTowerController.instance.NextTower();
    }

    void SetValue(bool isActive)
    {
        menuCamera.SetActive(!isActive);
        gameCamera.SetActive(isActive);
        touchScreen.SetActive(isActive);
        buttonStart.SetActive(!isActive);
        booster.SetActive(isActive);
        isStart = isActive;
        BlockController.instance.SetActiveUI(!isActive);
        CarController.instance.multiplier = isActive ? 1 : 0;
    }

    public enum WEAPON
    {
        NONE, SAW, FLAME, MACHINE_GUN
    }


    public Transform GetENearest(Vector2 startPos)
    {
        if (listEVisible.Count == 0) return defaultDir;
        int index = -1;
        float min = float.MaxValue;
        for (int i = 0; i < listEVisible.Count; i++)
        {
            float distance = Vector2.Distance(startPos, listEVisible[i].transform.position);
            if (distance < min)
            {
                min = distance;
                index = i;
            }
        }
        return listEVisible[index].transform;
    }

    public void OnDestroy()
    {
        List<IngameData> listData = new List<IngameData>();
        for (int i = 0; i < BlockController.instance.blocks.Count; i++)
        {
            Block scBlock = BlockController.instance.blocks[i].GetComponent<Block>();
            int blockLevel = scBlock.level;
            int blockGold = scBlock.gold;
            WEAPON weaponType = WEAPON.NONE;
            if (scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter != null) weaponType = scBlock.blockUpgradeHandler.weaponUpgradeHandler.weaponShoter.weaponType;
            int weaponLevel = scBlock.blockUpgradeHandler.weaponUpgradeHandler.level;
            int weaponUpgradeLevel = scBlock.blockUpgradeHandler.weaponUpgradeHandler.levelUpgrade;

            /*Debug.LogWarning(blockLevel);
            Debug.LogWarning(weaponType);
            Debug.LogWarning(weaponLevel);
            Debug.LogWarning(weaponUpgradeLevel);
            Debug.LogWarning("--------------------");*/

            IngameData ingameData = new IngameData(blockLevel, blockGold, weaponType, weaponLevel, weaponUpgradeLevel);
            listData.Add(ingameData);
        }

        string jsIngame = JsonConvert.SerializeObject(listData);
        string filePathIngame = Path.Combine(Application.persistentDataPath, "IngameData.json");
        File.WriteAllText(filePathIngame, jsIngame);

        string jsPlayer = JsonConvert.SerializeObject(DataManager.instance.playerData);
        string filePathPlayer = Path.Combine(Application.persistentDataPath, "PLayerData.json");
        File.WriteAllText(filePathPlayer, jsPlayer);
    }
}
