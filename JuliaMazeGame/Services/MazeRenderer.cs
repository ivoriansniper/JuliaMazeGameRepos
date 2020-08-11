using System;
using System.Threading.Tasks;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using JuliaMazeGame.Models;
using JuliaMazeGame.Models.Base;
using JuliaMazeGame.Models.Enums;
using JuliaMazeGame.Services.Interfaces;

namespace JuliaMazeGame.Services
{
    public class MazeRenderer : IMazeRenderer
    {
        private readonly IGameStateHandler _gameStateHandler;
        public const int Padding = 30;
        private const int ScoreAreaWidth = 250;
        private const int RoomGridDimension = Room.Dimension * Tile.Dimension;
        private const double DoorSize = Room.DoorCellSize * Tile.Dimension;
        private const double FinishScreenHeight = 200;
        private const double FinishScreenWidth = 300;
        
        public MazeRenderer(IGameStateHandler gameStateHandler)
        {
            _gameStateHandler = gameStateHandler;
        }

        private Canvas2DContext _context;
        public BECanvasComponent CanvasRef { get; set; }

        private double GameHeight { get; } = RoomGridDimension + (2 * Padding);
        private double GameWidth { get; } = RoomGridDimension + (2 * Padding);
        public int CanvasWidth => (int)GameWidth + ScoreAreaWidth;

        public int CanvasHeight => (int)GameHeight;
        public async Task RenderMazeAsync()
        {
            _context = await CanvasRef.CreateCanvas2DAsync();
            await _context.BeginBatchAsync();
            await PrintRoomWallsAsync();
            await PrintCurrentRoomGridAsync();
            await PrintPlayerAsync();
            await PrintScoreAreaAsync();
            if (_gameStateHandler.GameFinished)
            {
                await _context.SetFillStyleAsync("white");
                await _context.FillRectAsync((GameWidth - FinishScreenWidth) / 2, (GameHeight - FinishScreenHeight) / 2, FinishScreenWidth, FinishScreenHeight);
                await _context.SetStrokeStyleAsync("black");
                await _context.SetLineWidthAsync(5);
                await _context.StrokeRectAsync((GameWidth - FinishScreenWidth) / 2, (GameHeight - FinishScreenHeight) / 2, FinishScreenWidth, FinishScreenHeight);
                if (_gameStateHandler.Win)
                {
                    await PrintWinningTextAsync();
                }
                else
                {
                    await PrintLosingTextAsync();
                }
            }
            await _context.EndBatchAsync();
        }

        private async Task PrintLosingTextAsync()
        {
            await _context.SetFontAsync("30px arial");
            await _context.SetFillStyleAsync("red");
            await _context.FillTextAsync("You Lose!", (GameWidth - FinishScreenWidth) / 2 + 50, (GameHeight - FinishScreenHeight) / 2 + FinishScreenHeight / 3, FinishScreenWidth);
            await _context.FillTextAsync(":(", (GameWidth - FinishScreenWidth) / 2 + 50, (GameHeight - FinishScreenHeight) / 2 + (2 * FinishScreenHeight / 3), FinishScreenWidth);

        }

        private async Task PrintWinningTextAsync()
        {
            await _context.SetFontAsync("30px arial");
            await _context.SetFillStyleAsync("black");
            await  _context.FillTextAsync("You Win!", (GameWidth - FinishScreenWidth) / 2 + 50, (GameHeight - FinishScreenHeight) / 2 + FinishScreenHeight / 3, FinishScreenWidth);
            await  _context.FillTextAsync($"Score: {_gameStateHandler.Player.Score}", (GameWidth - FinishScreenWidth) / 2 + 50, (GameHeight - FinishScreenHeight) / 2 + (2 * FinishScreenHeight / 3), FinishScreenWidth);
        }

        private async Task PrintScoreAreaAsync()
        {
            await _context.SetFillStyleAsync("#cdcdcd");
            await _context.FillRectAsync(GameWidth, 0, ScoreAreaWidth, CanvasHeight);
            await _context.SetFillStyleAsync("black");
            await _context.SetFontAsync("48px arial");
            await _context.FillTextAsync("Score", GameWidth + 50, 100, ScoreAreaWidth);
            await _context.SetFillStyleAsync("#f5ad00");
            await _context.SetFontAsync("120px arial");
            await _context.FillTextAsync(_gameStateHandler.Player.Score.ToString(), GameWidth + 50, (double)CanvasHeight / 2, ScoreAreaWidth);
            await _context.SetFillStyleAsync("red");
            await _context.FillTextAsync(_gameStateHandler.Player.Health.ToString(), GameHeight + 50, (double)CanvasHeight - 30, ScoreAreaWidth);
        }
        private async Task PrintPlayerAsync()
        {
            await _context.SetFillStyleAsync("blue");
            await _context.FillRectAsync((_gameStateHandler.Player.X * Tile.Dimension) + Padding, (_gameStateHandler.Player.Y * Tile.Dimension) + Padding, Tile.Dimension, Tile.Dimension);
        }

        private async Task PrintCurrentRoomGridAsync()
        {
            foreach (var tileColumn in _gameStateHandler.Maze.CurrentRoom.RoomGrid)
            {
                foreach (var tile in tileColumn)
                {
                    var pixelX = tile.X * Tile.Dimension + Padding;
                    var pixelY = tile.Y * Tile.Dimension + Padding;
                    switch (tile.Type)
                    {
                        case TileType.Empty:
                            break;
                        case TileType.Treasure:
                            await _context.BeginPathAsync();
                            await _context.ArcAsync(pixelX + Tile.Dimension / 2, pixelY + Tile.Dimension / 2, Tile.Dimension / 2.5, 0, 2 * Math.PI, false);
                            await _context.SetFillStyleAsync("#f5ad00");
                            await _context.FillAsync();
                            await _context.SetLineWidthAsync(4);
                            await _context.SetStrokeStyleAsync("#2F2F31");
                            await _context.StrokeAsync();
                            break;
                        case TileType.Finish:
                            await _context.SetFillStyleAsync("#228b22");
                            await _context.FillRectAsync(pixelX, pixelY, Tile.Dimension, Tile.Dimension);
                            break;
                        case TileType.Enemy:
                            await _context.SetFillStyleAsync("red");
                            await _context.FillRectAsync(pixelX, pixelY, Tile.Dimension, Tile.Dimension);
                            break;
                        case TileType.Wall:
                            await _context.SetFillStyleAsync("black");
                            await _context.FillRectAsync(pixelX, pixelY, Tile.Dimension, Tile.Dimension);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private async Task PrintRoomWallsAsync()
        {
            await _context.SetFillStyleAsync("black");
            await _context.FillRectAsync(0, 0, GameWidth, GameHeight);
            await _context.ClearRectAsync(Padding, Padding, RoomGridDimension, RoomGridDimension);
            var doors = _gameStateHandler.Maze.CurrentRoom.Doors;
            if (doors.Left)
            {
                await _context.ClearRectAsync(0, GameHeight/2 - (DoorSize / 2), Padding, DoorSize);
            }

            if (doors.Right)
            {
                await _context.ClearRectAsync(GameWidth - Padding, GameHeight / 2 - DoorSize / 2, Padding, DoorSize);
            }

            if (doors.Top)
            {
                await _context.ClearRectAsync(GameWidth / 2 - DoorSize / 2, 0, DoorSize, Padding);
            }

            if (doors.Bottom)
            {
                await _context.ClearRectAsync(GameWidth / 2 -DoorSize / 2, GameHeight-Padding, DoorSize, Padding);
            }
        }
    }
}