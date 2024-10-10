using TMPro;
using UnityEngine;

public class BlockUpgradeController : MonoBehaviour
{
    public float raycastDistance;
    public LayerMask layerMask;
    public GameObject frame1;
    public GameObject frame2;
    public GameObject recyleClose;
    public GameObject recyleOpen;
    public TextMeshProUGUI goldInRecyle;
    GameObject blockSelected;
    bool isDrag;
    bool isHold;

    public void Update()
    {
        if(GameController.instance.isStart) return;
        if (Input.GetMouseButtonDown(0))
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
                if (Vector2.Distance(blockSelected.transform.position, recyleOpen.transform.position) <= 1f)
                {
                    scBlock.SubtractGold();
                    BlockController.instance.DeleteBlock(blockSelected);
                    scBlock.blockUpgradeHandler.ResetData();
                    BlockController.instance.CheckButtonStateAll();
                }
                blockSelected.transform.position = frame1.transform.position;
                scBlock.blockUpgradeHandler.DeSelected();
                SetActiveFrame(false);
                RecyleChange(true);
                blockSelected = null;
            }
        }
        if (isDrag)
        {
            if(blockSelected != null)
            {
                Vector2 pos = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                blockSelected.transform.position = pos;
                frame2.transform.position = pos;
                BlockController.instance.SetPositionNearest(blockSelected, frame1);
            }
            if (!isHold)
            {
                Vector2 raycastPosition = GameController.instance.cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.zero, raycastDistance, layerMask);
                if (hit.collider != null)
                {
                    blockSelected = hit.rigidbody.gameObject;
                    frame1.transform.position = blockSelected.transform.position;
                    frame2.transform.position = blockSelected.transform.position;
                    Block block = BlockController.instance.GetScBlock(blockSelected);
                    block.blockUpgradeHandler.Selected();
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
        frame1.SetActive(isActive);
        frame2.SetActive(isActive);
    }
}
