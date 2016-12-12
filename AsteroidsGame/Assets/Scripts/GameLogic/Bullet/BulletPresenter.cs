using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPresenter : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem _bullet;

	[SerializeField]
	private ParticleSystem _collision;

	public IBulletController Controller { get; private set; }
	private bool _isAlive = true;
	private Transform _transform;

	public static BulletPresenter Create(IBulletController controller, GameObject prefab, Transform place, Transform parent)
	{
		var presenterGo = ObjectPoolManager.GetObject(prefab.name);
		presenterGo.transform.position = place.position;
		presenterGo.transform.rotation = place.rotation;

		var presenter = presenterGo.GetComponent<BulletPresenter>();
		presenter.transform.SetParent(parent);

		presenter.Init(controller);

		return presenter;
	}

	public void Init(IBulletController controller)
	{
		Controller = controller;
		_transform = transform;
		UnSubscribeEvents();
		SubscribeEvents();
	}

	public void StartMoving()
	{
		StartCoroutine(StartAnimation());
		_bullet.Play();
	}

	private void SubscribeEvents()
	{
		Controller.OnDeathEvent += OnBullitDeath;
		Controller.OnHitEvent += OnBulletdHit;
		Controller.IsPause.ValueChanged += IsPause_ValueChanged;
	}

	private void UnSubscribeEvents()
	{
		if (Controller == null)
			return;

		Controller.OnDeathEvent -= OnBullitDeath;
		Controller.OnHitEvent -= OnBulletdHit;
		Controller.IsPause.ValueChanged -= IsPause_ValueChanged;
	}

	private void IsPause_ValueChanged(bool obj)
	{
		
	}

	private void OnBulletdHit(IBulletController bullet)
	{
		_isAlive = false;
		//ShowCollideParticle
		_bullet.Stop();
		_collision.Play();
		var timeDelay = _collision.main.duration;
		Destroy(gameObject, timeDelay + Time.deltaTime*3f);

		//Debug.Log("OnBulletdHit");
	}

	private void OnBullitDeath(IBulletController asteroid)
	{
		_isAlive = false;
		Destroy(gameObject);
		//Debug.Log("OnBullitDeath");
	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
		//Debug.Log("OnDestroy");
	}

	private void OnTriggerEnter2D(Collider2D other)
	{			
		Controller.OnCollision(other);
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
