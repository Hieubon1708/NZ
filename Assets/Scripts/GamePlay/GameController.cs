using Cinemachine;
using DG.Tweening;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int level;

    public bool isLose;
    public bool isPLayBoss;

    public List<GameObject> listEnemies;
    public List<GameObject> listEVisible = new List<GameObject>();

    public Transform poolDamages;
    public Transform poolWeapons;
    public Transform poolBullets;
    public Transform poolDynamics;
    public Transform poolPars;
    public Transform defaultDir;

    public float xPlus1, xPlus2;
    public float yPlus1, yPlus2;

    public float timeBlockNPlayerDamage;
    public float timeSawDamage;
    public float timeShockerDamage;
    public float timeFlameDamage;
    public float timeFlameBurningDamage;
    public float timeBossDamage;
    public float backgroundSpeed;

    public GameObject menuCamera;
    public GameObject gameCamera;
    public GameObject buttonStart;
    public GameObject touchScreen;
    public GameObject colDisplay;
    public GameObject colStopTower;
    public GameObject menu;

    public GameObject boss;
    public GameObject main;

    public bool isStart;
    public Camera cam;

    public GameObject v;

    private void Awake()
    {
        instance = this;
        DOTween.SetTweensCapacity(200, 1000);
        //Resize();
        GenerateColDisplay();
    }

    void GenerateColDisplay()
    {
        Vector2 sceenR = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height / 2));
        Instantiate(colDisplay, new Vector2(sceenR.x + gameCamera.transform.position.x, sceenR.y), Quaternion.identity, transform);
    }

    public void MapGenerate(int index)
    {
        if (EnemyTowerController.instance != null)
        {
            Destroy(EnemyTowerController.instance.gameObject);
            Restart();
        }
        main = (GameObject)Instantiate(Resources.Load(Path.Combine("Levels", index.ToString())), new Vector2(0, 0.85f), Quaternion.identity, transform);
        ChangeBlockSprites(level);
        ChangeCarSprites(level);
    }

    public void EndGame()
    {
        ShakeCam(0.25f);
        isLose = true;
        touchScreen.SetActive(false);
    }

    public void ShakeCam(float strength)
    {
        gameCamera.transform.DOComplete();
        gameCamera.transform.DOShakePosition(1f, strength);
    }

    public void GetEsByDistance(float distance, Vector2 from, List<Transform> es)
    {
        es.Clear();
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (Vector2.Distance(from, listEVisible[i].transform.position) <= distance) es.Add(listEVisible[i].transform);
        }
    }

    public void Start()
    {
        LoadData();
        MapGenerate(level + 1);
        //boss = (GameObject)Instantiate(Resources.Load(Path.Combine("BossLevels", DataManager.instance.dataStorage.bossDataStorage != null ? (DataManager.instance.dataStorage.bossDataStorage.level + 1).ToString() : "1")), new Vector2(0, 0.85f), Quaternion.identity, transform);
        //UIBoss.instance.bossHandler = boss.GetComponentInChildren<BossHandler>();
        //boss.SetActive(false);
        EquipmentController.instance.LoadData();
        PlayerController.instance.LoadData();
        UIHandler.instance.LoadData();
        UpgradeEvolutionController.instance.LoadData();
        BlockController.instance.LoadData();
        //UIBoss.instance.LoadData();

        /* Instantiate(v, new Vector2(CarController.instance.transform.position.x + 4f, CarController.instance.transform.position.y + 3), Quaternion.identity);
         Instantiate(v, new Vector2(CarController.instance.transform.position.x + 8f, CarController.instance.transform.position.y + 3), Quaternion.identity);
 */
        if (!UIHandler.instance.tutorial.isFirstTimePlay)
        {
            StartGame();
        }
        else
        {
            UIHandler.instance.tutorial.TutorialButtonBuyBlock(false);
        }
    }

    public GameObject EBlockNearest(LayerMask layer)
    {
        float min = float.MaxValue;
        GameObject e = null;
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (listEVisible[i].layer == layer && listEVisible[i].transform.position.x < min)
            {
                min = listEVisible[i].transform.position.x;
                e = listEVisible[i];
            }
        }
        return e;
    }

    void LoadData()
    {
        level = DataManager.instance.dataStorage != null ? DataManager.instance.dataStorage.level : 0;
    }

    public void EDeathAll(GameObject tower)
    {
        for (int i = 0; i < listEVisible.Count; i++)
        {
            if (listEVisible[i] != tower && listEVisible[i].CompareTag("Enemy"))
            {
                ParController.instance.PlayZomDieParticle(listEVisible[i].transform.position);
                EnemyHandler sc = EnemyTowerController.instance.GetScE(listEVisible[i]);
                sc.damage.ShowDamage(sc.enemyInfo.hp, sc.hitObj, false);
                UIHandler.instance.FlyGold(sc.enemyInfo.transform.position, 2);
            }
        }
        EnemyTowerController.instance.DisableEs();
        listEVisible.Clear();
    }

    public void ChangeBlockSprites(int level)
    {
        DataManager.instance.SetBlockSprites(level);
        BlockController.instance.ResetBlockSprites();
    }

    public void ChangeCarSprites(int level)
    {
        Sprite[] carSprites = DataManager.instance.GetCarSprites(level);

        if (carSprites != null)
        {
            CarController.instance.tent_part_3.sprite = carSprites[0];
            CarController.instance.tent_part_2.sprite = carSprites[1];
            CarController.instance.tent_part_1.sprite = carSprites[2];
            CarController.instance.chassis.sprite = carSprites[3];
            CarController.instance.wheelLeft.sprite = carSprites[4];
            CarController.instance.wheelRight.sprite = carSprites[4];
            CarController.instance.shadow.sprite = carSprites[5];
        }
        else
        {
            Debug.LogError("!");
        }
    }

    public void Restart()
    {
        SetValue(false);
        if (!isPLayBoss) EnemyTowerController.instance.Restart();
        else
        {
            UIBoss.instance.bossHandler.Restart();
        }
        UIHandler.instance.Restart();
        PlayerController.instance.Restart();
        ParController.instance.SetActivePar(false);
        BlockController.instance.Restart();
        CarController.instance.Restart();
        Booster.instance.ResetBooster();
        listEVisible.Clear();

        DataManager.instance.SaveDaily();
        DataManager.instance.SavePlayer();
        DataManager.instance.SaveTutorial();
        DataManager.instance.SaveData();
    }

    void Resize()
    {
        float defaultSize = cam.orthographicSize;
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 10.8f / 19.2f;
        if (screenRatio >= targetRatio)
        {
            menuCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize;
            gameCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize;
        }
        else
        {
            float changeSize = targetRatio / screenRatio;
            menuCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize * changeSize;
            gameCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = defaultSize * changeSize;
        }
    }

    public void StartGame()
    {
        if(UIHandler.instance.tutorial.isFirstTimePlay) AudioController.instance.PlaySoundButton(AudioController.instance.buttonClick);
        if (!isPLayBoss) EnemyTowerController.instance.NextTower();
        SetValue(true);
        BlockController.instance.StartGame();
        Booster.instance.StartGame();
        UIHandler.instance.StartGame();
        if (!isPLayBoss) PlayerController.instance.StartGame();
    }

    void SetValue(bool isActive)
    {
        menuCamera.SetActive(!isActive);
        gameCamera.SetActive(isActive);
        if (!isPLayBoss || !isActive) touchScreen.SetActive(isActive);
        if(isPLayBoss) UIBoss.instance.ActiveButtonBack(!isActive);
        buttonStart.SetActive(!isActive);
        if (!isPLayBoss) menu.SetActive(!isActive);
        isStart = isActive;
        Booster.instance.SetActiveBooster(isActive);
        BlockUpgradeController.instance.recyleClose.SetActive(!isActive);
        BlockController.instance.SetActiveUI(!isActive);
        UIHandler.instance.SetValue(isActive);
        CarController.instance.multiplier = isActive ? 1 : 0;
    }

    public enum WEAPON
    {
        NONE, SAW, FLAME, MACHINE_GUN, SHOCKER
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
}
