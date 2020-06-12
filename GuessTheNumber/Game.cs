using System;
using System.Diagnostics;
using System.Linq;

namespace GuessTheNumber
{
	/// <summary>Represents a game of "guess the number".</summary>
	public class Game
	{
		private readonly int[] _numbers;
		private readonly Stopwatch _stopwatch = new Stopwatch();

		/// <summary>The current game round.</summary>
		public int Round { get; private set; } = 1;

		/// <summary>The current game score.</summary>
		public int Score { get; private set; }

		/// <summary>Has the player completed the game?</summary>
		public bool IsComplete { get; private set; }

		/// <summary>Has the game finished, whether or not the player completed it?</summary>
		public bool IsFinished { get; private set; }

		/// <summary>The name of the player.</summary>
		public string PlayerName { get; }

		public int PointsRequiredToWin { get; set; } = 10;

		/// <summary>The length of time the game has run for.</summary>
		public TimeSpan RunningTime => _stopwatch.Elapsed;

		/// <summary>
		/// The current number which the player needs to
		/// guess a comparison against the next number.
		/// </summary>
		/// <remarks>The score also doubles as the round number so we can use it to determine the current number.</remarks>
		public int CurrentNumber => _numbers[Round - 1];

		public Game(string playerName) : this(playerName, GenerateNumbers()) { }

		public Game(string playerName, int[] numbers)
		{
			PlayerName = playerName;
			_stopwatch.Start();
			_numbers = numbers;
		}

		/// <summary>
		/// Guess whether the next number will be higher or lower. Incorrect guesses end the game.
		/// </summary>
		/// <returns>True if the guess was correct.</returns>
		/// <exception cref="InvalidOperationException">The game is already finished.</exception>
		public bool GuessNext(Guess comparison)
		{
			if (IsFinished) throw new InvalidOperationException("The game is finished.");

			var previous = CurrentNumber;
			Round++;

			var success = comparison == Guess.Lower
				? CurrentNumber < previous
				: CurrentNumber > previous;

			if (!success) Finish();
			else Score++;

			if (Score == PointsRequiredToWin) Complete();

			return success;
		}

		private static int[] GenerateNumbers()
		{
			return Enumerable.Range(1, 100)
				.OrderBy(x => Guid.NewGuid())
				.Take(11)
				.ToArray();
		}

		private void Complete()
		{
			IsComplete = true;
			Finish();
		}

		private void Finish()
		{
			_stopwatch.Stop();
			IsFinished = true;
		}
	}
}
