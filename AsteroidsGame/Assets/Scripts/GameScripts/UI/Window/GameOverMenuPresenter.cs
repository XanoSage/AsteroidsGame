using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuPresenter : UIWindowPresenter
{
	[SerializeField]
	private UIButtonPresenter _mainMenuButton;

	[SerializeField]
	private UILabelPresenter _playerScoreLabel;

	public UIButtonCommand MainMenuButton { get { return _mainMenuButton.Command(); } }

	public override void Init()
	{
		// Get Player Score Data and Set this _playerScoreLabel
		IPlayerController controller = GameController.Instance.PlayerController;
		string str = string.Format("{0:D6}", controller.Scores.Value);
		_playerScoreLabel.SetText(str);
		base.Init();
	}

	public override void Show()
	{
		_mainMenuButton.Command().Clicked -= MainMenuButton_Clicked;
		_mainMenuButton.Command().Clicked += MainMenuButton_Clicked;
		base.Show();
	}

	public override void Hide()
	{
		_mainMenuButton.Command().Clicked -= MainMenuButton_Clicked;
		base.Hide();
	}

	private void MainMenuButton_Clicked(ViewPresenter button)
	{
		SceneManager.LoadScene(GameConstants.SceneData.MAIN_MENU_SCENE_NAME);
	}

	private void OnDestroy()
	{
		_mainMenuButton.Command().Clicked -= MainMenuButton_Clicked;
	}
}
