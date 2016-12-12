using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDPresenter : ViewPresenter
{
	private const string _PLAYER_LIFE_FORMAT = "[{0}]";
	[SerializeField]
	private UILabelPresenter _scoresLabel;

	[SerializeField]
	private UILabelPresenter _bestScoresLabel;

	[SerializeField]
	private UILabelPresenter _playerLifeLabel;

	[SerializeField]
	private GameObject[] _playerLifeSprites;

	[SerializeField]
	private UIButtonPresenter _pauseMenuButton;

	private IPlayerController _playerController;

	private void Init()
	{
		_playerController = GameController.Instance.PlayerController;
		UnsubscribeEvents();
		SubscribeEvents();
		SetData();
	}

	private void SubscribeEvents()
	{
		_playerController.PlayerLife.ValueChanged += PlayerLife_ValueChanged;
		_playerController.Scores.ValueChanged += Scores_ValueChanged;
		_playerController.BestScores.ValueChanged += BestScores_ValueChanged;
		_pauseMenuButton.Command().Clicked += ButtonMenu_Clicked;
	}

	private void ButtonMenu_Clicked(ViewPresenter button)
	{
		GameController.Instance.PauseMenu();
	}

	private void UnsubscribeEvents()
	{
		_playerController.PlayerLife.ValueChanged -= PlayerLife_ValueChanged;
		_playerController.Scores.ValueChanged -= Scores_ValueChanged;
		_playerController.BestScores.ValueChanged -= BestScores_ValueChanged;
	}

	private void BestScores_ValueChanged(int value)
	{
		string scores = string.Format("{0:D6}", value);
		_bestScoresLabel.SetText(scores);
	}

	private void Scores_ValueChanged(int value)
	{
		string scores = string.Format("{0:D6}", value);
		_scoresLabel.SetText(scores);
	}

	private void PlayerLife_ValueChanged(int value)
	{
		string playerLife = string.Format(_PLAYER_LIFE_FORMAT, value);
		_playerLifeLabel.SetText(playerLife);
		SetPlayerLifeSpriteVisible(value);
	}

	private void SetPlayerLifeSpriteVisible(int playerLife)
	{
		for (int i = 0; i < _playerLifeSprites.Length; i++)
		{
			if (i < playerLife)
				_playerLifeSprites[i].SetActive(true);
			else
				_playerLifeSprites[i].SetActive(false);
		}

	}

	private void OnDestroy()
	{
		UnsubscribeEvents();
	}

	private void SetData()
	{
		PlayerLife_ValueChanged(_playerController.PlayerLife.Value);
		Scores_ValueChanged(_playerController.Scores.Value);
		BestScores_ValueChanged(_playerController.BestScores.Value);
	}

	// Use this for initialization
	void Start ()
	{
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
