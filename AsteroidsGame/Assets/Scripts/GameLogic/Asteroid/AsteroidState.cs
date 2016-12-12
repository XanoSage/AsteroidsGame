using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AsteroidType
{
	Iron = 0,
	Cobalt,
	Titanium
}

public enum AsteroidSize
{
	Small = 0,
	Medium,
	PartOfMedium
}

[Serializable]
public class AsteroidState
{
	public int HitPoint;
	public int ScorePoint;
	public AsteroidType Type;
	public AsteroidSize Size;
}
