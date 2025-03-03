using UnityEngine;
using DG.Tweening;
using System;

public class Shop : MonoBehaviour
{
	[Range(0, 1f)]
	[SerializeField] private float _transitionDuration;
	
	public event Action onShopOpen;
	public event Action onShopClose;

	public static Shop instance;

	public void Initialize()
	{
		instance = this;
	}

	private void Start()
	{
		transform.DOLocalMove(new Vector3(0, -2500f, 0), 0f);
	}
	
	public void OpenShop()
	{
		transform.DOLocalMove(Vector3.zero, _transitionDuration).SetEase(Ease.InOutCubic);
		onShopOpen?.Invoke();
		GameTitle.instance.SetText("SKINS");
	}
	
	public void CloseShop()
	{
		transform.DOLocalMove(new Vector3(0, -2500f, 0), _transitionDuration).SetEase(Ease.InOutCubic);
		onShopClose?.Invoke();
		GameTitle.instance.SetText();
	}
}
