using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPresenter : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem _hitParticle;

	[SerializeField]
	private SpriteRenderer _sprite;
	public IAsteroidController Controller { get; private set; }
	private bool _isAlive = true;
	private Transform _transform;


	public static AsteroidPresenter Create(IAsteroidController controller, GameObject prefab, Transform place, Transform parent)
	{
		var presenterGo = ObjectPoolManager.GetObject(prefab.name);
		presenterGo.transform.position = place.position;
		presenterGo.transform.rotation = place.rotation;

		var presenter = presenterGo.GetComponent<AsteroidPresenter>();
		presenter.transform.SetParent(parent);

		presenter.Init(controller);

		return presenter;
	}

	public void Init(IAsteroidController controller)
	{
		Controller = controller;
		_transform = transform;
		UnSubscribeEvents();
		SubscribeEvents();
	}

	public void StartMoving()
	{
		StartCoroutine(StartAnimation());
	}

	private void SubscribeEvents()
	{
		Controller.OnDeathEvent += OnAsteroidDeath;
		Controller.OnDestroyEvent += OnAsteroidDestroyed;
	}

	private void UnSubscribeEvents()
	{
		if (Controller == null)
			return;

		Controller.OnDeathEvent -= OnAsteroidDeath;
		Controller.OnDestroyEvent -= OnAsteroidDestroyed;
	}

	private void OnAsteroidDestroyed(IAsteroidController asteroid)
	{
		_isAlive = false;
		_hitParticle.Play();
		_sprite.enabled = false;
		Destroy(gameObject, _hitParticle.main.duration);
	}

	private void OnAsteroidDeath(IAsteroidController asteroid)
	{
		_isAlive = false;
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Controller.OnCollision(other);
		//Debug.LogFormat("[AsteroidPresenter] OnTriggerEnter2D - OK, collide with : {0}", other.gameObject.name);
	}

	private void OnCollisionEnter2d(Collision2D collision)
	{
		//Debug.LogFormat("[AsteroidPresenter] OnCollisionEnter2d - OK, collide with : {0}", collision.gameObject.name);
	}

	// Use this for initialization
	void Start () {
		_sprite.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator StartAnimation()
	{
		Vector3 moveVector = Controller.MovableObject.Direction.Value * Controller.MovableObject.Speed.Value;
		Vector3 rotateVector = Vector3.zero;
		float rotationSpeed = Controller.MovableObject.RotationSpeed.Value;
		bool needRotate = Controller.MovableObject.Speed.Value != 0f;
		float angleCounter = 0;
		while (_isAlive)
		{
			if (Controller.IsPause.Value)
			{
				yield return null;
			}
			else
			{
				_transform.position += moveVector;
				if (needRotate)
				{
					angleCounter += rotationSpeed * Time.deltaTime;

					if (angleCounter >= 360)
						angleCounter -= 360;

					rotateVector.z = angleCounter;
					Quaternion rot = Quaternion.Euler(rotateVector);
					_transform.localRotation = rot;
				}

				Controller.UpdateLifeTime(Time.deltaTime);

				yield return null;
			}
		}
	}
}
