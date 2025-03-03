using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ShopItemBase<T> : MonoBehaviour where T : ItemDataBase
{
    [Header("Item Data")]
    [SerializeField] public T itemData;

    [Header("UI Elements")]
    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected Image _image;

    [Header("Buy Button")]
    [SerializeField] protected Button _buyButton;
    [SerializeField] protected TextMeshProUGUI _buyButtonText;

    [Header("Set Button")]
    [SerializeField] protected Button _setButton;
    [SerializeField] protected TextMeshProUGUI _setButtonText;
    [SerializeField] protected Image _setButtonImage;
    [SerializeField] protected Sprite _setButtonUseSprite;
    [SerializeField] protected Sprite _setButtonUsedSprite;

    [Header("Audio Clips")]
    [SerializeField] protected AudioClip _purshaseSound;
    [SerializeField] protected AudioClip _setSound;

    public bool isPurchased;
    public bool isEquipped;

    protected virtual void Start()
    {
        _name.text = itemData.skinName.ToString();
        _image.sprite = itemData.skinImage;
        _buyButton.onClick.AddListener(BuyItem);
        _setButton.onClick.AddListener(SetItem);
        UpdateButtons();
    }

    protected void BuyItem()
    {
        int currentCoins = CoinWallet.instance.GetCoins();
        
        if (isPurchased || currentCoins < itemData.skinPrice)
        {
            CoinWallet.instance.NotEnoughMoneyAnimation();
            return;
        }

        CoinWallet.instance.RemoveCoins(itemData.skinPrice);
        SFXAudioPlayer.instance.PlaySFX(_purshaseSound);
        isPurchased = true;
        Skins.instance.SaveData();
        UpdateButtons();
    }

    protected abstract void SetItem();

    public virtual void UpdateButtons()
    {
        _buyButton.interactable = !isPurchased; 
        _setButton.interactable = isPurchased && !isEquipped;

        if (isPurchased)
        {
            _buyButtonText.text = "BOUGHT";
        }
        else 
        {
            _buyButtonText.text = $"{itemData.skinPrice} C";
        }

        if (!isPurchased)
        {
            _setButtonImage.sprite = _setButtonUseSprite;
            _setButtonText.text = "BUY NOW";
        }
        else if (isEquipped)
        {
            _setButtonImage.sprite = _setButtonUsedSprite;
            _setButtonText.text = "USED";
        }
        else 
        {
            _setButtonImage.sprite = _setButtonUseSprite;
            _setButtonText.text = "USE";
        }
    }
}
