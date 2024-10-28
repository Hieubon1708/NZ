using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockUpgradeController : MonoBehaviour
{
    public static BlockUpgradeController instance;

    public float raycastDistance;
    public LayerMask layerMask;
    public GameObject frame;
    public GameObject recyleClose;
    public GameObject recyleOpen;
    public TextMeshProUGUI goldInRecyle;
    GameObject blockSelected;
    public GameObject blockUI;
    public Image imageBlockUI;
    public WeaponBuyButton[] weaponBuyButtons;
    bool isDrag;
    bool isHold;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
            if (GameController.instance.isStart) return;
            isHold = false;
            if (blockSelected != null)
            {
                Block scBlock = BlockController.instance.GetScBlock(blockSelected);
                if (Vector2.Distance(blockSelected.transform.position, GameController.instance.cam.ScreenToWorldPoint(recyleOpen.transform.position)) <= 2f)
                {
                    scBlock.SubtractGold();
                    BlockController.instance.DeleteBlock(blockSelected);
                    scBlock.blockUpgradeHandler.ResetData();
                    BlockController.instance.CheckButtonStateAll();
                }
                blockSelected.transform.position = frame.transform.position;
                scBlock.blockUpgradeHandler.DeSelected();
                SetActiveFrame(false);
                RecyleChange(true);
                blockUI.gameObject.SetActive(false);
                blockSelected = null;
            }
        }
        if (isDrag)
        {
            if (blockSelected != null)
            {
                Vector2 pos = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                blockSelected.transform.position = pos;
                blockUI.transform.position = Input.mousePosition;
                BlockController.instance.SetPositionNearest(blockSelected, frame);
            }
            if (!isHold)
            {
                Vector2 raycastPosition = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero, raycastDistance, layerMask);
                if (hit.collider != null)
                {
                    UIHandler.instance.tutorial.TutorialDragBlock(true);
                    blockSelected = hit.rigidbody.gameObject;

                    frame.transform.position = blockSelected.transform.position;
                    Block block = BlockController.instance.GetScBlock(blockSelected);

                    block.blockUpgradeHandler.Selected();
                    SetActiveFrame(true);
                    RecyleChange(false);

                    imageBlockUI.sprite = block.sp.sprite;
                    imageBlockUI.SetNativeSize();
                    blockUI.SetActive(true);

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
        frame.SetActive(isActive);
    }
}
