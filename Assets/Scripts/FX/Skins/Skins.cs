using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class Skins : MonoBehaviour
{
	public static Skins instance;
	
	[Header("All Skins")]
	[SerializeField] private GameObject _birdSkinsContent;
	[SerializeField] private GameObject _environmentSkinsContent;
	[SerializeField] private List<BirdSkinItem> _allBirdSkins;
	[SerializeField] private List<EnvironmentSkinItem> _allEnvironmentSkins;

	[Header("Bird Skins")]
	[SerializeField] private BirdSkinItem _equippedBirdSkin;
	[SerializeField] private BirdSkinItem _defaultBirdSkin;

	[Header("Enviroment Skins")]
	[SerializeField] private EnvironmentSkinItem _equippedEnvironmentSkin;
	[SerializeField] private EnvironmentSkinItem _defaultEnvironmentSkin;

	public void Initialize()
	{
		instance = this;
		
		SaveLoadHandler.instance.onGameSave += SaveData;
		SaveLoadHandler.instance.onGameLoad += LoadData;

		foreach (Transform child in _birdSkinsContent.transform)
		{
			BirdSkinItem skin = child.GetComponent<BirdSkinItem>();
			if (skin != null)
			{
				_allBirdSkins.Add(skin);
			}
		}

		foreach (Transform child in _environmentSkinsContent.transform)
		{
			EnvironmentSkinItem skin = child.GetComponent<EnvironmentSkinItem>();
			if (skin != null)
			{
				_allEnvironmentSkins.Add(skin);
			}
		}
	}
	
	public void SaveData()
    {
        IDataService dataService = new JsonDataService();
        
        List<BirdSkinSaveData> birdSkinSaveDataList = new List<BirdSkinSaveData>();
        for (int i = 0; i < _allBirdSkins.Count; i++)
        {
            birdSkinSaveDataList.Add(new BirdSkinSaveData(i, _allBirdSkins[i].isPurchased, _allBirdSkins[i].isEquipped));
        }
        dataService.SaveData($"{Application.persistentDataPath}/BirdSkins.json", birdSkinSaveDataList, false);
        
        List<EnvironmentSkinSaveData> environmentSkinSaveDataList = new List<EnvironmentSkinSaveData>();
        for (int i = 0; i < _allEnvironmentSkins.Count; i++)
        {
            environmentSkinSaveDataList.Add(new EnvironmentSkinSaveData(i, _allEnvironmentSkins[i].isPurchased, _allEnvironmentSkins[i].isEquipped));
        }
        dataService.SaveData($"{Application.persistentDataPath}/EnvironmentSkins.json", environmentSkinSaveDataList, false);
    }
		
	public void LoadData()
	{
		string birdSkinsfilePath = $"{Application.persistentDataPath}/BirdSkins.json";
		string environmentSkinsfilePath = $"{Application.persistentDataPath}/EnvironmentSkins.json";

		IDataService dataService = new JsonDataService();

		if (File.Exists(birdSkinsfilePath))
		{
			List<BirdSkinSaveData> birdSkinSaveDataList = dataService.LoadData<List<BirdSkinSaveData>>(birdSkinsfilePath, false);

			if (birdSkinSaveDataList != null && birdSkinSaveDataList.Count > 0)
			{
				for (int i = 0; i < birdSkinSaveDataList.Count && i < _allBirdSkins.Count; i++)
				{
					var skin = _allBirdSkins[i];
					var savedData = birdSkinSaveDataList[i];

					skin.isPurchased = savedData.isPurchased;
					skin.isEquipped = savedData.isEquipped;
					skin.UpdateButtons();

					if (skin.isEquipped)
					{
						SetBirdEquippedSkin(skin);
						SkinAnimator.instance.SetSkin(skin.itemData.animatorController);
					}
				}
			}
			else
			{
				SetDefaultBirdSkin();
			}
		}
		else
		{
			SetDefaultBirdSkin();
		}

		if (File.Exists(environmentSkinsfilePath))
		{
			List<EnvironmentSkinSaveData> environmentSkinSaveDataList = dataService.LoadData<List<EnvironmentSkinSaveData>>(environmentSkinsfilePath, false);

			if (environmentSkinSaveDataList != null && environmentSkinSaveDataList.Count > 0)
			{
				for (int i = 0; i < environmentSkinSaveDataList.Count && i < _allEnvironmentSkins.Count; i++)
				{
					var skin = _allEnvironmentSkins[i];
					var savedData = environmentSkinSaveDataList[i];

					skin.isPurchased = savedData.isPurchased;
					skin.isEquipped = savedData.isEquipped;
					skin.UpdateButtons();

					if (skin.isEquipped)
					{
						SetEnvironmentEquippedSkin(skin);
					}
				}
			}
			else
			{
				SetDefaultEnvironmentSkin();
			}
		}
		else
		{
			SetDefaultEnvironmentSkin();
		}
	}
	
	private void SetDefaultBirdSkin()
	{
		SetBirdEquippedSkin(_defaultBirdSkin);
		_defaultBirdSkin.isPurchased = true;
		_defaultBirdSkin.UpdateButtons();
	}
	
	public void SetBirdEquippedSkin(BirdSkinItem equippedSkin)
	{    
		if (_equippedBirdSkin != null)
		{
			_equippedBirdSkin.isEquipped = false;
			_equippedBirdSkin.UpdateButtons();
		}
		
		_equippedBirdSkin = equippedSkin;
		_equippedBirdSkin.isEquipped = true;
		_equippedBirdSkin.UpdateButtons();

		SkinAnimator.instance.SetSkin(_equippedBirdSkin.itemData.animatorController);
	}
	
	private void SetDefaultEnvironmentSkin()
	{
		SetEnvironmentEquippedSkin(_defaultEnvironmentSkin);
		_defaultEnvironmentSkin.isPurchased = true;
		_defaultEnvironmentSkin.UpdateButtons();
	}
	
	public void SetEnvironmentEquippedSkin(EnvironmentSkinItem environmentSkinItem)
	{
		if (_equippedEnvironmentSkin != null)
		{
			_equippedEnvironmentSkin.isEquipped = false;
			_equippedEnvironmentSkin.UpdateButtons();
		}

		_equippedEnvironmentSkin = environmentSkinItem;
		_equippedEnvironmentSkin.isEquipped = true;
		_equippedEnvironmentSkin.UpdateButtons();
		
		BackgroundSkin.instance.SetSkin(environmentSkinItem.itemData.environmentBackground);
		GroundSkin.instance.SetSkin(environmentSkinItem.itemData.environmentGround);
		ObstacleSpawner.instance.SetSkinSprite(environmentSkinItem.itemData.environmentPipes);
	}
}

[Serializable]
public class BirdSkinSaveData
{
	public int skinID;
	public bool isPurchased;
	public bool isEquipped;

	public BirdSkinSaveData(int skinID, bool isPurchased, bool isEquipped)
	{
		this.skinID = skinID;
		this.isPurchased = isPurchased;
		this.isEquipped = isEquipped;
	}
}

[Serializable]
public class EnvironmentSkinSaveData
{
	public int skinID;
	public bool isPurchased;
	public bool isEquipped;

	public EnvironmentSkinSaveData(int skinID, bool isPurchased, bool isEquipped)
	{
		this.skinID = skinID;
		this.isPurchased = isPurchased;
		this.isEquipped = isEquipped;
	}
}