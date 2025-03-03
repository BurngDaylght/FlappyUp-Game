using UnityEngine;

public class SkinAnimator : MonoBehaviour
{
	private Animator _animator;
	
	public static SkinAnimator instance;

	private void Awake()
	{
		instance = this;
		
		_animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		GameState.instance.onGameOver += PlayFallAnimation;
	}

	private void PlayFallAnimation()
	{
		_animator.SetBool("isFalling", true);
	}
	
	public void SetSkin(RuntimeAnimatorController animator)
	{
		_animator.runtimeAnimatorController = animator;
	}
}
