using UnityEngine;

public class GameInitializationSettings : MonoBehaviour
{
    private void Start()
    {
    	Application.targetFrameRate = 144; 
        QualitySettings.vSyncCount = 0;
    }
}
