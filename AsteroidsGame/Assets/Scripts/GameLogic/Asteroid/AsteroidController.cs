using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : IAsteroidController
{
	public IReadOnlyObservableValue<int> HitPoint{ get { return _hitPoint; }}
	public IReadOnlyObservableValue<int> ScorePoint	{ get { return _scorePoint;	}}
	public IReadOnlyObservableValue<AsteroidSize> Size { get { return _size; } }
	public IReadOnlyObservableValue<AsteroidType> Type { get { return _type; } }
	public IMovableObject MovableObject { get { return _movableObject; } }

	public IReadOnlyObservableValue<bool> IsPause { get { return _isPause; } }
	private BoolObservableValue _isPause = new BoolObservableValue(false);

	private IntObservableValue _hitPoint = new IntObservableValue(0);
	private IntObservableValue _scorePoint = new IntObservableValue(0);
	
	private ObservableValue<AsteroidType> _type = new ObservableValue<AsteroidType>();
	private ObservableValue<AsteroidSize> _size = new ObservableValue<AsteroidSize>();
	private IMovableObject _movableObject;
	private float _lifeTimeCounter;

	public event Action<IAsteroidController> OnDeathEvent;
	public event Action<IAsteroidController> OnDestroyEvent;

	public void DeInit()
	{
		//
	}

	public AsteroidState GetState()
	{
		return new AsteroidState();
	}

	public void Hit(int damage)
	{
		_hitPoint.Subtract(damage);
		if (_hitPoint.Value <= 0)
		{
			if (OnDestroyEvent != null)
			{
				OnDestroyEvent(this);
			}
		}
	}

	public void Init(AsteroidState state, IMovableObject data1)
	{
		_movableObject = data1;

		_hitPoint.Set(state.HitPoint);
		_scorePoint.Set(state.ScorePoint);
		_type.Set(state.Type);
		_size.Set(state.Size);
		_lifeTimeCounter = data1.LifeTime.Value;
	}

	public void UpdateLifeTime(float deltaTime)
	{
		_lifeTimeCounter -= deltaTime;
		if (_lifeTimeCounter <= 0f)
		{
			if (OnDeathEvent != null)
			{
				OnDeathEvent(this);
			}
		}
	}

	public static IAsteroidController Create(AsteroidState state, IMovableObject movableObject)
	{
		var result = new AsteroidController();
		result.Init(state, movableObject);
		return result;
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

public interface IAsteroidController: IController<AsteroidState, IMovableObject>, IScorable, IPausable
{
	IReadOnlyObservableValue<int> HitPoint { get; }
	IReadOnlyObservableValue<AsteroidSize> Size { get; }
	IReadOnlyObservableValue<AsteroidType> Type { get; }
	IMovableObject MovableObject { get; }
	event Action<IAsteroidController> OnDeathEvent;
	event Action<IAsteroidController> OnDestroyEvent;
	void Hit(int damage);
	void UpdateLifeTime(float deltaTime);
}

public interface IMovableObject
{
	IReadOnlyObservableValue<float> Speed { get; }
	IReadOnlyObservableValue<float> LifeTime { get; }
	IReadOnlyObservableValue<float> RotationSpeed { get; }
	IReadOnlyObservableValue<Vector3> Direction { get; }
}
public interface IScorable
{
	IReadOnlyObservableValue<int> ScorePoint { get; }
}

public class MovableObjectState: IMovableObject
{
	public IReadOnlyObservableValue<float> Speed { get { return _speed; } }
	public IReadOnlyObservableValue<float> LifeTime { get { return _lifeTime; } }
	public IReadOnlyObservableValue<float> RotationSpeed { get { return _rotationSpeed; } }
	public IReadOnlyObservableValue<Vector3> Direction { get { return _direction; } }

	private FloatObservableValue _speed = new FloatObservableValue();
	private FloatObservableValue _lifeTime = new FloatObservableValue();
	private FloatObservableValue _rotationSpeed = new FloatObservableValue();
	private Vector3ObservableValue _direction = new Vector3ObservableValue();

	public MovableObjectState(float speed, float rotationSpeed, float lifeTime, Vector3 direction)
	{
		_speed.Set(speed);
		_rotationSpeed.Set(rotationSpeed);
		_direction.Set(direction);
		_lifeTime.Set(lifeTime);
	}

	public static IMovableObject Create(float speed, float rotationSpeed, float lifeTime, Vector3 direction)
	{
		return new MovableObjectState(speed, rotationSpeed, lifeTime, direction);
	}
}
