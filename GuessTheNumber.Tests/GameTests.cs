using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace GuessTheNumber.Tests
{
	public class GameTests
	{
		private static readonly int[] TestNumbers = Enumerable.Range(1, 11).ToArray();

		private readonly Game _game  = new Game("Dave", TestNumbers);

		[Fact]
		public void Ends_when_player_guesses_wrong()
		{
			_game.GuessNext(Guess.Lower);

			_game.Score.Should().Be(0);
			_game.IsFinished.Should().BeTrue();
			_game.IsComplete.Should().BeFalse();

			var func = new Func<bool>(() => _game.GuessNext(Guess.Higher));
			func.Should().ThrowExactly<InvalidOperationException>();
		}

		[Fact]
		public void Awards_a_point_when_the_player_guesses_correctly()
		{
			_game.GuessNext(Guess.Higher);

			_game.Score.Should().Be(1);
			_game.IsFinished.Should().BeFalse();
			_game.IsComplete.Should().BeFalse();
		}

		[Fact]
		public void Completes_when_player_guesses_correctly_ten_times()
		{
			for (var i = 0; i < 10; i++)
				_game.GuessNext(Guess.Higher);

			_game.Score.Should().Be(10);
			_game.IsFinished.Should().BeTrue();
			_game.IsComplete.Should().BeTrue();

			var func = new Func<bool>(() => _game.GuessNext(Guess.Higher));
			func.Should().ThrowExactly<InvalidOperationException>();
		}
	}
}
