using JuliaMazeGame.Models;
using JuliaMazeGame.Models.Enums;

namespace JuliaMazeGame.Services.Interfaces
{
    public interface IGameStateHandler
    {
       Player Player { get; set; }

       Maze Maze { get; set; }

       bool IsRunning { get; set; }

       bool GameFinished { get; set; }
       bool Win { get; set; }

       public void Reset();

       void MovePlayer(Direction direction);
    }
}