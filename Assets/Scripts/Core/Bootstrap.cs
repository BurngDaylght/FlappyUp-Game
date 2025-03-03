using UnityEngine;

public class Bootstrap : MonoBehaviour
{
	[SerializeField] private GameState _gameState;
	[SerializeField] private Score _score;
	[SerializeField] private ResultPanel _resultPanel;
	[SerializeField] private Shop _shop;
	[SerializeField] private Settings _settings;
	[SerializeField] private Skins _skins;
	[SerializeField] private CoinWallet _coinWallet;
	[SerializeField] private SaveLoadHandler _saveLoadHandler;
	[SerializeField] private RestartGame _restartGame;
	[SerializeField] private BlackoutScreen _blackoutScreen;

	private void Awake()
	{
		_gameState.Initialize();
		_saveLoadHandler.Initialize();
		
		_score.Initialize();
		_coinWallet.Initialize();
		_resultPanel.Initialize();
		_shop.Initialize();
		_settings.Initialize();
		_skins.Initialize();

		_restartGame.Initialize();
		_blackoutScreen.Initialize();
	}
	
	private void Start()
	{
		_saveLoadHandler.LoadData();
	}
}
