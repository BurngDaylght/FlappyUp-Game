using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlackoutScreen : MonoBehaviour
{
	[SerializeField] private RawImage _transitionScreen;
	[SerializeField] private CanvasGroup _canvasGroup;
	
	[Range(0, 3)]
	[SerializeField] private float _fadeDuration;
	
	[Range(0, 3)]
	[SerializeField] private float _appearDuration;
	
	public void Initialize()
	{
		RestartGame.instance.onGameReset += AppearAnimation;
	}
	
	private void Start()
	{
		_transitionScreen.enabled = true;
		_canvasGroup.alpha = 0f;
		
		FadeAnimation();
	}
	
	private void FadeAnimation()
	{
		_canvasGroup.alpha = 1f;
		_canvasGroup.DOFade(0f, _fadeDuration).SetEase(Ease.InOutCubic);
	}
	
	private void AppearAnimation()
	{
		_canvasGroup.alpha = 0f;
		_canvasGroup.DOFade(1f, _appearDuration).SetEase(Ease.InOutCubic)
			.OnComplete(() => 
			{
				SceneLoader.instance.RestartScene();
			});
	}
}
