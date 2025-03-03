using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class Score : MonoBehaviour
{
	public static Score instance;

    [Header("Score Settings")]
    [SerializeField] private int _scoreCount;
    [SerializeField] private int _bestScoreCount;
    
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    
    [Header("Animation Settings")]
    [Range(0, 1)]
    [SerializeField] private float _animationDuration;
    
    [Range(0, 1)]
    [SerializeField] private float _fadeDuration;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioClip _scoreSound;
	
	public Action onNewRecord;
	public Action onAddScore;
	
	private void OnEnable()
	{
		GameState.instance.onGameOver += FadeAnimation;
		GameState.instance.onGameOver += CheckBestScore;
		GameState.instance.onGameOver += SaveData;
		
		SaveLoadHandler.instance.onGameLoad += LoadData;
	}
	
	public void Initialize()
	{
		instance = this;
	}
	
	private void Start()
	{
		_scoreText.text = "0";
	}
	
	public void AddScore(int score)
	{
		_scoreCount += score;
		CheckBestScore();
		AddScoreAnimation();

		onAddScore?.Invoke();

		SFXAudioPlayer.instance.PlaySFX(_scoreSound);
	}
	
	private void AddScoreAnimation()
	{
		float initialScale = _scoreText.transform.localScale.x;
		float midScale = initialScale * 1.2f;
		float shrinkScale = initialScale * 0.8f;

		_scoreText.transform.DOScale(shrinkScale, _animationDuration)
			.OnComplete(() =>
			{
				UpdateText();
				_scoreText.transform.DOScale(midScale, _animationDuration)
					.OnComplete(() => 
					{
						_scoreText.transform.DOScale(initialScale, _animationDuration);
					});
			});
	}
	
	private void FadeAnimation()
	{
		AddScoreAnimation();

		_scoreText.DOFade(0f, _fadeDuration);
	}
	
	private void UpdateText()
	{
		_scoreText.text = _scoreCount.ToString();
	}
	
	private void CheckBestScore()
	{
		if (_scoreCount > _bestScoreCount)
		{
			_bestScoreCount = _scoreCount;
			onNewRecord?.Invoke();
		}
	}
	
	public void SaveData()
	{
		IDataService dataService = new JsonDataService();
		
		ScoreData scoreData = new ScoreData(_bestScoreCount);
		dataService.SaveData($"{Application.persistentDataPath}/BestScore.json", scoreData, false);
	}
	
	public void LoadData()
	{
		IDataService dataService = new JsonDataService();

		ScoreData scoreData = dataService.LoadData<ScoreData>($"{Application.persistentDataPath}/BestScore.json", false);
		_bestScoreCount = scoreData.BestScoreCount;
	}

	
	public int GetScore()
	{
		return _scoreCount;
	}
	
	public int GetBestScore()
	{
		CheckBestScore();
		return _bestScoreCount;
	}
}

	
	
[Serializable]
public class ScoreData
{
	public int BestScoreCount;

	public ScoreData(int BestScoreCount)
	{
		this.BestScoreCount = BestScoreCount;
	}
}