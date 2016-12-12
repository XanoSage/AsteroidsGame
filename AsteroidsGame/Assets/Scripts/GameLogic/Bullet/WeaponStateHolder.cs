using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateHolder : MonoSingleton<WeaponStateHolder>
{
	[SerializeField]
	private WeaponData[] _weaponDatas;

	public WeaponData GetWeaponData(BulletType bullet)
	{
		for (int i = 0; i < _weaponDatas.Length; i++)
		{
			if (_weaponDatas[i].Bullet== bullet)
			{
				return _weaponDatas[i];
			}
		}
		return null;
	}

	
}
