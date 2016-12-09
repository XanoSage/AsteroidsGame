using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipPresenter : MonoBehaviour
{
	[SerializeField]
	private float _speed;
	[SerializeField] private float _rotationSpeed;

	[SerializeField] private Rigidbody2D _rigidBody;

	[SerializeField] private CheckObjectMovement _checkObjectMovement;

	private IObjectControlling _objectControlling;
	private SpriteRenderer _spriteRenderer;
	private Transform _transform;
	private bool _isShipMoving = false;

	private Rect _screenBoundsRect;

	public void Init(IObjectControlling objectControlling)
	{
		_objectControlling = objectControlling;
		SubscribeEvents();
	}

	private void SubscribeEvents()
	{
		_objectControlling.Force01.ValueChanged += Force01_ValueChanged;
		_objectControlling.Direction.ValueChanged += Direction_ValueChanged;
		_checkObjectMovement.IsMoving.ValueChanged += IsMoving_ValueChanged;
	}

	private void UnSubscribeEvents()
	{
		_objectControlling.Force01.ValueChanged -= Force01_ValueChanged;
		_objectControlling.Direction.ValueChanged -= Direction_ValueChanged;
		_checkObjectMovement.IsMoving.ValueChanged -= IsMoving_ValueChanged;
	}

	private void Force01_ValueChanged(float obj)
	{
		Vector2 force = _objectControlling.Direction.Value * _speed * _objectControlling.Force01.Value;
		_rigidBody.AddForce(force);

		// Debug.LogFormat("[SpaceShipPresenter]Force01_ValueChanged , force {0}", force);
	}

	private void Direction_ValueChanged(Vector3 direction)
	{
		var dir = direction;//_rigidBody.velocity;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
  		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
 		_transform.rotation = Quaternion.Slerp(_transform.rotation, q, Time.deltaTime * _rotationSpeed);
		Debug.LogFormat("[SpaceShipPresenter]Direction_ValueChanged , force {0}", dir);
	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
	}

	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_transform = transform;
		_checkObjectMovement.IsMoving.ValueChanged += IsMoving_ValueChanged;
		_screenBoundsRect = BoundsToScreenRect(_spriteRenderer.bounds);
	}
	
	private void IsMoving_ValueChanged(bool value)
	{
		_isShipMoving = value;
	}

	// Update is called once per frame
	void Update ()
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
			spaceShipNewScreenPosX = spaceShipScreenPos.x + halfBoundsX*2 - Screen.width;
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

			spaceShipNewScreenPosY = spaceShipScreenPos.y + halfBoundsY*2 - Screen.height;
			spaceShipWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(spaceShipScreenPos.x, spaceShipNewScreenPosY, spaceShipScreenPos.z));
		}
		else if (spaceShipScreenPos.y - halfBoundsY < 0)
		{
			spaceShipNewScreenPosY = Screen.height - spaceShipScreenPos.y - halfBoundsY;
			spaceShipWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(spaceShipScreenPos.x, spaceShipNewScreenPosY, spaceShipScreenPos.z));
		}

		_transform.position = spaceShipWorldPos;

		Debug.LogFormat("spaceShipWorldPos: {0}", spaceShipScreenPos);
	}

	public Rect BoundsToScreenRect(Bounds bounds)
 {
     // Get mesh origin and farthest extent (this works best with simple convex meshes)
     Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
     Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));
     
     // Create rect in screen space and return - does not account for camera perspective
     return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
 }
}
