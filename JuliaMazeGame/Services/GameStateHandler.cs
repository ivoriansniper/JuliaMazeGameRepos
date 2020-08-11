using System;
using JuliaMazeGame.Models;
using JuliaMazeGame.Models.Base;
using JuliaMazeGame.Models.Enums;
using JuliaMazeGame.Services.Interfaces;
using Newtonsoft.Json;

namespace JuliaMazeGame.Services
{
    public class GameStateHandler : IGameStateHandler
    {
        public Player Player { get; set; } = new Player();

        public Maze Maze { get; set; } = new Maze(3);
        public bool IsRunning { get; set; }

        public bool GameFinished { get; set; }
        public bool Win { get; set; }

        public void Reset()
        {
            IsRunning = true;
            GameFinished = false;
            Player.Reset();
            Maze = new Maze(3);
        }

        public void MovePlayer(Direction direction)
        {
            // Check if moving to a new room
            var doorTiles = Maze.CurrentRoom.TilesByDoor(direction);
            var currentTile = Maze.CurrentRoom.RoomGrid[Player.X][Player.Y];
            if (doorTiles.Contains(currentTile))
            {
                // Move To New Room
                Maze.MoveToNewRoom(direction);
                Player.NewRoom(direction);
            }
            else
            {
                var tileToMoveInto = Maze.CurrentRoom.GetNewTile(Player, direction);
                if (TryMovePlayer(tileToMoveInto))
                {
                    // Move Player
                    Player.MovePlayer(direction);
                }
            }
        }

        private bool TryMovePlayer(Tile tile)
        {
            switch (tile.Type)
            {
                case TileType.Empty:
                    return true;
                case TileType.Treasure:
                    Player.Score++;
                    tile.Type = TileType.Empty;
                    return true;
                case TileType.Finish:
                    IsRunning = false;
                    GameFinished = true;
                    Win = true;
                    return true;
                case TileType.Enemy:
                    Player.Health--;
                    if (Player.Health == 0)
                    {
                        Win = false;
                        GameFinished = true;
                        IsRunning = false;
                        return false;
                        
                    }
                    tile.Type = TileType.Empty;
                    return true;
                case TileType.Wall:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}