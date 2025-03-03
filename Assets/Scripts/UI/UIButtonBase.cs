using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public abstract class UIButtonBase : MonoBehaviour
{
    [SerializeField] protected Button _button;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] protected AudioClip _swooshSound;
    
    [Range(0, 1f)]
    [SerializeField] private float _fadeDuration = 0.2f;

    protected abstract void OnButtonClick();

    protected virtual void Start()
    {
        ShowButton();
        _button.onClick.AddListener(OnButtonClick);
    }

    protected void HideButton()
    {
        _buttonImage.DOFade(0f, _fadeDuration).SetEase(Ease.InOutCubic);
        _buttonText.DOFade(0f, _fadeDuration).SetEase(Ease.InOutCubic);
        _button.interactable = false;
    }

    protected void ShowButton()
    {
        _buttonImage.DOFade(1f, _fadeDuration).SetEase(Ease.InOutCubic);
        _buttonText.DOFade(1f, _fadeDuration).SetEase(Ease.InOutCubic);
        _button.interactable = true;
    }
}
