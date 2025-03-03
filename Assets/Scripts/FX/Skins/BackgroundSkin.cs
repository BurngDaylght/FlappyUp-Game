using UnityEngine;

public class BackgroundSkin : EnvironmentSkin
{
    public static BackgroundSkin instance;

    private void Awake()
    {
        instance = this;
    }
}
