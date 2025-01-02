using System;

namespace Snippets.Data;

internal static class Mapping
{
	public record Game(int Id, string Name, string Genre, decimal Price);
	
	public static class GameMapper
	{
		public static GameDto ToDto(this Game game)
		{
			return new GameDto(
				game.Id,
				game.Name,
				game.Genre,
				game.Price
			);
		}
	}
}
