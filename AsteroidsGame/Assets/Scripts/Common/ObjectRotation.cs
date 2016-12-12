using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
	[Header("Select Rotation Axis")]
	[SerializeField]
	private ObjectRotationParameters _parameters;

	[SerializeField]
	private bool _animateOnStart = false;

	private Transform _transform;
	private Vector3 _rotateAngle = Vector3.zero;

	private bool _isAnimated = false;

	public void Init(ObjectRotationParameters parameters)
	{
		_parameters = parameters;
		InitRoatationAngle();
	}

	public void StartRotation()
	{
		StartCoroutine(Rotate());
	}
	public void StopRotation()
	{
		_isAnimated = false;
		StopAllCoroutines();
	}

	private void Awake ()
	{
		_transform = transform;
		if (_parameters != null)
		{
			InitRoatationAngle();
		}	
	}

	private void Start()
	{
		if (_animateOnStart)
			StartRotation();
	}

	private void InitRoatationAngle()
	{
		_rotateAngle.x = _parameters.XAxis ? _parameters.Speed : 0;
		_rotateAngle.y = _parameters.YAxis ? _parameters.Speed : 0;
		_rotateAngle.z = _parameters.ZAxis ? _parameters.Speed : 0;
	}
	
	private void OnDestroy()
	{
		StopRotation();
	}

	private IEnumerator Rotate()
	{
		_isAnimated = true;
		while (_isAnimated)
		{
			_transform.Rotate(_rotateAngle);
			yield return null;
		}
	}
}
[Serializable]
public class ObjectRotationParameters
{
	public bool XAxis;
	public bool YAxis;
	public bool ZAxis;
	public float Speed = 10f;
}
