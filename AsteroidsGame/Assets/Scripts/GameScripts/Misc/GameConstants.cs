using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameConstants
{
	public static class PlayerData
	{
		public const string BEST_SCORE_TEXT = "BestScore";
	}

	public static class SceneData
	{
		public const string MAIN_MENU_SCENE_NAME = "MainMenu";
		public const string GAME_PLAY_SCENE_NAME = "GamePlay";
	}

	public static class BulletData
	{
		public const float DEFAULT_LIFETIME = 5f;
	}

	public static class LayerMaskName
	{
		public const int BULLET_LAYER_MASK = 10;
		public const int SHIP_LAYER_MASK = 11;
	}

}
public enum WindowType
{
	PauseMenu = 0,
	GameOverMenu = 1,
	MainMenu = 2
}

public enum WindowsPriority
{
	LowPriority = 3,
	MiddlePriority = 16,
	HighPriority = 999
}


