using UnityEngine;
using System.Collections;
using TMPro;

public class GameInfoDisplay : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _gameTitleText;
	[SerializeField] private TextMeshProUGUI _gameVersionText;
	private enum GameVersion
	{
		Release,
		Beta,
		Alpha
	}

	[SerializeField] private GameVersion _gameVersion;

	private IEnumerator Start()
	{
		yield return null;
		_gameTitleText.text = Application.productName;
		
		if (_gameVersion == GameVersion.Release)
		{
			_gameVersionText.text = Application.version + " release";
		}
		else if (_gameVersion == GameVersion.Beta)
		{
			_gameVersionText.text = Application.version + " beta";
		}
		else
		{
			_gameVersionText.text = Application.version + " alpha";
		}
	}
}
