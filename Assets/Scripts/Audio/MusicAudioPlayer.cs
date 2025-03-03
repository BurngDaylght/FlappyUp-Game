using UnityEngine;
using System.Collections.Generic;

public class MusicAudioPlayer : MonoBehaviour
{
    public static MusicAudioPlayer instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _musicTracks;
    [SerializeField] private bool _playOnStart = true;

    private int _currentTrackIndex = 0;

    private void OnEnable()
    {
        GameState.instance.onGameOver += StopMusic;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (_playOnStart && _musicTracks.Count > 0)
        {
            PlayMusic(-1);
        }
    }

    private void Update()
    {
        if (!_audioSource.isPlaying && Settings.instance.isMusicEnabled)
        {
            NextTrack();
        }
    }

    public void PlayMusic(int trackIndex = -1)
    {
        if (_musicTracks.Count == 0) return;

        if (trackIndex == -1)
        {
            trackIndex = Random.Range(0, _musicTracks.Count);
        }

        if (trackIndex >= 0 && trackIndex < _musicTracks.Count)
        {
            _currentTrackIndex = trackIndex;
            _audioSource.clip = _musicTracks[_currentTrackIndex];
            _audioSource.Play();
        }
    }

    public void NextTrack()
    {
        _currentTrackIndex = (_currentTrackIndex + 1) % _musicTracks.Count;
        PlayMusic(_currentTrackIndex);
    }

    public void ToggleMusic(bool isEnabled)
    {
        _audioSource.mute = !isEnabled;
        Settings.instance.isMusicEnabled = isEnabled;
    }

    public void StopMusic()
    {
        Settings.instance.isMusicEnabled = false;
        _audioSource.Stop();
        _audioSource.mute = true;
    }
}
