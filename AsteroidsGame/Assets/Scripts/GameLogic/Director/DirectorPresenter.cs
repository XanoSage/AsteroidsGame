using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DirectorPresenter : MonoSingleton<DirectorPresenter>
{

	[SerializeField]
	private DirectorsParameters _directorsParameters;

	public DirectorsParameters DirectorParameters{ get { return _directorsParameters;}}

	//private int _waveIndex = 0;   //need if use more than one waves

	private void InitCurrentWave()
	{

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[Serializable]
public class WavesParameters
{
	public float MinAsteroidSpeed;
	public float MaxAsteroidSpeed;
	public float MinAsteroidRotationSpeed;
	public float MaxAsteroidRotationSpeed;
	public List<AsteroidType> AsteroidTypes;
	public List<AsteroidSize> AsteroidSizes;
	public float MinCoolDown;
	public float MaxCoolDown;
	public float BaseLifeTime;
	public int AsteroidCountByWaves;
}

[Serializable]
public class DirectorsParameters
{
	public List<WavesParameters> Waves;

	public WavesParameters GetWavesParameters(int waveIndex)
	{
		if (waveIndex >= Waves.Count)
			return null;
		return Waves[waveIndex];
	}
}

public enum DirectorStates
{
	InitializeWave = 0,
	GenerateAsteroids,
	Pause,
	GetNewWave
}

