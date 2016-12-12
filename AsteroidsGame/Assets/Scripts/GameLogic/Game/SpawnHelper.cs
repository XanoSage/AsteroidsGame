using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHelper : MonoSingleton<SpawnHelper>
{
	[SerializeField]
	private List<SpawnData> _spawnDatas;

	public Transform GetRandomSpawnPoint(SpawnSide side)
	{
		Transform result = null;
		var points = GetSpawnPointsBySide(side);
		int index = UnityEngine.Random.Range(0, points.Count);
		result = points[index];
		return result;
	}

	private List<Transform> GetSpawnPointsBySide(SpawnSide side)
	{
		for (int i = 0; i < _spawnDatas.Count; i++)
		{
			if (_spawnDatas[i].SideFrom == side)
				return _spawnDatas[i].Points;
		}
		return null;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[Serializable]
public class SpawnData
{
	public SpawnSide SideFrom;
	public List<Transform> Points;
}

public enum SpawnSide
{
	Top = 0,
	Right,
	Bottom,
	Left
}