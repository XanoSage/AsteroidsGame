using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField]
	private GameObject _spaceShip;

	[SerializeField]
	private Transform _parentForObgects;

	[SerializeField]
	private JoystickHandler _joystickHandler;

	private SpaceShipPresenter _spaceShipPresenter;
	
	private void CreateSpaceShip()
	{
		var go = Instantiate(_spaceShip, Vector3.zero, Quaternion.identity);
		if (go != null)
		{
			_spaceShipPresenter = go.GetComponent<SpaceShipPresenter>();
			_spaceShipPresenter.Init(_joystickHandler);
			go.transform.SetParent(_parentForObgects);
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		CreateSpaceShip();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
