using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace GuessTheNumber.HighScores
{
	/// <summary>Repository for games with the highest scores.</summary>
	public class GameRepository
	{
		private readonly LiteDatabase _database = new LiteDatabase("Scores.db");
		private readonly ILiteCollection<Game> _scores;

		public GameRepository()
		{
			_scores = _database.GetCollection<Game>();
		}

		/// <summary>Get all the high-scores.</summary>
		public IEnumerable<Game> GetAll()
		{
			return _scores.FindAll()
				.OrderByDescending(s => s.Score)
				.ThenBy(s => s.RunningTime);
		}

		/// <summary>Add a game if it is a new high-score.</summary>
		/// <returns>True if the game was a high-score.</returns>
		public bool AddGame(Game game)
		{
			var newTopScores = _scores.FindAll()
				.Concat(new[] {game})
				.OrderByDescending(s => s.Score)
				.ThenBy(s => s.RunningTime)
				.Take(3)
				.ToArray();

			if (!newTopScores.Contains(game))
				return false;

			_scores.DeleteAll();
			_scores.Insert(newTopScores);
			return true;

		}
	}
}
