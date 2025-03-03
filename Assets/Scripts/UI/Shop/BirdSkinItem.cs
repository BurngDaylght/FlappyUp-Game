public class BirdSkinItem : ShopItemBase<BirdSkinData>
{
	protected override void SetItem()
	{
		if (!isPurchased || isEquipped) 
		{
			return;
		}

		SFXAudioPlayer.instance.PlaySFX(_setSound);
		Skins.instance.SetBirdEquippedSkin(this);
		Skins.instance.SaveData();
		UpdateButtons();
	}
}
