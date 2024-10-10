
using UnityEngine;

public class BlockBuyHandler : ButtonBuyer
{
    public override void Buy()
    {
        BlockController.instance.AddBlock();
        if (BlockController.instance.blockPools.Count == 0) gameObject.SetActive(false);
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.blockConfig.startPrice.ToString();
    }

    public override void CheckButtonState()
    {
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.startPrice) UIHandler.instance.BlockButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, framePrice);
        else UIHandler.instance.BlockButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameButton, framePrice);
    }
}
