using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WhiteoutScreen : MonoBehaviour
{
	[SerializeField] private RawImage _transitionScreen;
	[SerializeField] private CanvasGroup _canvasGroup;
	
	[Range(0, 3)]
	[SerializeField] private float _fadeDuration;
	
	[Range(0, 1)]
	[SerializeField] private float _canvasAlpha;
	
	private void OnEnable()
	{
		GameState.instance.onGameOver += WhiteOutFadeAnimation;
	}
	
	private void Start()
	{
		_transitionScreen.enabled = true;
		_canvasGroup.alpha = 0f;
	}
	
	private void WhiteOutFadeAnimation()
	{
		_canvasGroup.alpha = _canvasAlpha;
		_canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InOutCubic);
	}
}
