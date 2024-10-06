
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
        if (PlayerHandler.instance.playerInfo.gold < DataManager.instance.blockConfig.startPrice) UIHandler.instance.ChangeSpriteBlockUpgradee(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton);
        else UIHandler.instance.ChangeSpriteBlockUpgradee(UIHandler.Type.ENOUGH_MONEY, frameButton);
    }
}
