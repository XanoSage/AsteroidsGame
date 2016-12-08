using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class JoystickHandler : MonoBehaviour, IObjectControlling, IPointerDownHandler, IPointerUpHandler
{
	public IReadOnlyObservableValue<float> Force01 { get { return _force01;}}
	public IReadOnlyObservableValue<Vector3> Direction { get { return _direction;}}
	[SerializeField] private Image _handler;
	[SerializeField] private float _handlerRadius;
	[SerializeField] private float _animationTime = 0.5f;

	private Vector3 _startPosition;

	private FloatObservableValue _force01 = new FloatObservableValue(0f);
	private Vector3ObservableValue _direction = new Vector3ObservableValue();
	private bool _isPointerDown = false;
	private RectTransform _handlerRectTransform;
	private Vector3 _handlerCenterPoint;
	private bool _isAnimated = false;
	public void OnPointerDown(PointerEventData eventData)
	{
		_isPointerDown = true;
		_handlerCenterPoint = _handlerRectTransform.TransformPoint(_handlerRectTransform.rect.center);//eventData.position;
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		_isPointerDown = false;

		StartCoroutine(MoveToStartPos(_animationTime, SetDefaultObjectControllingData));
	}

	private void UpdateHandlerPosition(Vector3 touchPosition)
	{
		var pos = touchPosition - _handlerCenterPoint; 
		var resultPos = ClampPosition(pos);
		_handlerRectTransform.anchoredPosition =  resultPos;
	}

	private Vector2 ClampPosition(Vector2 pos)
	{
		Vector2 result = Vector2.zero;
		result = Vector2.ClampMagnitude(pos, _handlerRadius);
		return result;
	}

	private void CalculateObjectControllingData()
	{
		float force = (_startPosition - _handlerRectTransform.position).magnitude/_handlerRadius;
		_force01.Set(force);
		Vector3 direction = (_handlerRectTransform.position - _startPosition).normalized;
		_direction.Set(direction);
		Debug.LogFormat("[JoystickHandler] CalculateObjectControllingData - forc: {0}, direction: {1}", force, direction);
	}

	void Start () 
	{
		_handlerRectTransform = _handler.GetComponent<RectTransform>();
		_startPosition = _handlerRectTransform.position;
	}
	
	private void SetDefaultObjectControllingData()
	{
		_force01.Set(0);
		_direction.Set(Vector3.zero);
	}

	// Update is called once per frame
	void Update () 
	{
		if (_isPointerDown)
		{
			UpdateHandlerPosition(Input.mousePosition);
			CalculateObjectControllingData();
		}
		else if (_isAnimated)
		{
			CalculateObjectControllingData();
		}
	}

	private IEnumerator MoveToStartPos(float duration, Action onAnimationEnd)
	{
		_isAnimated = true;
		float timeCounter = 0f;
		Vector3 startPos = _handlerRectTransform.position;
		Vector3 endPos = _startPosition;
		while (timeCounter < duration)
		{
			var time01 = timeCounter/duration;
			_handlerRectTransform.position = Vector3.Lerp(startPos, endPos, time01);
			timeCounter += Time.deltaTime;
			yield return null;
		}
		_handlerRectTransform.position = endPos;
		_isAnimated = false;
		
		if (onAnimationEnd != null)
		{
			onAnimationEnd();
		}
	}
}
