using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.IO;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    [Header("UI Settings")]
    [Range(0, 1f)]
    [SerializeField] private float _transitionDuration;

    [Header("Buttons")]
    [SerializeField] private Button _sfxToggleButton;
    [SerializeField] private Button _musicToggleButton;
    
    [Header("Images")]
    [SerializeField] private Image _sfxButtonImage;
    [SerializeField] private Image _musicButtonImage;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _sfxText;
    [SerializeField] private TextMeshProUGUI _musicText;
    
    [Header("Sprites")]
    [SerializeField] private Sprite _sfxOnSprite;
    [SerializeField] private Sprite _sfxOffSprite;
    [SerializeField] private Sprite _musicOnSprite;
    [SerializeField] private Sprite _musicOffSprite;

    public bool isSfxEnabled = true;
    public bool isMusicEnabled = true;

    public Action onSettingsOpen;
    public Action onSettingsClose;

    private void OnEnable()
    {
        SaveLoadHandler.instance.onGameLoad += LoadData;
        SaveLoadHandler.instance.onGameSave += SaveData;
    }

    public void Initialize()
    {
        instance = this;
    }

    private void Start()
    {
        transform.DOLocalMove(new Vector3(0, -2500f, 0), 0f);
        _sfxToggleButton.onClick.AddListener(ToggleSFX);
        _musicToggleButton.onClick.AddListener(ToggleMusic);
        UpdateUI();
    }

    public void OpenSettings()
    {
        transform.DOLocalMove(Vector3.zero, _transitionDuration).SetEase(Ease.InOutCubic);
        onSettingsOpen?.Invoke();
        GameTitle.instance.SetText("SETTINGS");
    }

    public void CloseSettings()
    {
        transform.DOLocalMove(new Vector3(0, -2500f, 0), _transitionDuration).SetEase(Ease.InOutCubic);
        onSettingsClose?.Invoke();
        GameTitle.instance.SetText();

        SaveData();
    }

    private void ToggleSFX()
    {
        isSfxEnabled = !isSfxEnabled;
        UpdateUI();
    }

    private void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;
        MusicAudioPlayer.instance.ToggleMusic(isMusicEnabled);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _sfxButtonImage.sprite = isSfxEnabled ? _sfxOnSprite : _sfxOffSprite;
        _musicButtonImage.sprite = isMusicEnabled ? _musicOnSprite : _musicOffSprite;
        _sfxText.text = isSfxEnabled ? "Sounds On" : "Sounds Off";
        _musicText.text = isMusicEnabled ? "Music On" : "Music Off";
    }
    
    public void SaveData()
    {
        IDataService dataService = new JsonDataService();

        SettingsData settingsData = new SettingsData(isSfxEnabled, isMusicEnabled);
        dataService.SaveData($"{Application.persistentDataPath}/Settings.json", settingsData, false);
    }

    public void LoadData()
    {
        IDataService dataService = new JsonDataService();
        string filePath = $"{Application.persistentDataPath}/Settings.json";

        if (File.Exists(filePath))
        {
            SettingsData settingsData = dataService.LoadData<SettingsData>(filePath, false);
            
            isSfxEnabled = settingsData.isSfxEnabled;
            isMusicEnabled = settingsData.isMusicEnabled;
            MusicAudioPlayer.instance.ToggleMusic(isMusicEnabled);
        }
        else
        {
            SaveData();
        }
        
        UpdateUI();
    }
}

[Serializable]
public class SettingsData
{
    public bool isSfxEnabled;
    public bool isMusicEnabled;

    public SettingsData(bool sfxEnabled, bool musicEnabled)
    {
        isSfxEnabled = sfxEnabled;
        isMusicEnabled = musicEnabled;
    }
}
