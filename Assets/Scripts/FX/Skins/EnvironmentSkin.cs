using UnityEngine;

public abstract class EnvironmentSkin : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _spriteRendered;

    public void SetSkin(Sprite spriteSkin)
    {
        foreach (var renderer in _spriteRendered)
        {
            renderer.sprite = spriteSkin;
        }
    }
}