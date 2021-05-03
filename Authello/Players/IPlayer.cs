namespace Authello.Players
{
    interface IPlayer
    {
        public Player Player { get; set; }
        public string PlayerName { get; }
        public string PlayerDescription { get; }
        public (int X, int Y) MakeMove(Board board);

        
    }
}
