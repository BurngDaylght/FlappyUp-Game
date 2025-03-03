using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
	[SerializeField] private int _coinAmount;
	
	[Header("Animatio Parameter")]
	[Header("Moving")]
	[Range(0, 5f)]
	[SerializeField] private float _moveDistanceY;
	[Range(0, 1f)]
	[SerializeField] private float _moveDuration;
	
	[Header("Fade")]
	[Range(0, 1f)]
	[SerializeField] private float _fadeDuration;
	
	[Header("Scale")]
	[Range(0, 4f)]
	[SerializeField] private float _scaleAmount;
	[Range(0, 1f)]
	[SerializeField] private float _scaleDuration;
	
	[SerializeField] private SpriteRenderer _spriteRenderer;
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			CoinWallet.instance.AddCoins(_coinAmount);
			DestroyAnimation();
		}
	}
	
	private void DestroyAnimation()
	{
		transform.DOMoveY(transform.position.y + _moveDistanceY, _moveDuration).SetEase(Ease.OutCubic);
		transform.DOScale(_scaleAmount, _scaleDuration).SetEase(Ease.InOutSine);
		_spriteRenderer.DOColor(new Color(1f, 1f, 1f, 0f), _fadeDuration).OnComplete(() =>
		{
			transform.DOKill();
			_spriteRenderer.DOKill();
	
			Destroy(gameObject);
		});
	}
}
