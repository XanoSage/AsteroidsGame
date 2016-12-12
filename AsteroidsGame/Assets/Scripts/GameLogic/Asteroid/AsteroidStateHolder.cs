using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidStateHolder : MonoSingleton<AsteroidStateHolder>
{
	[SerializeField]
	private AsteroidState[] _asteroids;

	public AsteroidState GetState(AsteroidSize size, AsteroidType type)
	{
		AsteroidState result = null;
		for (int i = 0; i < _asteroids.Length; i++)
		{
			if (_asteroids[i].Size == size && _asteroids[i].Type == type)
			{
				result = _asteroids[i];
				break;
			}
		}
		return result;
	}

	public AsteroidState GetState(AsteroidType type)
	{
		AsteroidState result = null;
		return result;
	}
}
