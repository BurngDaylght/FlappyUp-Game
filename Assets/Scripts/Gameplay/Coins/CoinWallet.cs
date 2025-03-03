using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.IO;

public class CoinWallet : MonoBehaviour
{
	
	public static CoinWallet instance;

    [Header("Coin Data")]
    [SerializeField] private int _coinsCount;
    public int coinsInSessionCount;

    [Header("UI Elements")]
    [SerializeField] private Transform _coinCointainer;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private RawImage _coinIcon;

    [Header("Popup Prefabs")]
    [SerializeField] private GameObject _addCoinPopupTextPrefab;
    [SerializeField] private GameObject _removeCoinPopupTextPrefab;

    [Header("Animation Settings")]
    [Range(0, 1)]
    [SerializeField] private float _animationDuration;
    [Range(0, 1)]
    [SerializeField] private float _fadeDuration;

    [Header("Audio Settings")]
    [SerializeField] protected AudioClip _coinSound;
    [Range(0, 1)]
    [SerializeField] protected float _coinSoundVolume;
    [SerializeField] protected AudioClip _noMoneySound;
    [Range(0, 1)]
    [SerializeField] protected float _noMoneySoundVolume;

	public void Initialize()
	{
		instance = this;
		
		GameState.instance.onGameOver += FadeAnimation;
		GameState.instance.onGameOver += SaveData;
		
		SaveLoadHandler.instance.onGameSave += SaveData;
		SaveLoadHandler.instance.onGameLoad += LoadData;
	}
	
	private void Start()
	{
		LoadData();
		coinsInSessionCount = 0;
	}
	
	private void AddCoinAnimation()
	{
		float initialScale = _coinText.transform.localScale.x;
		float midScale = initialScale * 1.2f;
		float shrinkScale = initialScale * 0.8f;

		_coinText.transform.DOScale(shrinkScale, _animationDuration)
			.OnComplete(() =>
			{
				UpdateText();
				_coinText.transform.DOScale(midScale, _animationDuration)
					.OnComplete(() => 
					{
						_coinText.transform.DOScale(initialScale, _animationDuration);
					});
			});
	}
	
	public void NotEnoughMoneyAnimation()
	{
		Color originalColor = _coinText.color;
		
		_coinText.DOColor(Color.red, 0.2f)
			.OnComplete(() => _coinText.DOColor(Color.white, 0.2f));

		_coinText.transform.DOShakePosition(0.5f, strength: new Vector3(10f, 0, 0), vibrato: 10, randomness: 0);

		SFXAudioPlayer.instance.PlaySFX(_noMoneySound, _noMoneySoundVolume);
	}

	private void FadeAnimation()
	{
		AddCoinAnimation();

		_coinText.DOFade(0f, _fadeDuration);
		_coinIcon.DOFade(0f, _fadeDuration);
	}
	
	private void UpdateText()
	{
		float startValue;
		float parsedValue;
		
		if (float.TryParse(_coinText.text, out parsedValue)) 
		{
			startValue = parsedValue;
		}
		else 
		{
			startValue = 0;
		}
		
		if ((_coinsCount - startValue) < 5)
		{
			_coinText.text = _coinsCount.ToString();
			return;
		}

		DOTween.To(() => startValue, x => 
		{
			_coinText.text = Mathf.RoundToInt(x).ToString();
		}, _coinsCount, 1f)
		.SetEase(Ease.OutCubic);
	}
	
	public void AddCoins(int count)
	{
		_coinsCount += count;
		coinsInSessionCount += count;

		GameObject popupInstance = Instantiate(_addCoinPopupTextPrefab, _coinCointainer.position, Quaternion.identity, _coinCointainer.parent);
		
		TextPopup popup = popupInstance.GetComponent<TextPopup>();
		popup.Setup($"+{count}");

		SFXAudioPlayer.instance.PlaySFX(_coinSound, _coinSoundVolume);

		AddCoinAnimation();
	}
	
	public void RemoveCoins(int count)
	{
		_coinsCount -= count;

		GameObject popupInstance = Instantiate(_removeCoinPopupTextPrefab, _coinCointainer.position, Quaternion.identity, _coinCointainer.parent);
		
		TextPopup popup = popupInstance.GetComponent<TextPopup>();
		popup.Setup($"-{count}");

		AddCoinAnimation();
	}
	
	public int GetCoins()
	{
		return _coinsCount;
	}
	
	
	public void SaveData()
	{
		IDataService dataService = new JsonDataService();
		
		CoinsData coinsData = new CoinsData(_coinsCount);
		dataService.SaveData($"{Application.persistentDataPath}/CoinsCount.json", coinsData, false);
	}
	
	public void LoadData()
	{		
		IDataService dataService = new JsonDataService();
		string filePath = $"{Application.persistentDataPath}/CoinsCount.json";

		if (File.Exists(filePath))
        {
			CoinsData coinsData = dataService.LoadData<CoinsData>(filePath, false);
			_coinsCount = coinsData.coinsCount;
		}
		else
		{
		    _coinsCount = 0;
			_coinText.text = "0";
		}
		
		UpdateText();
	}
}

[Serializable]
public class CoinsData
{
	public int coinsCount;

	public CoinsData(int coinsCount)
	{
		this.coinsCount = coinsCount;
	}
}
