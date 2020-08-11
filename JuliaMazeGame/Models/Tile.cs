using JuliaMazeGame.Models.Enums;

namespace JuliaMazeGame.Models.Base
{
    public class Tile : Location
    {
        public TileType Type { get; set; } = TileType.Empty;
      
        public const int Dimension = 40; //pixels
    }
}