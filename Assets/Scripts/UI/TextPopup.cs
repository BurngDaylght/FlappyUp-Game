using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextPopup : MonoBehaviour
{    
	[SerializeField] private TextMeshProUGUI _textMeshPro;
	[SerializeField] private float _scaleMultiplier = 1f;
	[SerializeField] private Vector2 _offset;

	[Header("Animation Parameters")]
	[Header("Moving")]
	[Range(0, 200f)]
	[SerializeField] private float _moveDistanceY;
	[Range(0, 1f)]
	[SerializeField] private float _moveDuration;

	[Header("Fade")]
	[Range(0, 1f)]
	[SerializeField] private float _fadeDuration;

	private RectTransform _rectTransform;

	private void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
	}

	public void Setup(string text)
	{
		transform.localScale *= _scaleMultiplier;
		
		_textMeshPro.text = text;

		_rectTransform.anchoredPosition += _offset;

		PlayAnimation();
	}

	private void PlayAnimation()
	{
		_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + _moveDistanceY, _moveDuration).SetEase(Ease.OutCubic);

		_textMeshPro.DOFade(0f, _fadeDuration).OnComplete(() =>
		{
			Destroy(gameObject);
		});
	}
}
