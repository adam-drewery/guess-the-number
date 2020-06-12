using System;
using GuessTheNumber.HighScores;
using static System.Console;

namespace GuessTheNumber.Terminal
{
	internal static class Program
	{
		private static readonly GameRepository HighScores = new GameRepository();

		private static void Main()
		{
			WriteLine("What's your name?");
			var name = ReadLine();

			while(true)
			{
				Clear();

				var game = new Game(name);

				WriteLine($"Hello, {name}!");
				WriteLine("Use the up and down keys to guess whether the next number will be higher or lower.");
				WriteLine("Press (h) to view the high-scores, or any other key to continue.");

				var input = ReadKey();

				if (input.Key == ConsoleKey.H)
					PrintHighScores();

				Clear();

				while (!game.IsFinished)
				{
					WriteLine($"The current number is {game.CurrentNumber}.");
					WriteLine("Will the next be higher or lower?.");

					Guess? guess = null;

					while (guess == null)
					{
						input = ReadKey();

						guess = input.Key switch
						{
							ConsoleKey.UpArrow => Guess.Higher,
							ConsoleKey.DownArrow => Guess.Lower,
							_ => null
						};
					}

					var success = game.GuessNext(guess.Value);

					if (success) WriteLine($"Nice! You guessed {guess.Value.ToString().ToLowerInvariant()} and the next number is {game.CurrentNumber}");
					else WriteLine($"Oh no! You guessed {guess.Value.ToString().ToLowerInvariant()} and the next number is {game.CurrentNumber}");

					if (game.IsFinished)
					{
						if (game.IsComplete) WriteLine($"You completed the game in {game.RunningTime.TotalSeconds} seconds");
						else WriteLine($"You lost the game in {game.RunningTime.Seconds} seconds with a score of {game.Score}");

						var newHighScore = HighScores.AddGame(game);
						if (newHighScore)
						{
							WriteLine("That was a NEW HIGH SCORE!");
							ReadKey();
							WriteLine();
							PrintHighScores();
						}

						WriteLine("Press any key to play again or ESC to quit.");

						var key = ReadKey().Key;
						if (key == ConsoleKey.Escape) return;
						game = new Game(name);
					}
				}
			}
		}

		private static void PrintHighScores()
		{
			Clear();
			WriteLine("Current High Scores:");
			WriteLine();

			foreach (var score in HighScores.GetAll())
			{
				WriteLine("Name: " + score.PlayerName);
				WriteLine("Score: " + score.Score);
				WriteLine("Time: " + score.RunningTime);
				WriteLine();
			}

			ReadKey();
		}
	}
}
