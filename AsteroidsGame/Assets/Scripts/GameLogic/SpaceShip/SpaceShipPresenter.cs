using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipPresenter : MonoBehaviour
{
	private const float _ANIMATION_TIME = 0.3f;
	[SerializeField]
	private Rigidbody2D _rigidBody;

	[SerializeField]
	private CheckObjectMovement _checkObjectMovement;

	[SerializeField]
	private Transform _bulletSpawner;
	public Transform BulletSpawner { get { return _bulletSpawner; } }

	[SerializeField]
	private SpriteRenderer _shipSprite;

	public ISpaceShipController Controller;

	private IObjectControlling _objectControlling;
	private SpriteRenderer _spriteRenderer;
	private Transform _transform;
	private bool _isShipMoving = false;
	private BoxCollider2D _boxCollider;
	private Vector2 _forceBeforePause;

	private Rect _screenBoundsRect;

	public void Init(IObjectControlling objectControlling, ISpaceShipController controller)
	{
		_objectControlling = objectControlling;
		Controller = controller;
		SubscribeEvents();
	}

	public void ResetPosition()
	{
		_rigidBody.velocity = Vector2.zero;
		_transform.position = Vector3.zero;
		_transform.rotation = Quaternion.identity;
		Debug.LogFormat("[SpaceShipPresenter] ResetPosition = OK");
	}

	private void SubscribeEvents()
	{
		_objectControlling.Force01.ValueChanged += Force01_ValueChanged;
		_objectControlling.Direction.ValueChanged += Direction_ValueChanged;
		_checkObjectMovement.IsMoving.ValueChanged += IsMoving_ValueChanged;
		Controller.BehaviourState.ValueChanged += BehaviourState_ValueChanged;
	}

	private void BehaviourState_ValueChanged(SpaceShipBehaviourState state)
	{
		if (state == SpaceShipBehaviourState.Invulnerability)
		{
			SetColliderActivity(false);
			StartCoroutine(PlayInvulnerabilityMode());
		}
		else
		{
			var shipColor = _shipSprite.color;
			shipColor.a = 1;
			_shipSprite.color = shipColor;
			SetColliderActivity(false);
		}
	}

	private void UnSubscribeEvents()
	{
		_objectControlling.Force01.ValueChanged -= Force01_ValueChanged;
		_objectControlling.Direction.ValueChanged -= Direction_ValueChanged;
		_checkObjectMovement.IsMoving.ValueChanged -= IsMoving_ValueChanged;
		Controller.BehaviourState.ValueChanged -= BehaviourState_ValueChanged;
		Controller.IsPause.ValueChanged += IsPause_ValueChanged;
	}

	private void IsPause_ValueChanged(bool obj)
	{
		if (obj)
		{
			_forceBeforePause = _rigidBody.velocity;
			_rigidBody.AddForce(Vector2.zero);
		}
		else
		{
			_rigidBody.AddForce(_forceBeforePause);
			_forceBeforePause = Vector2.zero;
		}
			
		
	}

	private void Force01_ValueChanged(float obj)
	{
		Vector2 force = _objectControlling.Direction.Value * Controller.CurrentSpeed.Value * _objectControlling.Force01.Value;
		_rigidBody.AddForce(force);

		// Debug.LogFormat("[SpaceShipPresenter]Force01_ValueChanged , force {0}", force);
	}

	private void Direction_ValueChanged(Vector3 direction)
	{
		var dir = direction;//_rigidBody.velocity;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		_transform.rotation = Quaternion.Slerp(_transform.rotation, q, Time.deltaTime * Controller.CurrentRotationSpeed.Value);
		//Debug.LogFormat("[SpaceShipPresenter]Direction_ValueChanged , force {0}", dir);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//collision.gameObject.name;
		Controller.OnCollision(collision);
		Debug.LogFormat("[SpaceShipPresenter] OnCollisionEnter - OK, collide with : {0}", collision.gameObject.name);
	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
	}

	// Use this for initialization
	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_transform = transform;
		_checkObjectMovement.IsMoving.ValueChanged += IsMoving_ValueChanged;
		_screenBoundsRect = BoundsToScreenRect(_spriteRenderer.bounds);

		Physics2D.IgnoreLayerCollision(GameConstants.LayerMaskName.SHIP_LAYER_MASK, GameConstants.LayerMaskName.BULLET_LAYER_MASK);

		_boxCollider = _transform.GetComponent<BoxCollider2D>();
	}

	private void IsMoving_ValueChanged(bool value)
	{
		_isShipMoving = value;
	}

	// Update is called once per frame
	void Update()
	{
		if (_isShipMoving)
			CheckShipPosition();
	}

	private void CheckShipPosition()
	{
		var spaceShipScreenPos = Camera.main.WorldToScreenPoint(_transform.position);

		float halfBoundsX = _screenBoundsRect.size.x * 0.5f;
		Vector3 spaceShipWorldPos = _transform.position;
		float spaceShipNewScreenPosX = 0f;
		float spaceShipNewScreenPosY = 0f;
		if (spaceShipScreenPos.x + halfBoundsX >= Screen.width)
		{
			spaceShipNewScreenPosX = spaceShipScreenPos.x + halfBoundsX * 2 - Screen.width;
			spaceShipWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(spaceShipNewScreenPosX, spaceShipScreenPos.y, spaceShipScreenPos.z));
		}
		else if (spaceShipScreenPos.x - halfBoundsX < 0)
		{
			spaceShipNewScreenPosX = Screen.width - spaceShipScreenPos.x - halfBoundsX;
			spaceShipWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(spaceShipNewScreenPosX, spaceShipScreenPos.y, spaceShipScreenPos.z));
		}

		_transform.position = spaceShipWorldPos;

		float halfBoundsY = _screenBoundsRect.size.x * 0.5f;
		if (spaceShipScreenPos.y + halfBoundsY >= Screen.height)
		{

			spaceShipNewScreenPosY = spaceShipScreenPos.y + halfBoundsY * 2 - Screen.height;
			spaceShipWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(spaceShipScreenPos.x, spaceShipNewScreenPosY, spaceShipScreenPos.z));
		}
		else if (spaceShipScreenPos.y - halfBoundsY < 0)
		{
			spaceShipNewScreenPosY = Screen.height - spaceShipScreenPos.y - halfBoundsY;
			spaceShipWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(spaceShipScreenPos.x, spaceShipNewScreenPosY, spaceShipScreenPos.z));
		}

		_transform.position = spaceShipWorldPos;

		//Debug.LogFormat("spaceShipWorldPos: {0}", spaceShipScreenPos);
	}

	public Rect BoundsToScreenRect(Bounds bounds)
	{
		// Get mesh origin and farthest extent (this works best with simple convex meshes)
		Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
		Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

		// Create rect in screen space and return - does not account for camera perspective
		return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
	}

	private void SetColliderActivity(bool isActive)
	{
		_boxCollider.enabled = isActive;
	}

	private IEnumerator PlayInvulnerabilityMode()
	{
		bool isVisible = true;
		float timeCounter = 0;
		var shipColor = _shipSprite.color;
		Debug.LogFormat("[SpaceShipPresenter] PlayInvulnerabilityMode - OK,");
		while (Controller.BehaviourState.Value == SpaceShipBehaviourState.Invulnerability)
		{
			timeCounter = 0;
			while (timeCounter < _ANIMATION_TIME)
			{
				timeCounter += Time.deltaTime;
				yield return null;
			}
			isVisible = !isVisible;
			shipColor = _shipSprite.color;
			shipColor.a = isVisible ? 0 : 1;
			_shipSprite.color = shipColor;

			Debug.LogFormat("[SpaceShipPresenter] PlayInvulnerabilityMode - OK, _shipSprite.color is {0}", shipColor);
			yield return null;
		}
		shipColor = _shipSprite.color;
		shipColor.a = 1;
		_shipSprite.color = shipColor;
		SetColliderActivity(true);
	}
}
