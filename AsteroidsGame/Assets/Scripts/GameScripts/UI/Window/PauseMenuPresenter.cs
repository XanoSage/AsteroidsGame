using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPresenter : UIWindowPresenter
{
	[SerializeField]
	private UIButtonPresenter _continueButton;

	[SerializeField]
	private UIButtonPresenter _mainMenuButton;

	public UIButtonCommand ContinueButton { get { return _continueButton.Command(); } }
	public UIButtonCommand MainMenuButton { get { return _mainMenuButton.Command(); } }

	public override void Init()
	{
	}

	public override void Show()
	{
		UnSubscribeEvents();
		SubscribeEvents();
		base.Show();
	}

	public override void Hide()
	{
		UnSubscribeEvents();
		base.Hide();
	}

	private void SubscribeEvents()
	{
		_continueButton.Command().Clicked += ContinueButton_Clicked;
		_mainMenuButton.Command().Clicked += MainMenuButton_Clicked;
	}

	private void UnSubscribeEvents()
	{
		_continueButton.Command().Clicked -= ContinueButton_Clicked;
		_mainMenuButton.Command().Clicked -= MainMenuButton_Clicked;
	}

	private void MainMenuButton_Clicked(ViewPresenter button)
	{
		SceneManager.LoadScene(GameConstants.SceneData.MAIN_MENU_SCENE_NAME);
	}

	private void ContinueButton_Clicked(ViewPresenter button)
	{
		Hide();
	}

	private void OnDestroy()
	{
		UnSubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
		
	}


}
