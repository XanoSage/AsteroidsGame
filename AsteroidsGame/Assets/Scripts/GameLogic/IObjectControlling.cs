using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectControlling 
{
	IReadOnlyObservableValue<float> Force01 {get;}
	IReadOnlyObservableValue<Vector3> Direction {get;}
}

