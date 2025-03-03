using UnityEngine;
using System;

public class Bird : MonoBehaviour
{
	public static Bird instance;
	
    [Header("Flight Settings")]
    [Range(0, 1000)]
    [SerializeField] private float _forcePower;
    [Range(0, 50)]
    [SerializeField] private float _gameOverForcePower;
    [SerializeField] private Vector2 _forceDirection;

    [Header("Rotation Settings")]
    [SerializeField] private float _speedRotateUp;
    [SerializeField] private float _cornerLimitUp;
    [SerializeField] private float _cornerLimitDown;

    [Header("Game State")]
    [SerializeField] private bool _isCanFlying = true;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _flapSound;
    [Range(0f, 1f)]
    [SerializeField] private float _flapSoundVolume;

    [SerializeField] private AudioClip _dieSound;
    [Range(0f, 1f)]
    [SerializeField] private float _dieSoundVolume;

    [SerializeField] private AudioClip _fallSound;
    [Range(0f, 1f)]
    [SerializeField] private float _fallSoundVolume;

	private Rigidbody2D _rigidbody2D;
	private CircleCollider2D _circleCollider2;

	private void Awake()
	{
		instance = this;

		_rigidbody2D = GetComponent<Rigidbody2D>();
		_circleCollider2 = GetComponent<CircleCollider2D>();
	}
	
	private void Start()
	{
		DisablePhysics();
	}

	private void OnEnable()
	{
		GameState.instance.onGameStart += EnablePhysics;
		GameState.instance.onGameStart += Flap;
		
		GameState.instance.onGameOver += StopFlying;
	}
	
	private void Update()
	{
		if (GameState.instance.isGameStart)
		{
			HandleFlying();
		}
	}
	
	private void HandleFlying()
	{
		if (transform.position.y >= 6 && _isCanFlying)
		{
			GameState.instance.SetGameOver();
		}
		else if (Input.GetMouseButtonDown(0) && _isCanFlying)
		{
			Flap();
		}

		float angle = Mathf.Lerp(_cornerLimitDown, _cornerLimitUp, Mathf.InverseLerp(-10, 10, _rigidbody2D.linearVelocity.y));
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), _speedRotateUp * Time.deltaTime);
	}
	
	private void Flap()
	{
		_rigidbody2D.linearVelocity = Vector2.zero;
		_rigidbody2D.AddForce(_forceDirection.normalized * _forcePower, ForceMode2D.Impulse);

		SFXAudioPlayer.instance.PlaySFX(_flapSound, _flapSoundVolume);
	}
	
	private void DisablePhysics()
	{
		_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
	}
	
	private void EnablePhysics()
	{
		_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
	}

	private void StopFlying()
	{
		_isCanFlying = false;
		_circleCollider2.enabled = false;

		_rigidbody2D.linearVelocity = Vector2.zero;
		_rigidbody2D.AddForce(_forceDirection.normalized * _gameOverForcePower, ForceMode2D.Impulse);

		PlayGameOverSFX();
	}

	private void PlayGameOverSFX()
	{
	    SFXAudioPlayer.instance.PlaySFX(_dieSound, _dieSoundVolume);
		SFXAudioPlayer.instance.PlaySFX(_fallSound, _fallSoundVolume);
	}
}
