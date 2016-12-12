using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class DirectorController : IDirectorController
{
	public IReadOnlyObservableValue<bool> IsPause { get { return _isPause; } }
	private BoolObservableValue _isPause = new BoolObservableValue(false);

	//private List<AsteroidPresenter> _asteroidPresenters;
	private DirectorsParameters _parameters;
	private MonoBehaviour _coroutineLauncher;
	private int _currentWaveIndex;
	private int _asteroidLaunchCounter;
	private WavesParameters _currentWavesParameter;
	private readonly SpawnSide[] _sides = (SpawnSide[])Enum.GetValues(typeof(SpawnSide));
	private Transform _parentForAsteroids;

	private List<IAsteroidController> _asteroids;

	public DirectorController(MonoBehaviour coroutineLauncher, Transform asteroidParent)
	{
		_coroutineLauncher = coroutineLauncher;
		_asteroids = new List<IAsteroidController>();
		_currentWaveIndex = 0;
		_parentForAsteroids = asteroidParent;
	}
	public void DeInit()
	{

	}

	public DirectorsParameters GetDirectorsState()
	{
		return _parameters;
	}

	public DirectorsParameters GetState()
	{
		return _parameters;
	}

	public void Init(DirectorsParameters state)
	{
		_parameters = state;
		_isPause.Set(false);
	}

	public void InitWaves(int indexOfWaves)
	{
		_currentWavesParameter = _parameters.Waves[_currentWaveIndex];
	}

	public void StartWave()
	{
		_coroutineLauncher.StartCoroutine(StartWaves());
	}

	private AsteroidType GetTypeForAsteroid()
	{
		int indexOfTypeArrays = UnityRandom.Range(0, _currentWavesParameter.AsteroidTypes.Count);
		var asteroidType = _currentWavesParameter.AsteroidTypes[indexOfTypeArrays];
		return asteroidType;
	}

	private AsteroidSize GetSizeForAsteroid()
	{
		int indexOfSizeArrays = UnityRandom.Range(0, _currentWavesParameter.AsteroidSizes.Count);
		var asteroidSize = _currentWavesParameter.AsteroidSizes[indexOfSizeArrays];
		return asteroidSize;
	}

	private float GetSpeedForAsteroid()
	{
		float speed = UnityRandom.Range(_currentWavesParameter.MinAsteroidSpeed, _currentWavesParameter.MaxAsteroidSpeed);
		return speed;
	}

	private IEnumerator StartWaves()
	{
		int asteroidsCountInWave = _currentWavesParameter.AsteroidCountByWaves;
		while (_asteroidLaunchCounter < asteroidsCountInWave)
		{
			if (_isPause.Value)
			{
				yield return null;
			}
			else
			{
				float cooldownTime = UnityRandom.Range(_currentWavesParameter.MinCoolDown, _currentWavesParameter.MaxCoolDown);
				yield return new WaitForSeconds(cooldownTime);
				var asteroidController = CreateAsteroid();
				asteroidController.OnDeathEvent += AsteroidController_OnDeathEvent;
				asteroidController.OnDestroyEvent += AsteroidController_OnDestroyEvent;
				AddAsteroid(asteroidController);
				_asteroidLaunchCounter++;
				yield return null;
			}
		}
	}

	private void AsteroidController_OnDestroyEvent(IAsteroidController obj)
	{
		RemoveAsteroid(obj);
		GameController.Instance.PlayerController.AddScores(obj.ScorePoint.Value);
		obj.OnDeathEvent -= AsteroidController_OnDeathEvent;
		obj.OnDestroyEvent -= AsteroidController_OnDestroyEvent;
	}

	private void AsteroidController_OnDeathEvent(IAsteroidController obj)
	{
		RemoveAsteroid(obj);
		obj.OnDeathEvent -= AsteroidController_OnDeathEvent;
		obj.OnDestroyEvent -= AsteroidController_OnDestroyEvent;
	}

	private void ResetWaveParameters()
	{
		_asteroidLaunchCounter = 0;
	}

	private void AddAsteroid(IAsteroidController asteroid)
	{
		_asteroids.Add(asteroid);
	}
	private void RemoveAsteroid(IAsteroidController asteroid)
	{
		_asteroids.Remove(asteroid);
	}

	private SpawnSide GetSpawnSide()
	{
		int indexOfTheSide = UnityRandom.Range(0, _sides.Length);
		return _sides[indexOfTheSide];
	}

	private Transform GetSpawnPoint(SpawnSide side)
	{
		var point = SpawnHelper.Instance.GetRandomSpawnPoint(side);
		return point;
	}

	private float GetRotationSpeedForAsteroid()
	{
		float rotationSpeed = UnityRandom.Range(_currentWavesParameter.MinAsteroidRotationSpeed, _currentWavesParameter.MaxAsteroidRotationSpeed);
		return rotationSpeed;
	}

	private IAsteroidController CreateAsteroid()
	{
		AsteroidType type = GetTypeForAsteroid();
		AsteroidSize size = GetSizeForAsteroid();
		AsteroidState asteroidState = AsteroidStateHolder.Instance.GetState(size, type);
		float speed = GetSpeedForAsteroid();
		float rotationSpeed = GetRotationSpeedForAsteroid();
		SpawnSide spawnSide = GetSpawnSide();
		float lifeTime = GetAsteroidLifeTime(spawnSide, speed);
		Transform spawnPoint = GetSpawnPoint(spawnSide);
		IMovableObject movableObject = MovableObjectState.Create(speed, rotationSpeed, lifeTime, spawnPoint.up);

		IAsteroidController asteroidController = AsteroidController.Create(asteroidState, movableObject);

		GameObject asteroidPrefab = AsteroidPoolContainer.Instance.GetAsteroidPrefab(size, type);
		var asteroidPresenter = AsteroidPresenter.Create(asteroidController, asteroidPrefab, spawnPoint, _parentForAsteroids);
		if (asteroidPresenter != null)
		{
			asteroidPresenter.StartMoving();
		}
		//Debug.LogFormat("[DirectorController] CreateAsteroidController - OK, was created size:{0}, type: {1}, lifeTime: {2}, spawnSide: {3} ", size, type, lifeTime, spawnSide);
		return asteroidController;
	}

	private float GetAsteroidLifeTime(SpawnSide side, float speed)
	{
		float baseLifeTime = _currentWavesParameter.BaseLifeTime;
		float result = 0;
		if (side == SpawnSide.Bottom || side == SpawnSide.Top)
		{
			result = baseLifeTime * 7.5f * speed;
		}
		else
		{
			result = baseLifeTime * 15f * speed;
		}

		return result;
	}

	public void Pause()
	{
		_isPause.Set(true, true);
		for (int i = 0; i < _asteroids.Count; i++)
		{
			_asteroids[i].Pause();
		}
	}
	public void Resume()
	{
		_isPause.Set(false, true);
		for (int i = 0; i < _asteroids.Count; i++)
		{
			_asteroids[i].Resume();
		}
	}
}

public interface IDirectorController : IController<DirectorsParameters>, IPausable
{
	void StartWave();
	void InitWaves(int indexOfWaves);
	DirectorsParameters GetDirectorsState();
}

