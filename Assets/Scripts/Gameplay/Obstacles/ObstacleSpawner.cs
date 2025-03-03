using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner instance;

    [Header("Common Settings")]
    [SerializeField] private Sprite _pipeSkinSprite;
    [SerializeField] private Transform _spawnPoint;
    [Range(0f, 1f)]
    [SerializeField] private float _eventChance;

    [Header("Difficulty Settings")]
    [SerializeField] private List<int> _scoreThresholds;
    [SerializeField] private List<DifficultySettingConfig> _difficultyConfigs;

    [Header("Pipe Settings")]
    private float _timer;
    private float _spawnTime;
    [SerializeField] private float _eventSpawnTime;

    private float _passageDistance;
    public float PassageDistance => _passageDistance;
    private float _speed;
    private float _smoothness;
    private GameObject _pipePrefab;

    [Header("Spawn Control")]
    private float _lastYPosition;
    [SerializeField] private bool _canSpawn;
    [SerializeField] private bool _eventActive;

    [Header("Event Configs")]
    [SerializeField] private List<DifficultySettingConfig> _eventConfigs;
    private List<int> _eventWeights;

    private void OnEnable()
    {
        GameState.instance.onGameOver += StopSpawning;
        GameState.instance.onGameStart += StartSpawning;
        Score.instance.onAddScore += CheckScore;
    }

    private void Awake()
    {
        instance = this;
        _eventWeights = new List<int>();

        if (_eventConfigs.Count > 0)
        {
            foreach (var config in _eventConfigs)
            {
                _eventWeights.Add(config.eventWeight);
            }
        }
    }

    private void Update()
    {
        if (_canSpawn && !_eventActive)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                SpawnObstacle();
                _timer = _spawnTime;
            }
        }
    }

    private IEnumerator DelayBeforeEvent()
    {
        yield return new WaitForSeconds(_eventSpawnTime);
        TrySpawnEvent();
    }

    private void SpawnObstacle()
    {
        float targetY = Random.Range(-2f, 3f);
        float yPosition = Mathf.Lerp(_lastYPosition, targetY, _smoothness);
        _lastYPosition = yPosition;

        Vector3 spawnPosition = new Vector3(_spawnPoint.position.x, yPosition, _spawnPoint.position.z);
        GameObject pipe = Instantiate(_pipePrefab, spawnPosition, _spawnPoint.rotation);

        pipe.GetComponent<PipeSkin>().SetSkin(_pipeSkinSprite);
        Pipe pipeComponent = pipe.GetComponent<Pipe>();
        pipeComponent.SetPassageDistance(_passageDistance);
        pipeComponent.SetSpeed(_speed);

        if (Random.value < _eventChance && _eventConfigs.Count > 0 && !_eventActive)
        {
            _canSpawn = false;
            StartCoroutine(DelayBeforeEvent());
        }
    }

    private void SpawnObstacle(DifficultySettingConfig config)
    {
        float targetY = Random.Range(-2f, 3f);
        float yPosition = Mathf.Lerp(_lastYPosition, targetY, config.smoothness);
        _lastYPosition = yPosition;

        Vector3 spawnPosition = new Vector3(_spawnPoint.position.x, yPosition, _spawnPoint.position.z);
        GameObject pipe = Instantiate(config.pipePrefab, spawnPosition, _spawnPoint.rotation);

        pipe.GetComponent<PipeSkin>().SetSkin(_pipeSkinSprite);
        Pipe pipeComponent = pipe.GetComponent<Pipe>();
        pipeComponent.SetPassageDistance(config.passageDistance);
        pipeComponent.SetSpeed(config.speed);
    }

    private void StartSpawning()
    {
        CheckScore();
        _canSpawn = true;
    }

    private void StopSpawning()
    {
        _canSpawn = false;
    }

    public void SetSkinSprite(Sprite sprite)
    {
        _pipeSkinSprite = sprite;
    }

    private void CheckScore()
    {
        int currentScore = Score.instance.GetScore();

        for (int i = 0; i < _scoreThresholds.Count; i++)
        {
            if (currentScore == _scoreThresholds[i])
            {
                ApplyDifficultySettings(_difficultyConfigs[i]);
            }
        }
    }

    private void ApplyDifficultySettings(DifficultySettingConfig config)
    {
        _pipePrefab = config.pipePrefab;
        _passageDistance = config.passageDistance;
        _speed = config.speed;
        _spawnTime = config.spawnTime;
        _smoothness = config.smoothness;
    }

    private void TrySpawnEvent()
    {
        _canSpawn = false;
        _eventActive = true;

        DifficultySettingConfig eventConfig = GetRandomEvent();
        StartCoroutine(SpawnEvent(eventConfig));
    }

    private DifficultySettingConfig GetRandomEvent()
    {
        int totalWeight = 0;
        foreach (int weight in _eventWeights)
        {
            totalWeight += weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentSum = 0;

        for (int i = 0; i < _eventWeights.Count; i++)
        {
            currentSum += _eventWeights[i];
            if (randomValue < currentSum)
            {
                return _eventConfigs[i];
            }
        }

        return _eventConfigs[0];
    }

    private IEnumerator SpawnEvent(DifficultySettingConfig eventConfig)
    {
        for (int i = 0; i < eventConfig.pipesInEvent; i++)
        {
            SpawnObstacle(eventConfig);
            yield return new WaitForSeconds(eventConfig.spawnTime);
        }

        _canSpawn = true;
        _eventActive = false;
    }
}
