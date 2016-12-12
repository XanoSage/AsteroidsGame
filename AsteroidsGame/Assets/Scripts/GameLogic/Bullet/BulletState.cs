using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BulletState
{
	public int Damage;
	public BulletType Type;
}

public enum BulletType
{
	LightBeam = 0,
	Laser,
	Missile
}