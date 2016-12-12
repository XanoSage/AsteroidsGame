using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : ISpaceShipController
{
	public IReadOnlyObservableValue<float> CurrentRotationSpeed	{get { return _speed; }	}
	public IReadOnlyObservableValue<float> CurrentSpeed	{ get { return _rotationSpeed;}	}
	public IReadOnlyObservableValue<SpaceShipBehaviourState> BehaviourState { get { return _state; } }
	public IWeaponController Weapon { get { return _weaponController; } }
	public IReadOnlyObservableValue<bool> IsPause { get { return _isPause; } }

	public event Action<ISpaceShipController> OnShipDestroyEvent;

	private FloatObservableValue _speed = new FloatObservableValue();
	private FloatObservableValue _rotationSpeed = new FloatObservableValue();
	private BoolObservableValue _isPause = new BoolObservableValue(false);
	private ObservableValue<SpaceShipBehaviourState> _state = new ObservableValue<SpaceShipBehaviourState>();
	private IWeaponController _weaponController;
	private MonoBehaviour _coroutineLauncher;
	private float _invulnerabilityTime = 0;

	public SpaceShipController(MonoBehaviour coroutineLauncher)
	{
		_coroutineLauncher = coroutineLauncher;
	}

	public void OnCollision(Collision2D collision)
	{
		var asteroid = collision.gameObject.GetComponent<AsteroidPresenter>();
		if (asteroid != null)
		{
			SetSpaceShipState(SpaceShipBehaviourState.Invulnerability);
			_coroutineLauncher.StartCoroutine(WaitInvulnerability());
			if (OnShipDestroyEvent != null)
			{
				OnShipDestroyEvent(this);
			} 
		}
	}

	public void DeInit()
	{
		//
	}

	public SpaceShipState GetState()
	{
		return new SpaceShipState();
	}

	public void Init(SpaceShipState state)
	{
		_speed.Set(state.Speed);
		_rotationSpeed.Set(state.RotationSpeed);
		_state.Set(SpaceShipBehaviourState.Flying);
		_invulnerabilityTime = state.IninvulnerabilityTime;
		//_coroutineLauncher.StartCoroutine(WaitInvulnerability());
		Debug.Log("Init");
	}

	public void Fire()
	{
		_weaponController.StartFire();
	}

	public void StopFire()
	{
		_weaponController.StopFire();
	}

	public void InitWeapon(IWeaponController weapon)
	{
		_weaponController = weapon;
	}

	public SpaceShipController()
	{
		_speed.Set(0f);
		_rotationSpeed.Set(0f);
	}

	private IEnumerator WaitInvulnerability()
	{
		float timeCounter = 0f;
		Debug.LogFormat("WaitInvulnerability, time {0}", _invulnerabilityTime);
		while (timeCounter < _invulnerabilityTime)
		{
			timeCounter += Time.deltaTime;
			yield return null;
		}
		SetSpaceShipState(SpaceShipBehaviourState.Flying);
	}
	private void SetSpaceShipState(SpaceShipBehaviourState state)
	{
		_state.Set(state, true);
	}

	public void Pause()
	{
		_isPause.Set(true);
	}
	public void Resume()
	{
		_isPause.Set(false);
	}
}

public interface ISpaceShipController: IController<SpaceShipState>, IPausable
{
	IReadOnlyObservableValue<float> CurrentSpeed { get; }
	IReadOnlyObservableValue<float> CurrentRotationSpeed { get; }
	IReadOnlyObservableValue<SpaceShipBehaviourState> BehaviourState { get; }
	IWeaponController Weapon { get; }
	event Action<ISpaceShipController> OnShipDestroyEvent;

	void OnCollision(Collision2D collision);
	void InitWeapon(IWeaponController weapon);
	void Fire();
	void StopFire();
}