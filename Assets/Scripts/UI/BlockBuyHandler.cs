public class BlockBuyHandler : ButtonBuyer
{
    public override void Buy()
    {
        BlockController.instance.AddBlock();
        UIHandler.instance.tutorial.TutorialButtonBuyBlock(true);
        if (BlockController.instance.blockPools.Count == 0) gameObject.SetActive(false);
        BlockController.instance.CheckButtonStateAll();
    }

    public override void LoadData()
    {
        textPrice.text = DataManager.instance.blockConfig.startPrice.ToString();
    }

    public override void CheckButtonState()
    {
        if (PlayerController.instance.player.gold < DataManager.instance.blockConfig.startPrice) UIHandler.instance.BlockButtonChangeState(UIHandler.Type.NOT_ENOUGH_MONEY, frameButton, framePrice);
        else UIHandler.instance.BlockButtonChangeState(UIHandler.Type.ENOUGH_MONEY, frameButton, framePrice);
    }
}
