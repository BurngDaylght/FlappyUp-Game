using UnityEngine;

public class GroundSkin : EnvironmentSkin
{
    public static GroundSkin instance;
    
    private void Awake()
    {
        instance = this;
    }
}
