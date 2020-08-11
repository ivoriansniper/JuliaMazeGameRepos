using System;
using System.Collections.Generic;
using System.Linq;
using JuliaMazeGame.Models.Base;
using JuliaMazeGame.Models.Enums;


namespace JuliaMazeGame.Models
{
    public class Maze
    {
        private readonly int _dimension;
        private Room[][] _mazeGrid;
        private readonly Stack<Room> _roomStack = new Stack<Room>();

        public Maze(int dimension)
        {   
            _dimension = dimension;
            // Create Room Grid
            CreateMazeGridNoDoors();
            CreateDoorsBetweenRooms();
            SetFinishRoom();
            SetStartRoom();
        }

        public Room CurrentRoom { get; set; }

        public void MoveToNewRoom(Direction direction)
        {
            switch (direction)
            {
                case Direction.Right:
                    CurrentRoom = _mazeGrid[CurrentRoom.X + 1][CurrentRoom.Y];
                    break;
                case Direction.Left:
                    CurrentRoom = _mazeGrid[CurrentRoom.X - 1][CurrentRoom.Y];
                    break;
                case Direction.Up:
                    CurrentRoom = _mazeGrid[CurrentRoom.X][CurrentRoom.Y - 1];
                    break;
                case Direction.Down:
                    CurrentRoom = _mazeGrid[CurrentRoom.X][CurrentRoom.Y + 1];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private void CreateMazeGridNoDoors()
        {
            _mazeGrid = new Room[_dimension][];
            for (var x = 0; x < _dimension; x++)
            {
                _mazeGrid[x] = new Room[_dimension];
                for (var y = 0; y < _dimension; y++)
                {
                    _mazeGrid[x][y] = new Room { X = x, Y = y };
                }
            }
        }
        private void CreateDoorsBetweenRooms() 
        {
            CurrentRoom = _mazeGrid[0][0];
            var visitedRooms = 1;
            var totalRooms = _dimension * _dimension;
            while (visitedRooms < totalRooms)
            {
                var adjacentRooms = GetAdjacentWithNoDoors(CurrentRoom);
                if (adjacentRooms.Count > 0)
                {
                    var randomNum = Room.TheRandom.Next(adjacentRooms.Count);
                    var theRoom = adjacentRooms[randomNum];
                    CurrentRoom.CreateDoors(theRoom);
                    _roomStack.Push(CurrentRoom);
                    CurrentRoom = theRoom;
                    visitedRooms++;
                }
                else
                {
                    CurrentRoom = _roomStack.Pop();
                }
            }
        }

        private List<Room> GetAdjacentWithNoDoors(Room currentRoom)
        {
            var adjacentRooms = new List<Room>();
            
            
            if (currentRoom.X != 0)
            {
                var left = _mazeGrid[currentRoom.X - 1][currentRoom.Y];
                adjacentRooms.Add(left);
            }

            if (currentRoom.X != _dimension-1)
            {
                var right = _mazeGrid[currentRoom.X + 1][currentRoom.Y];
                adjacentRooms.Add(right);
            }

            if (currentRoom.Y != 0)
            {
                var top = _mazeGrid[currentRoom.X][currentRoom.Y - 1];
                adjacentRooms.Add(top);
            }

            if (currentRoom.Y != _dimension-1)
            {
                var bottom = _mazeGrid[currentRoom.X][currentRoom.Y + 1];
                adjacentRooms.Add(bottom);
            }

            //has to have no doors
            return adjacentRooms.Where((room) => room.HasNoDoors).ToList();
        }

        private void SetFinishRoom()
        {           
            CurrentRoom.RoomGrid = GridGenerator.GetFinishRoom();
        }

        private void SetStartRoom()
        {            
            _mazeGrid[0][0].RoomGrid = GridGenerator.GetEmptyRoomGrid();
            CurrentRoom = _mazeGrid[0][0];
        }
    }
}