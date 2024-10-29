using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockUpgradeController : MonoBehaviour
{
    public static BlockUpgradeController instance;

    public float raycastDistance;
    public LayerMask layerMask;
    public GameObject frameInsert;
    public GameObject frameChoice;
    public GameObject recyleClose;
    public GameObject recyleOpen;
    public TextMeshProUGUI goldInRecyle;
    GameObject blockSelected;
    public WeaponBuyButton[] weaponBuyButtons;
    bool isDrag;
    bool isHold;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (GameController.instance.isStart) return;
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            isHold = false;
            if (blockSelected != null)
            {
                Block scBlock = BlockController.instance.GetScBlock(blockSelected);
                if (Vector2.Distance(blockSelected.transform.position, recyleOpen.transform.position) <= 2f)
                {
                    scBlock.SubtractGold();
                    BlockController.instance.DeleteBlock(blockSelected);
                    scBlock.blockUpgradeHandler.ResetData();
                    BlockController.instance.CheckButtonStateAll();
                }
                blockSelected.transform.position = frameInsert.transform.position;
                scBlock.blockUpgradeHandler.DeSelected();
                SetActiveFrame(false);
                RecyleChange(true);
                blockSelected = null;
            }
        }
        if (isDrag)
        {
            if (blockSelected != null)
            {
                Vector2 pos = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                frameChoice.transform.position = pos;
                blockSelected.transform.position = pos;
                BlockController.instance.SetPositionNearest(blockSelected, frameInsert);
            }
            if (!isHold)
            {
                Vector2 raycastPosition = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero, raycastDistance, layerMask);
                if (hit.collider != null)
                {
                    UIHandler.instance.tutorial.TutorialDragBlock(true);
                    blockSelected = hit.rigidbody.gameObject;

                    Block block = BlockController.instance.GetScBlock(blockSelected);
                    block.blockUpgradeHandler.Selected();

                    frameChoice.transform.position = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                    frameInsert.transform.position = blockSelected.transform.position;

                    SetActiveFrame(true);
                    RecyleChange(false);
                    goldInRecyle.text = UIHandler.instance.ConvertNumberAbbreviation(block.sellingPrice);
                    isHold = true;
                }
            }
        }
    }

    void RecyleChange(bool isActive)
    {
        recyleClose.SetActive(isActive);
        recyleOpen.SetActive(!isActive);
    }

    void SetActiveFrame(bool isActive)
    {
        frameInsert.SetActive(isActive);
        frameChoice.SetActive(isActive);
    }
}