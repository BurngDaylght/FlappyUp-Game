using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class StartGame : MonoBehaviour
{
	public static StartGame instance;

	[SerializeField] private Button _startButton;
	[SerializeField] private TextMeshProUGUI _startButtonText;
	
	[Range(0, 50)]
	[SerializeField] private float _moveDistance = 10f; 
	[Range(0, 5)]
	[SerializeField] private float _moveDuration = 0.5f;
	[Range(0, 5)]
	[SerializeField] private float _fadeDuration = 0.5f;
	[Range(0, 1)]
	[SerializeField] private float _fadeMinAlpha = 0.5f;
	[Range(0, 5)]
	[SerializeField] private float _hideDuration = 0.5f;
	
	
	private RectTransform _buttonRect;

    private void OnEnable()
    {
		Shop.instance.onShopOpen += OffButton;
		Shop.instance.onShopClose += OnButton;

		Settings.instance.onSettingsOpen += OffButton;
		Settings.instance.onSettingsClose += OnButton;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start() 
	{
		_buttonRect = _startButton.GetComponent<RectTransform>();
		_startButton.onClick.AddListener(BeginGame);
		ShowButton();
	}
	
	private void BeginGame()
	{
		_startButton.enabled = false;
		GameState.instance.SetStartGame();
		HideButton();
	}
	
	private void ShowButton()
	{
		SetButtonInteractable(true);
		
		_buttonRect.DOAnchorPosY(_buttonRect.anchoredPosition.y + _moveDistance, _moveDuration)
			.SetLoops(-1, LoopType.Yoyo)
			.SetEase(Ease.InOutSine);

		var textColor = _startButtonText.color;
		_startButtonText.DOColor(new Color(textColor.r, textColor.g, textColor.b, _fadeMinAlpha), _fadeDuration)
			.SetLoops(-1, LoopType.Yoyo)
			.SetEase(Ease.InOutSine);
	}
	
	private void HideButton()
	{
		Sequence hideSequence = DOTween.Sequence();

		hideSequence.Append(_buttonRect.DOAnchorPosY(_buttonRect.anchoredPosition.y + _moveDistance + 100f, _hideDuration / 2)
			.SetEase(Ease.OutSine));

		hideSequence.Append(_buttonRect.DOAnchorPosY(_buttonRect.anchoredPosition.y - 1000f, _hideDuration)
			.SetEase(Ease.InSine));

		hideSequence.OnComplete(() => _startButton.gameObject.SetActive(false));
	}

	private void OnButton()
	{
	    _startButton.interactable = true;
	}

	private void OffButton()
	{
	    _startButton.interactable = false;
	}

	public void SetButtonInteractable(bool boolean) 
	{
	    _startButton.interactable = boolean;
	}
}
