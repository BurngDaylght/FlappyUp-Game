using UnityEngine;

public class Pipe : MonoBehaviour
{
	[SerializeField] private GameObject _pipeUp;
	[SerializeField] private GameObject _pipeDown;
	[SerializeField] private float _speed;

    private void OnEnable()
    {
        GameState.instance.onGameOver += StopMoving;
    }

    private void Update()
	{
		transform.Translate(Vector2.left * Time.deltaTime * _speed);
		
		if (transform.position.x <= -5)
		{
			Destroy(gameObject);
		}
	}

	public void SetPassageDistance(float distance)
	{
		_pipeUp.transform.localPosition = new Vector2(0, distance / 2);
		_pipeDown.transform.localPosition = new Vector2(0, -distance / 2);
	}

	public void SetSpeed(float speed)
	{
		_speed = speed;
	}

	private void StopMoving()
	{
		_speed = 0;
    }
}
