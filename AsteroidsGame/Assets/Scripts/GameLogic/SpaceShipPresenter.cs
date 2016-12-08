using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipPresenter : MonoBehaviour
{
	[SerializeField]
	private float _speed;
	[SerializeField]
	private float _rotationSpeed;

	[SerializeField]
	private Rigidbody2D _rigidBody;

	private IObjectControlling _objectControlling;
	private SpriteRenderer _spriteRenderer;
	private Transform _transform;

	public void Init(IObjectControlling objectControlling)
	{
		_objectControlling = objectControlling;
		SubscribeEvents();
	}

	private void SubscribeEvents()
	{
		_objectControlling.Force01.ValueChanged += Force01_ValueChanged;
		_objectControlling.Direction.ValueChanged += Direction_ValueChanged;
	}

	private void UnSubscribeEvents()
	{
		_objectControlling.Force01.ValueChanged -= Force01_ValueChanged;
		_objectControlling.Direction.ValueChanged -= Direction_ValueChanged;
	}

	private void Force01_ValueChanged(float obj)
	{
		Vector2 force = _objectControlling.Direction.Value * _speed * _objectControlling.Force01.Value;
		_rigidBody.AddForce(force);

		Debug.LogFormat("[SpaceShipPresenter]Force01_ValueChanged , force {0}", force);
	}

	private void Direction_ValueChanged(Vector3 obj)
	{
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
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckShipPosition();
	}

	private void CheckShipPosition()
	{
		var spaceShipScreenPos = Camera.main.WorldToScreenPoint(_transform.position);
		var bounds = _spriteRenderer.bounds;
		float halfBoundsX = bounds.size.x * 0.5f;
		Vector3 spaceShipWorldPos = _transform.position;
		if (spaceShipScreenPos.x + halfBoundsX > Screen.width)
		{
			float spaceShipWorldPosX = spaceShipScreenPos.x + halfBoundsX - Screen.width;
			spaceShipWorldPos = Camera.main.ScreenToViewportPoint(new Vector3(spaceShipWorldPosX, spaceShipScreenPos.y, spaceShipScreenPos.z));
		}
		else if (spaceShipScreenPos.x - halfBoundsX < 0)
		{
			float spaceShipWorldPosX = -Screen.width - spaceShipScreenPos.x - halfBoundsX;
			spaceShipWorldPos = Camera.main.ScreenToViewportPoint(new Vector3(spaceShipWorldPosX, spaceShipScreenPos.y, spaceShipScreenPos.z));	
		}

		_transform.position = spaceShipWorldPos;

		float halfBoundsY = bounds.size.x * 0.5f;
		if (spaceShipScreenPos.y + halfBoundsY > Screen.width)
		{

			float spaceShipWorldPosY = spaceShipScreenPos.y + halfBoundsY - Screen.height;
			spaceShipWorldPos = Camera.main.ScreenToViewportPoint(new Vector3(spaceShipScreenPos.x, spaceShipWorldPosY, spaceShipScreenPos.z));
		}
		else if (spaceShipScreenPos.x - halfBoundsY < 0)
		{
			float spaceShipWorldPosX = -Screen.height - spaceShipScreenPos.y - halfBoundsY;
			spaceShipWorldPos = Camera.main.ScreenToViewportPoint(new Vector3(spaceShipScreenPos.x, spaceShipWorldPosX, spaceShipScreenPos.z));
		}

		_transform.position = spaceShipWorldPos;
	}
}
