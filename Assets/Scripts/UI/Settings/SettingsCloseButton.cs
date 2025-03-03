public class SettingsCloseButton : UIButtonBase
{
    protected override void OnButtonClick()
    {
        Settings.instance.CloseSettings();
        SFXAudioPlayer.instance.PlaySFX(_swooshSound);
    }
}
