using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameTitle : MonoBehaviour
{
    public static GameTitle instance;

    [SerializeField] private TextMeshProUGUI _titleText;

    [Range(0f, 1f)]
    [SerializeField] private float _duration;
    [SerializeField] private float _jumpPower = 30f;
    [SerializeField] private int _jumpCount = 1;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
       GameState.instance.onGameStart += HideAnimation;
    }

    private void Start()
    {
        _titleText.text = "FLAPPY UP";
        ShowAnimation();
    }

    public void SetText(string newText = "FLAPPY UP")
    {
        Sequence textSequence = DOTween.Sequence();

        textSequence.Append(_titleText.DOFade(0f, _duration)).SetEase(Ease.InOutCubic);
        textSequence.Join(_titleText.transform.DOScale(0.8f, _duration)).SetEase(Ease.InOutCubic);

        textSequence.AppendCallback(() => _titleText.text = newText);

        textSequence.Append(_titleText.DOFade(1f, _duration)).SetEase(Ease.InOutCubic);
        textSequence.Join(_titleText.transform.DOJump(_titleText.transform.position, _jumpPower, _jumpCount, _duration)).SetEase(Ease.InOutCubic);
        textSequence.Join(_titleText.transform.DOScale(1f, _duration)).SetEase(Ease.InOutCubic);
    }

    public void HideAnimation()
    {
        _titleText.DOFade(0f, 0.5f);
    }

    public void ShowAnimation()
    {
        _titleText.DOFade(1f, 0.5f);
    }
}
