using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPoolContainer : MonoSingleton<AsteroidPoolContainer>
{
	[SerializeField]
	private GameObject _ironSmallPrefab;

	[SerializeField]
	private GameObject _cobaltSmallPrefab;

	[SerializeField]
	private AsteroidPoolHelper[] _asteroidPrefabs;

	[SerializeField]
	private int _maxContainerSize = 30;

	[SerializeField]
	private int _containerSize = 5;

	public GameObject GetAsteroidPrefab(AsteroidSize size, AsteroidType type)
	{
		for (int i = 0; i < _asteroidPrefabs.Length; i++)
		{
			if (_asteroidPrefabs[i].Size == size && _asteroidPrefabs[i].Type == type)
				return _asteroidPrefabs[i].Prefab;
		}
		return null;
	}
	
	private void Awake()
	{
		//ObjectPoolManager.CreatePool(_ironSmallPrefab, _containerSize, _maxContainerSize);
		//ObjectPoolManager.CreatePool(_cobaltSmallPrefab, _containerSize, _maxContainerSize);

		for (int i = 0; i < _asteroidPrefabs.Length; i++)
		{
			ObjectPoolManager.CreatePool(_asteroidPrefabs[i].Prefab, _containerSize, _maxContainerSize);
		}
	}
}

[Serializable]
public class AsteroidPoolHelper
{
	public AsteroidSize Size;
	public AsteroidType Type;
	public GameObject Prefab;
}
