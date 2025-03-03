using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Pipe Reference")]
    [SerializeField] private Pipe _pipe;
    
    [Header("Coin Settings")]
    [SerializeField] private GameObject[] _coinPrefabs;
    [SerializeField] private int[] _spawnWeights;
    
    [Header("Spawn Offsets")]
    [SerializeField] private Vector2 _centerOffset;
    [SerializeField] private Vector2 _topOffset;
    [SerializeField] private Vector2 _bottomOffset;

    private void Start()
    {
        _pipe = GetComponent<Pipe>();

        if (_coinPrefabs.Length != _spawnWeights.Length)
        {
            Debug.Log("Mistake! The number of coins does not match the number of scales!");
            return;
        }

        SpawnCoin();
    }

    private void SpawnCoin()
    {
        GameObject coinPrefab = GetRandomCoin();
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity, transform);
    }

    private GameObject GetRandomCoin()
    {
        int totalWeight = 0;
        foreach (int weight in _spawnWeights)
        {
            totalWeight += weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int currentSum = 0;

        for (int i = 0; i < _spawnWeights.Length; i++)
        {
            currentSum += _spawnWeights[i];
            if (randomValue < currentSum)
            {
                return _coinPrefabs[i];
            }
        }

        return _coinPrefabs[0];
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float passage = ObstacleSpawner.instance.PassageDistance;
        float pipeY = transform.position.y;

        Vector3[] spawnPositions = new Vector3[]
        {
            new Vector3(transform.position.x + _centerOffset.x, pipeY + _centerOffset.y, transform.position.z), // Центр
            new Vector3(transform.position.x + _topOffset.x, pipeY + passage / 2 - 0.5f + _topOffset.y, transform.position.z), // Верх
            new Vector3(transform.position.x + _bottomOffset.x, pipeY - passage / 2 + 0.5f + _bottomOffset.y, transform.position.z)  // Низ
        };

        return spawnPositions[Random.Range(0, spawnPositions.Length)];
    }
}