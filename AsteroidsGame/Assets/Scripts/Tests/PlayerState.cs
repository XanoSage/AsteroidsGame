using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	[Serializable]
	public class PlayerState
	{

		public int PlayerLife;
		public int Scores;
		public int BestScores;
	}

	public class PlayerController : IPlayerController
	{
		public IReadOnlyObservableValue<int> BestScores	{get{ return _bestScores; }}
		public IReadOnlyObservableValue<int> PlayerLife { get { return _playerLife; } }
		public IReadOnlyObservableValue<int> Scores	{get{ return _scores; }}

		public event Action<IPlayerController> OnPlayerDeath;

		private IntObservableValue _playerLife;
		private IntObservableValue _scores;
		private IntObservableValue _bestScores;

		public PlayerController()
		{
			_playerLife = new IntObservableValue();
			_scores = new IntObservableValue();
			_bestScores = new IntObservableValue();
		}

		public void AddScores(int scores)
		{
			_scores.Add(scores);
			if (_scores.Value >= _bestScores.Value)
			{
				_bestScores.Set(_scores.Value);
			}
		}

		public void DeInit()
		{
			throw new NotImplementedException();
		}

		public PlayerState GetState()
		{
			return new PlayerState()
			{
				Scores = _scores.Value,
				BestScores = _bestScores.Value,
				PlayerLife = _playerLife.Value
			};
		}

		public void Init(PlayerState state)
		{
			_playerLife.Set(state.PlayerLife);
			_scores.Set(state.Scores);
			_bestScores.Set(state.BestScores);
		}

		public void ShipDestroy()
		{
			_playerLife.Subtract(1);
			if (_playerLife.Value <= 0)
			{
				if (OnPlayerDeath != null)
				{
					OnPlayerDeath(this);
				}
			}
		}

	}

	public interface IPlayerController:IController<PlayerState>
	{
		IReadOnlyObservableValue<int> PlayerLife { get; }
		IReadOnlyObservableValue<int> Scores { get; }
		IReadOnlyObservableValue<int> BestScores { get; }
		void ShipDestroy();
		void AddScores(int scores);
		event Action<IPlayerController> OnPlayerDeath;
	}
}