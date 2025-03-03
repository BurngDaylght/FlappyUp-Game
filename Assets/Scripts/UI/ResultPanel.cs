using UnityEngine;
using DG.Tweening;
using TMPro;

public class ResultPanel : MonoBehaviour
{
	[SerializeField] private GameObject _panel;
	[SerializeField] private CanvasGroup _canvasGroup;
	
	[Range(0, 1)]
	[SerializeField] private float _appearDuration;
	
	[SerializeField] private TextMeshProUGUI _scoreText;
	[SerializeField] private TextMeshProUGUI _bestScoreText;
	[SerializeField] private TextMeshProUGUI _newRecordText;
	[SerializeField] private TextMeshProUGUI _coinsEarnedText;
	
	public void Initialize()
	{
		GameState.instance.onGameOver += ShowPanel;
		Score.instance.onNewRecord += ShowNewRecordText;
	}
	
	private void Start()
	{
		HidePanel();
		HideNewRecordText();
	}
	
	private void HidePanel()
	{
		_canvasGroup.alpha = 0f;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
	}
	
	private void ShowPanel()
	{
		_canvasGroup.alpha = 1f;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
		
		SetScore();
		SetCoinsEarnedCount();
		
		RectTransform rectTransform = _panel.GetComponent<RectTransform>();
		Vector2 currentPosition = rectTransform.anchoredPosition;
		
		Sequence showSequence = DOTween.Sequence();
		
		showSequence.Append(rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y - 2000f, 0f));
		
		showSequence.Append(rectTransform.DOAnchorPosY(currentPosition.y, _appearDuration)
			.SetEase(Ease.InOutCubic));
	}
	
	private void SetScore()
	{
		int finalScore = Score.instance.GetScore();
		int previousBestScore = Score.instance.GetBestScore();
		int newBestScore = previousBestScore;

		if (finalScore > previousBestScore)
		{
			newBestScore = finalScore;
		}

		int displayedScore = 0;
		DOTween.To(() => displayedScore, x =>
		{
			displayedScore = x;
			_scoreText.text = displayedScore.ToString();
		}, finalScore, 3f)
		.SetEase(Ease.InOutCubic);

		if (finalScore > previousBestScore)
		{
			int displayedBestScore = previousBestScore;
			DOTween.To(() => displayedBestScore, x =>
			{
				displayedBestScore = x;
				_bestScoreText.text = displayedBestScore.ToString();
			}, newBestScore, 3f)
			.SetEase(Ease.InOutCubic);
		}
		else
		{
			_bestScoreText.text = previousBestScore.ToString();
		}
	}
	
	private void SetCoinsEarnedCount()
	{
		int coinsInSessionCount = CoinWallet.instance.coinsInSessionCount;
		
		float startValue = 0f;
		DOTween.To(() => startValue, x => 
		{
			startValue = x;
			_coinsEarnedText.text = $"+{Mathf.RoundToInt(startValue)}";
		}, coinsInSessionCount, 3f)
		.SetEase(Ease.InOutCubic);
	}
	
	private void HideNewRecordText()
	{
		_newRecordText.alpha = 0f;
	}
	
	private void ShowNewRecordText()
	{
		_newRecordText.alpha = 1f;
	}
}
