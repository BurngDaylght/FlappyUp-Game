using UnityEngine;
using UnityEngine.UI;

public class DeleteData : MonoBehaviour
{
	[SerializeField] private Button _button;
		
	private void Start()
	{
		_button.onClick.AddListener(Delete);
	}

	private void Delete()
	{
		IDataService dataService = new JsonDataService();
		dataService.ClearData($"{Application.persistentDataPath}/BirdSkins.json");
		dataService.ClearData($"{Application.persistentDataPath}/EnvironmentSkins.json");
		dataService.ClearData($"{Application.persistentDataPath}/BestScore.json");
		dataService.ClearData($"{Application.persistentDataPath}/CoinsCount.json");
		dataService.ClearData($"{Application.persistentDataPath}/Settings.json");
		
		SceneLoader.instance.RestartScene();
	}
}
