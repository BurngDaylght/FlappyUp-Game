using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
	public event Action onGameStart;
	public event Action onGameOver;

	public bool isGameStart;
	public bool isGameOver;

	public static GameState instance;   

	public void Initialize()
	{
		instance = this;
	}
	
	public void SetStartGame()
	{
		onGameStart?.Invoke();
		isGameStart = true;

		Debug.Log("Game Start!");
	}

	public void SetGameOver()
	{
		onGameOver?.Invoke();
		isGameOver = true;

		Debug.Log("Game Over!");
	}
}
