using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : ISpaceShipController
{
	public IReadOnlyObservableValue<float> CurrentRotationSpeed
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public IReadOnlyObservableValue<float> CurrentSpeed
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public void DeInit()
	{
		throw new NotImplementedException();
	}

	public SpaceShipStaticData GetState()
	{
		throw new NotImplementedException();
	}

	public void Init(SpaceShipStaticData state, IObjectControlling data1)
	{
		throw new NotImplementedException();
	}
}

public interface ISpaceShipController: IController<SpaceShipStaticData, IObjectControlling>
{
	IReadOnlyObservableValue<float> CurrentSpeed { get; }
	IReadOnlyObservableValue<float> CurrentRotationSpeed { get; }
}