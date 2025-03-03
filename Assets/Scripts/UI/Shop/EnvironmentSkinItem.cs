public class EnvironmentSkinItem : ShopItemBase<EnvironmentSkinData>
{
	protected override void SetItem()
	{
		if (!isPurchased || isEquipped) 
		{
			return;
		}

		SFXAudioPlayer.instance.PlaySFX(_setSound);
		Skins.instance.SetEnvironmentEquippedSkin(this);
		Skins.instance.SaveData();
		UpdateButtons();
	}
}
