using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : IBulletController
{
	public IReadOnlyObservableValue<int> Damage	{ get {	return _damage; }}
	public IMovableObject MovableObject	{get{ return _movableObject; }}
	public IReadOnlyObservableValue<BulletType> Type { get { return _type; } }
	public event Action<IBulletController> OnDeathEvent;
	public event Action<IBulletController> OnHitEvent;

	public IReadOnlyObservableValue<bool> IsPause { get { return _isPause; } }
	private BoolObservableValue _isPause = new BoolObservableValue(false);

	private IMovableObject _movableObject;
	private IntObservableValue _damage = new IntObservableValue();
	private ObservableValue<BulletType> _type = new ObservableValue<BulletType>();

	private float _lifeTimeCounter = 0f;
	private bool _isHit = false;

	public void DeInit()
	{
		//
	}

	public BulletState GetState()
	{
		return new BulletState();
	}

	public void Init(BulletState state, IMovableObject data1)
	{
		_damage.Set(state.Damage);
		_type.Set(state.Type);
		_movableObject = data1;
		_lifeTimeCounter = data1.LifeTime.Value;
		_isHit = false;
	}

	public void OnCollision(Collider2D collider)
	{
		var asteroidPresenter = collider.GetComponent<AsteroidPresenter>();
		if (asteroidPresenter != null)
		{
			asteroidPresenter.Controller.Hit(_damage.Value);
			_isHit = true;
			if (OnHitEvent != null)
			{
				OnHitEvent(this);
			}
		}
	}

	public void Pause()
	{
		_isPause.Set(true);
	}
	public void Resume()
	{
		_isPause.Set(false);
	}

	public void UpdateLifeTime(float deltaTime)
	{
		_lifeTimeCounter -= deltaTime;
		if (_lifeTimeCounter <= 0f && !_isHit)
		{
			if (OnDeathEvent != null)
			{
				OnDeathEvent(this);
			}
		}
	}

	public static IBulletController Create(BulletState state, IMovableObject movableObject)
	{
		var result = new BulletController();
		result.Init(state, movableObject);
		return result;
	}
}

public interface IBulletController: IController<BulletState, IMovableObject>, IPausable
{
	IReadOnlyObservableValue<int> Damage { get; }
	IReadOnlyObservableValue<BulletType> Type { get; }
	IMovableObject MovableObject { get; }
	void OnCollision(Collider2D collider);
	event Action<IBulletController> OnDeathEvent;
	event Action<IBulletController> OnHitEvent;
	void UpdateLifeTime(float deltaTime);
}