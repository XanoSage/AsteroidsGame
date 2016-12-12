using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData
{
	public BulletType Bullet;
	public float Speed;
	public float CoolDown;
}

public interface IWeaponController:IController<WeaponData>, IPausable
{
	IReadOnlyObservableValue<float> BulletSpeed { get; }
	IReadOnlyObservableValue<float> Cooldown { get; }
	IReadOnlyObservableValue<BulletType> Bullet { get; }
	void ChangeWeapon(WeaponData data);
	void StartFire();
	void StopFire();
}

public class WeaponController : IWeaponController
{
	public IReadOnlyObservableValue<float> BulletSpeed { get { return _bulletSpeed; } }
	public IReadOnlyObservableValue<float> Cooldown { get { return _cooldown; } }
	public IReadOnlyObservableValue<BulletType> Bullet { get { return _bulletType; } }

	public IReadOnlyObservableValue<bool> IsPause { get { return _isPause; } }
	private BoolObservableValue _isPause = new BoolObservableValue(false);

	private MonoBehaviour _coroutineLauncher;
	private Transform _parentForBullets;
	private Transform _spawnPoints;

	private FloatObservableValue _bulletSpeed = new FloatObservableValue();
	private FloatObservableValue _cooldown = new FloatObservableValue();
	private ObservableValue<BulletType> _bulletType = new ObservableValue<BulletType>();

	private bool _isOnFire = false;

	private List<IBulletController> _bullets;

	public WeaponController(MonoBehaviour coroutineLauncher, Transform spawnPoint, Transform parentForBullets)
	{
		_coroutineLauncher = coroutineLauncher;
		_spawnPoints = spawnPoint;
		_parentForBullets = parentForBullets;
		_bullets = new List<IBulletController>();
	}

	public void ChangeWeapon(WeaponData data)
	{
		_bulletSpeed.Set(data.Speed);
		_bulletType.Set(data.Bullet);
		_cooldown.Set(data.CoolDown);
	}

	public void DeInit()
	{
		//
	}

	public WeaponData GetState()
	{
		return new WeaponData();
	}

	public void Init(WeaponData state)
	{
		_bulletSpeed.Set(state.Speed);
		_bulletType.Set(state.Bullet);
		_cooldown.Set(state.CoolDown);
	}

	public void StartFire()
	{
		_coroutineLauncher.StartCoroutine(Fire());
		
	}

	public void StopFire()
	{
		_isOnFire = false;
	}

	public void Pause()
	{
		_isPause.Set(true);
		for (int i = 0; i < _bullets.Count; i++)
		{
			_bullets[i].Pause();
		}
	}
	public void Resume()
	{
		_isPause.Set(false);
		for (int i = 0; i < _bullets.Count; i++)
		{
			_bullets[i].Resume();
		}
	}

	private IBulletController CreateBullet()
	{
		BulletState bulletState = BulletStateHolder.Instance.GetBulletState(_bulletType.Value);
		IMovableObject movableObject = MovableObjectState.Create(_bulletSpeed.Value, 0f, GameConstants.BulletData.DEFAULT_LIFETIME, _spawnPoints.up);
		IBulletController bullet = BulletController.Create(bulletState, movableObject);
		GameObject prefab = BulletPoolContainer.Instance.GetBulletPrefab(_bulletType.Value);

		var bulletPresenter = BulletPresenter.Create(bullet, prefab, _spawnPoints, _parentForBullets);
		if (bulletPresenter != null)
		{
			bulletPresenter.StartMoving();
		}

		//Debug.Log("[WeaponController] CreateBullet - OK");
		return bullet;
	}

	private void Bullet_OnHitEvent(IBulletController obj)
	{
		obj.OnHitEvent -= Bullet_OnDeathEvent;
		RemoveBullet(obj);
	}

	private void Bullet_OnDeathEvent(IBulletController obj)
	{
		obj.OnDeathEvent -= Bullet_OnDeathEvent;
		RemoveBullet(obj);
	}

	private IEnumerator Fire()
	{
		_isOnFire = true;
		while (_isOnFire)
		{
			if (_isPause.Value)
			{
				yield return null;
			}
			else
			{

				yield return new WaitForSeconds(_cooldown.Value);
				var bullet = CreateBullet();
				bullet.OnDeathEvent += Bullet_OnDeathEvent;
				bullet.OnHitEvent += Bullet_OnHitEvent;
				AddBullet(bullet);
				if (!_isOnFire)
					yield break;

				yield return null;
			}
		}
	}

	private void AddBullet(IBulletController bullet)
	{
		_bullets.Add(bullet);

	}
	private void RemoveBullet(IBulletController bullet)
	{
		_bullets.Remove(bullet);

	}
}