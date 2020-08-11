using System;
using System.Threading.Tasks;
using JuliaMazeGame.Models.Enums;
using JuliaMazeGame.Services.Interfaces;
using Microsoft.JSInterop;

namespace JuliaMazeGame.Services
{
    public class UserInputHandler : IUserInputHandler
    {
        private readonly IGameStateHandler _gameStateHandler;
        private readonly IMazeRenderer _mazeRenderer;

        public UserInputHandler(IGameStateHandler gameStateHandler, IMazeRenderer mazeRenderer)
        {
            _gameStateHandler = gameStateHandler;
            _mazeRenderer = mazeRenderer;
        }

        [JSInvokable]
        public async Task OnKeyPress(int e)
        {
            var found = false;
            var consoleKey = default(ConsoleKey);
            try
            {
                consoleKey = (ConsoleKey) e;
                found = true;
            }
            catch
            {
                Console.WriteLine($"Couldn't find Key for key value {e}.");
            }

            if (found)
            {
                HandleValidKeyInput(consoleKey);
            }
        }

        public void StartGameButtonPress()
        {
            _gameStateHandler.Reset();
            _mazeRenderer.RenderMazeAsync();
        }
        private void HandleValidKeyInput(ConsoleKey key)
        {
            Direction direction;
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    direction = Direction.Right;
                    break;
                case ConsoleKey.UpArrow:
                    direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    direction = Direction.Down;
                    break;
                default:
                    return;
            }

            if (_gameStateHandler.IsRunning)
            {
                _gameStateHandler.MovePlayer(direction);
                _mazeRenderer.RenderMazeAsync();
            }
        }
    }
}
