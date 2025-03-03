using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
	[SerializeField] private bool _isScoreTrigger;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && !_isScoreTrigger)
		{
			GameState.instance.SetGameOver();
		}
		else
		{
			Score.instance.AddScore(1);
		}
	}
}
