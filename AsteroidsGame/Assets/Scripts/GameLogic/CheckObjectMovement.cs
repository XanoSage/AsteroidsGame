using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObjectMovement : MonoBehaviour 
{
	[SerializeField] private Transform _objectTransfom;
	
	private float _noMovementThreshold = 0.001f;
	private const int _NO_MOVEMENT_FRAMES = 3;
	Vector3[] previousLocations = new Vector3[_NO_MOVEMENT_FRAMES];
	private BoolObservableValue _isMoving = new BoolObservableValue(false);
	
	//Let other scripts see if the object is moving
	public IReadOnlyObservableValue<bool> IsMoving
	{
		get{ return _isMoving; }
	}
	
	void Awake()
	{
		//For good measure, set the previous locations
		for(int i = 0; i < previousLocations.Length; i++)
		{
			previousLocations[i] = Vector3.zero;
		}
	}
	
	void Update()
	{
		//Store the newest vector at the end of the list of vectors
		for(int i = 0; i < previousLocations.Length - 1; i++)
		{
			previousLocations[i] = previousLocations[i+1];
		}
		previousLocations[previousLocations.Length - 1] = _objectTransfom.position;
	
		//Check the distances between the points in your previous locations
		//If for the past several updates, there are no movements smaller than the threshold,
		//you can most likely assume that the object is not moving
		for(int i = 0; i < previousLocations.Length - 1; i++)
		{
			if(Vector3.Distance(previousLocations[i], previousLocations[i + 1]) >= _noMovementThreshold)
			{
				//The minimum movement has been detected between frames
				_isMoving.Set(true);
				break;
			}
			else
			{
				_isMoving.Set(false);
			}
		}
	}
}
