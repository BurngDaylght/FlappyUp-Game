using UnityEngine;
using DG.Tweening;
using System;

public class ScoreCoinTransition : MonoBehaviour
{
	[SerializeField] private RectTransform _scoreContainer; 
	[SerializeField] private RectTransform _coinContainer; 
	
	private Vector3 _scoreContainerStartScale; 
	private Vector3 _coinContainerStartScale; 
	
	[Range(0, 1f)]
	[SerializeField] private float _transitionDuration; 
	
	[Range(0, 1f)]
	[SerializeField] private float _coinContainerScale; 
	
	[Range(0, 1f)]
	[SerializeField] private float _scaleContainerScale; 
	
	private bool _isSwapped = false;
	
	private void OnEnable()
	{
		Shop.instance.onShopOpen += SwapCoinScore;
		Shop.instance.onShopClose += SwapCoinScore;
	}

	private void Start()
	{
		_scoreContainerStartScale = _scoreContainer.localScale;
		_coinContainerStartScale = _coinContainer.localScale;
	}

	public void SwapCoinScore()
	{
		Vector2 scorePos = _scoreContainer.anchoredPosition;
		Vector2 coinPos = _coinContainer.anchoredPosition;
	
		_scoreContainer.DOAnchorPos(coinPos, _transitionDuration).SetEase(Ease.InOutCubic);
		_coinContainer.DOAnchorPos(scorePos, _transitionDuration).SetEase(Ease.InOutCubic);
		
		if (!_isSwapped)
		{
			_scoreContainer.DOScale(_scaleContainerScale, _transitionDuration).SetEase(Ease.InOutCubic);
			_coinContainer.DOScale(_coinContainerScale, _transitionDuration).SetEase(Ease.InOutCubic);
		}
		else 
		{
			_scoreContainer.DOScale(_scoreContainerStartScale, _transitionDuration).SetEase(Ease.InOutCubic);
			_coinContainer.DOScale(_coinContainerStartScale, _transitionDuration).SetEase(Ease.InOutCubic);
		}
		
		_isSwapped = !_isSwapped;
	}
}
