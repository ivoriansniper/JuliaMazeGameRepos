using System;
using System.Collections.Generic;
using JuliaMazeGame.Models.Base;
using JuliaMazeGame.Models.Enums;

namespace JuliaMazeGame.Models
{
    public class Room : Location
    {
        public const int Dimension = 10; //tiles

        public const double DoorCellSize = 2.5;

        public Tile[][] RoomGrid { get; set; } = GridGenerator.RandomRoomGenerator();

        public Doors Doors { get; } = new Doors();

        public bool HasNoDoors => !(Doors.Top || Doors.Bottom || Doors.Left || Doors.Right);

        public static Random TheRandom { get; } = new Random();

        public int NumberOfDoors { get; private set; }

        public void CreateDoors(Room otherRoom)
        {
            if (otherRoom.X == this.X + 1)
            {
                this.Doors.Right = true;
                otherRoom.Doors.Left = true;
            }

            if (otherRoom.X == this.X - 1)
            {
                this.Doors.Left = true;
                otherRoom.Doors.Right = true;
            }

            if (otherRoom.Y == this.Y + 1)
            {
                this.Doors.Bottom = true;
                otherRoom.Doors.Top = true;
            }

            if (otherRoom.Y == this.Y - 1)
            {
                this.Doors.Top = true;
                otherRoom.Doors.Bottom = true;
            }

            this.NumberOfDoors++;
            otherRoom.NumberOfDoors++;
        }

        public List<Tile> TilesByDoor(Direction direction)
        {
            const int middleDimension = Dimension / 2;
            if (direction == Direction.Right && Doors.Right)
                return new List<Tile>
                {
                    RoomGrid[Dimension - 1][middleDimension - 1],
                    RoomGrid[Dimension - 1][middleDimension],
                };
            else if (direction == Direction.Up && Doors.Top)
                return new List<Tile>
                {
                    RoomGrid[middleDimension - 1][0],
                    RoomGrid[middleDimension][0],
                };
            else if (direction == Direction.Down && Doors.Bottom)
                return new List<Tile>
                {
                    RoomGrid[middleDimension - 1][Dimension - 1],
                    RoomGrid[middleDimension][Dimension - 1],
                };
            else if (direction == Direction.Left && Doors.Left)
                return new List<Tile>
                {
                    RoomGrid[0][middleDimension - 1],
                    RoomGrid[0][middleDimension],
                };
            else
                return new List<Tile>();
        }

        public Tile GetNewTile(Player player, Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    return RoomGrid[player.X + 1][player.Y];
                case Direction.Left:
                    return RoomGrid[player.X - 1][player.Y];
                case Direction.Up:
                    return RoomGrid[player.X][player.Y - 1];
                case Direction.Down:
                    return RoomGrid[player.X][player.Y + 1];
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
