public class ShopOpenButton : UIButtonBase
{
    private void OnEnable()
    {
        GameState.instance.onGameStart += HideButton;
        Shop.instance.onShopOpen += HideButton;
        Shop.instance.onShopClose += ShowButton;
        Settings.instance.onSettingsOpen += HideButton;
        Settings.instance.onSettingsClose += ShowButton;
    }

    protected override void OnButtonClick()
    {
        Shop.instance.OpenShop();
        StartGame.instance.SetButtonInteractable(false);
        SFXAudioPlayer.instance.PlaySFX(_swooshSound);
    }
}
