using System;
using JuliaMazeGame.Models.Base;
using JuliaMazeGame.Models.Enums;

namespace JuliaMazeGame.Models
{
    public class Player : Location
    {
        public void Reset()
        {
            X = 5;
            Y = 5;
            Score = 0;
            Health = 5;
        }

        public int Score { get; set; }
        public int Health { get; set; } = 5;

        public void MovePlayer(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (X > 0)
                    {
                        X--;
                    }
                    break;
                case Direction.Right:
                    if (X < Room.Dimension - 1)
                    {
                        X++;
                    }
                    break;
                case Direction.Up:
                    if (Y > 0)
                    {
                        Y--;
                    }
                    break;
                case Direction.Down:
                    if (Y < Room.Dimension - 1)
                    {
                        Y++;
                    }
                    break;
                default: return;
            }
        }

        public void NewRoom(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    X = 0;
                    break;
                case Direction.Left:
                    X = Room.Dimension - 1;
                    break;
                case Direction.Up:
                    Y = Room.Dimension - 1;
                    break;
                case Direction.Down:
                    Y = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}