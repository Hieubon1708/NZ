using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

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
                if (Vector2.Distance(blockSelected.transform.position, GameController.instance.cam.ScreenToWorldPoint(recyleOpen.transform.position)) <= 2f)
                {
                    scBlock.SubtractGold();
                    BlockController.instance.DeleteBlock(blockSelected);
                    scBlock.blockUpgradeHandler.ResetData();
                    BlockController.instance.CheckButtonStateAll();
                }
                else
                {
                    blockSelected.transform.position = GameController.instance.cam.ScreenToWorldPoint(frame.transform.position);
                    scBlock.blockUpgradeHandler.DeSelected();
                }
                RecyleChange(true);
                blockUI.gameObject.SetActive(false);
                SetActiveFrame(false);
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
                    Block block = BlockController.instance.GetScBlock(blockSelected);
                    
                    frame.transform.position = GameController.instance.cam.WorldToScreenPoint(blockSelected.transform.position);
                    blockUI.transform.position = Input.mousePosition;
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
