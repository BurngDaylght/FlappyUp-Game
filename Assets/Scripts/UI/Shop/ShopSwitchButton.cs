using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopSwitchButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _switchButton;
    [SerializeField] private TextMeshProUGUI _switchButtonText;
    [SerializeField] private GameObject _birdScrollView;
    [SerializeField] private GameObject _envirementScrollView;

    [Header("State Variables")]
    private bool _isBirdScrollActive = true;

    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration = 0.5f;
    [SerializeField] private float _moveDistance = 200f;

    [Header("Sound Effects")]
    [SerializeField] protected AudioClip _swooshSound;

    [Header("Scroll View Positions")]
    private Vector3 _birdScrollStartPos;
    private Vector3 _envScrollStartPos;

    private void Start()
    {
        _birdScrollStartPos = _birdScrollView.transform.localPosition;
        _envScrollStartPos = _envirementScrollView.transform.localPosition;

        _birdScrollView.SetActive(true);
        _envirementScrollView.SetActive(false);

        _switchButton.onClick.AddListener(SwitchScrollView);
    }

    private void SwitchScrollView()
    {
        if (_isBirdScrollActive)
        {
            AnimateSwitch(_birdScrollView, _envirementScrollView, _birdScrollStartPos, _envScrollStartPos, "Birds");
        }
        else
        {
            AnimateSwitch(_envirementScrollView, _birdScrollView, _envScrollStartPos, _birdScrollStartPos, "Themes");
        }
        _isBirdScrollActive = !_isBirdScrollActive;
        SFXAudioPlayer.instance.PlaySFX(_swooshSound);
    }

    private void AnimateSwitch(GameObject hideScrollView, GameObject showScrollView, Vector3 hideStartPos, Vector3 showStartPos, string newButtonText)
    {
        CanvasGroup hideCanvas = hideScrollView.GetComponent<CanvasGroup>();
        CanvasGroup showCanvas = showScrollView.GetComponent<CanvasGroup>();

        _switchButton.interactable = false;

        hideScrollView.transform.DOLocalMoveY(hideStartPos.y - _moveDistance, _animationDuration)
            .SetEase(Ease.InOutQuad);
        hideCanvas.DOFade(0, _animationDuration)
            .OnComplete(() =>
            {
                hideScrollView.SetActive(false);
                hideScrollView.transform.localPosition = hideStartPos;
            });

        showScrollView.SetActive(true);
        showScrollView.transform.localPosition = new Vector3(showStartPos.x, showStartPos.y + _moveDistance, showStartPos.z);
        showCanvas.alpha = 0;

        showScrollView.transform.DOLocalMoveY(showStartPos.y, _animationDuration)
            .SetEase(Ease.InOutQuad);
        showCanvas.DOFade(1, _animationDuration)
            .OnComplete(() =>
            {
                _switchButton.interactable = true;
            });

        _switchButtonText.text = newButtonText;
    }
}
