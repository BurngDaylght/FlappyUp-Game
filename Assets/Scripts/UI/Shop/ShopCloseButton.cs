public class ShopCloseButton : UIButtonBase
{
    protected override void OnButtonClick()
    {
        Shop.instance.CloseShop();
        StartGame.instance.SetButtonInteractable(true);
        SFXAudioPlayer.instance.PlaySFX(_swooshSound);
    }
}
