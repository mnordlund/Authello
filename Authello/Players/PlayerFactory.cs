using System;
using System.Linq;

namespace Authello.Players
{
    class PlayerFactory
    {
        public static IPlayer[] ListAllIPlayers()
        {
            var type = typeof(IPlayer);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface).ToArray();

            return types.Select(t =>
                (IPlayer)Activator.CreateInstance(t)).ToArray();
        }
    }
}
