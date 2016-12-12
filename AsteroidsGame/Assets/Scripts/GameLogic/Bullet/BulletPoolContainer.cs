using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletPoolContainer : MonoSingleton<BulletPoolContainer>
 {

	[SerializeField]
	private BulletPoolHelper[] _bulletPrefabs;

	[SerializeField]
	private int _maxContainerSize = 30;

	[SerializeField]
	private int _containerSize = 5;

	public GameObject GetBulletPrefab(BulletType type)
	{
		for (int i = 0; i < _bulletPrefabs.Length; i++)
		{
			if (_bulletPrefabs[i].Type == type)
				return _bulletPrefabs[i].Prefab;
		}
		return null;
	}

	private void Awake()
	{
		for (int i = 0; i < _bulletPrefabs.Length; i++)
		{
			ObjectPoolManager.CreatePool(_bulletPrefabs[i].Prefab, _containerSize, _maxContainerSize);
		}
	}
}

[Serializable]
public class BulletPoolHelper
{
	public BulletType Type;
	public GameObject Prefab;
}