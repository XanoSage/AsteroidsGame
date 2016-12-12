using System;

[Serializable]
public class SpaceShipState
{
	public float Speed = 5f;
	public float RotationSpeed = 10f;
	public float IninvulnerabilityTime = 5f;
	public float ExplosingTime = 3f;
}

public enum SpaceShipBehaviourState
{
	None = 0,
	Invulnerability,
	Flying,
	Explosing,
}