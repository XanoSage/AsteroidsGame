using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
	[SerializeField]
	private Transform _parentForObgects;

	[SerializeField]
	private GameObject _spaceShip;

	[SerializeField]
	private int _defaultPlayerLife;

	[SerializeField]
	private SpaceShipState _spaceShipState;

	[SerializeField]
	private JoystickHandler _joystickHandler;

	//UI
	[SerializeField]
	private PauseMenuPresenter _pauseMenu;

	[SerializeField]
	private GameOverMenuPresenter _gameOverMenuPresenter;

	public IPlayerController PlayerController { get { return _playerController; } }
	private SpaceShipPresenter _spaceShipPresenter;
	private IDirectorController _directorController;
	private ISpaceShipController _spaceShipController;
	private IPlayerController _playerController;


	private int _bestScore = 0;

	public void SaveScoreData(IPlayerController controller)
	{
		//PlayerPrefs.SetInt(_SCORE_TEXT, controller.Scores.Value);
		PlayerPrefs.SetInt(GameConstants.PlayerData.BEST_SCORE_TEXT, controller.BestScores.Value);
	}

	public void PauseMenu(bool needShowMenu = true)
	{
		PauseAllControllers();

		if (needShowMenu)
		{
			_pauseMenu.Init();
			_pauseMenu.Show();
		}
	}

	private void ContinueGame()
	{
		ResumeAllControllers();
	}

	private void SubscribeEvents()
	{
		_pauseMenu.ContinueButton.Clicked += PauseMenu_ContinueButton_Clicked;
		_pauseMenu.MainMenuButton.Clicked += PauseMnu_MainMenuButton_Clicked;
		_gameOverMenuPresenter.MainMenuButton.Clicked += PauseMnu_MainMenuButton_Clicked;
	}

	private void PauseMnu_MainMenuButton_Clicked(ViewPresenter button)
	{
		SaveScoreData(_playerController);
	}

	private void UnSubscribeEvents()
	{
		_pauseMenu.ContinueButton.Clicked -= PauseMenu_ContinueButton_Clicked;
		_pauseMenu.MainMenuButton.Clicked -= PauseMnu_MainMenuButton_Clicked;
		_gameOverMenuPresenter.MainMenuButton.Clicked -= PauseMnu_MainMenuButton_Clicked;
	}

	private void PauseMenu_ContinueButton_Clicked(ViewPresenter button)
	{
		ContinueGame();
	}

	private void CreateSpaceShip()
	{
		var go = Instantiate(_spaceShip, Vector3.zero, Quaternion.identity);
		if (go != null)
		{
			_spaceShipPresenter = go.GetComponent<SpaceShipPresenter>();
			_spaceShipPresenter.Init(_joystickHandler, _spaceShipController);
			go.transform.SetParent(_parentForObgects);
		}
	}

	private void CreateAndInitDirectorController()
	{
		_directorController = new DirectorController(this, _parentForObgects); 
		_directorController.Init(DirectorPresenter.Instance.DirectorParameters);
		_directorController.InitWaves(0);
	}

	private void CreateAndInitSpaceShipController()
	{
		_spaceShipController = new SpaceShipController(this);
		_spaceShipController.Init(_spaceShipState);
		_spaceShipController.OnShipDestroyEvent += SpaceShipController_OnShipDestroy;
	}

	private void SpaceShipController_OnShipDestroy(ISpaceShipController controller)
	{
		_playerController.ShipDestroy();
		if (_playerController.PlayerLife.Value > 0)
		{
			_spaceShipPresenter.ResetPosition();
			Debug.LogFormat("[{0}], SpaceShipController_OnShipDestroy - yet one Life", typeof(GameController));
		}
		Debug.LogFormat("[{0}], SpaceShipController_OnShipDestroy - ShipDestroy, playerLives {1}", typeof(GameController), _playerController.PlayerLife.Value);
	}

	private void CreateAndInitPlayerController()
	{
		_playerController = new PlayerController();
		LoadScoreData();
		var playerState = new PlayerState()
		{
			PlayerLife = _defaultPlayerLife,
			Scores = 0,
			BestScores = _bestScore
		};

		_playerController.Init(playerState);
		_playerController.OnPlayerDeath += PlayerController_OnPlayerDeath;
	}

	private void PlayerController_OnPlayerDeath(IPlayerController controller)
	{
		SaveScoreData(controller);
		PauseMenu(false);
		_gameOverMenuPresenter.Init();
		_gameOverMenuPresenter.Show();
		
	}

	private void PauseAllControllers()
	{
		_spaceShipController.Pause();
		_spaceShipController.Weapon.Pause();
		_directorController.Pause();
	}

	private void ResumeAllControllers()
	{
		_spaceShipController.Resume();
		_spaceShipController.Weapon.Resume();
		_directorController.Resume();
	}

	private void StartDirectorWave()
	{
		_directorController.StartWave();
	}
	
	private void CreateAndInitWeaponController()
	{
		IWeaponController weaponController = new WeaponController(this, _spaceShipPresenter.BulletSpawner, _parentForObgects);
		weaponController.Init(WeaponStateHolder.Instance.GetWeaponData(BulletType.LightBeam));
		_spaceShipController.InitWeapon(weaponController);
		_spaceShipController.Fire();
	}

	// Use this for initialization
	void Awake ()
	{
		CreateAndInitSpaceShipController();
		CreateSpaceShip();
		CreateAndInitPlayerController();
		CreateAndInitDirectorController();
		CreateAndInitWeaponController();
		StartDirectorWave();

		UnSubscribeEvents();
		SubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected override void OnDestroying()
	{
		if (_playerController != null)
		{
			_playerController.OnPlayerDeath -= PlayerController_OnPlayerDeath;
		}
		if (_spaceShipController != null)
		{
			_spaceShipController.OnShipDestroyEvent -= SpaceShipController_OnShipDestroy;
		}
		SaveScoreData(_playerController);
		UnSubscribeEvents();
	}

	private bool LoadScoreData()
	{
		if (PlayerPrefs.HasKey(GameConstants.PlayerData.BEST_SCORE_TEXT))
		{
			_bestScore = PlayerPrefs.GetInt(GameConstants.PlayerData.BEST_SCORE_TEXT);
			return true;
		}
		return false;
	}
}
