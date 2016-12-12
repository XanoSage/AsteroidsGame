using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateHolder : MonoSingleton<BulletStateHolder>
{
	[SerializeField]
	private BulletState[] _states;

	public BulletState GetBulletState(BulletType type)
	{
		for (int i = 0; i < _states.Length; i++)
		{
			if (_states[i].Type == type)
				return _states[i];
		}
		return null;
	}
}
