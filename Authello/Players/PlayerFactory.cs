using System;
using System.Linq;

namespace Authello.Players
{
    class PlayerFactory
    {
        private static IPlayer[] ListAllIPlayers()
        {
            var type = typeof(IPlayer);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface).ToArray();

            return types.Select(t =>
                (IPlayer)Activator.CreateInstance(t)).ToArray();
        }

        private static void ToArray()
        {
            throw new NotImplementedException();
        }

        public static IPlayer GetPlayer(Tile player)
        {
            var players = ListAllIPlayers();

            for(int i = 0; i< players.Length; i++)
            {
                Console.WriteLine($"{i}. {players[i].PlayerDescription}");
            }
            Console.Write($"Choose {player} player: ");
            var selection = int.Parse(Console.ReadLine());

            players[selection].Player = player;

            return players[selection];
        }
    }
}
