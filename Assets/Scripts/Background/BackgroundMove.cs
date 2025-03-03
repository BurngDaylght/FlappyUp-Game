using UnityEngine;

public class BackgroundMove : MonoBehaviour
{	
	[SerializeField] private float _speed;
	private Vector3 _startPos;
	private float _repeatWidth;

    private void OnEnable()
    {
        GameState.instance.onGameOver += StopMoving;
    }

    private void Start()
	{
		_startPos = transform.position;
		_repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;
	}

	private void Update()
	{
		Repeat();
		MoveLeft();
	}
	
	private void MoveLeft()
	{
		transform.Translate(Vector2.left * Time.deltaTime * _speed);
	}
	
	private void Repeat()
	{
		if (transform.position.x < _startPos.x - _repeatWidth)
		{
			transform.position = _startPos;
		}
	}

    private void StopMoving()
    {
		_speed = 0;
    }
}
