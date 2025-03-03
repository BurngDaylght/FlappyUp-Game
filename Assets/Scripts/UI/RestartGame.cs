using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
	public static RestartGame instance;

	[SerializeField] private Button _restartButton;
	[SerializeField] private CanvasGroup _canvasGroup;
	
	public Action onGameReset;

    public void Initialize()
    {
        instance = this;
    }

    private void Start()
	{
		_restartButton.onClick.AddListener(HandleButtonClick);
	}
	
	public void HandleButtonClick()
	{
		_restartButton.interactable = false;
		onGameReset?.Invoke();
	}
}
