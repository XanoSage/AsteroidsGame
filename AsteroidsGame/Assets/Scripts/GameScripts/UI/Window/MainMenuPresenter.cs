using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPresenter : MonoBehaviour
{
	[SerializeField]
	private UILabelPresenter _bestScorePresenter;

	[SerializeField]
	private UIButtonPresenter _playGameButton;

	private int _bestScores = 0;

	// Use this for initialization
	void Start ()
	{
		LoadScoreData();
		Init();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void Init()
	{
		string scoresText = string.Format("{0:D6}", _bestScores);
		_bestScorePresenter.SetText(scoresText);
		_playGameButton.Command().Clicked += MainMenuPresenter_Clicked;
	}

	private void MainMenuPresenter_Clicked(ViewPresenter button)
	{
		SceneManager.LoadScene(GameConstants.SceneData.GAME_PLAY_SCENE_NAME);
	}

	private void LoadScoreData()
	{
		if (PlayerPrefs.HasKey(GameConstants.PlayerData.BEST_SCORE_TEXT))
		{
			_bestScores = PlayerPrefs.GetInt(GameConstants.PlayerData.BEST_SCORE_TEXT);
		}
	}

	private void OnDestroy()
	{
		_playGameButton.Command().Clicked -= MainMenuPresenter_Clicked;
	}
}
