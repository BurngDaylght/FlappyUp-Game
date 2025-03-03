using UnityEngine;
using System;

public class SaveLoadHandler : MonoBehaviour
{
	public Action onGameSave;
	public Action onGameLoad;
	
	public static SaveLoadHandler instance;

    public void Initialize()
    {
        instance = this;
    }

    public void LoadData()
	{	
		onGameLoad?.Invoke();
	}
	
	private void OnApplicationPause(bool isPaused)
	{
		if (isPaused)
		{
			onGameSave?.Invoke();
		}
	}
	
	private void OnApplicationQuit()
	{
		onGameSave?.Invoke();
	}
}
