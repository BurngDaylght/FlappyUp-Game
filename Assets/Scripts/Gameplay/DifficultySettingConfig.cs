using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty Setting/Difficulty Setting Config", fileName = "NewDifficultySettingConfig")]
public class DifficultySettingConfig : ScriptableObject
{
    [Header("Pipe Prefab")]
    public GameObject pipePrefab;

    [Header("Pipe Settings")]
    [Range(8f, 13f)]
    public float passageDistance;
    
    [Range(0f, 5f)]
    public float speed;
    public float spawnTime;
    public float smoothness;

    [Header("Event Settings")]
    public bool isEvent;
    public int pipesInEvent;
    public int eventWeight;
}
