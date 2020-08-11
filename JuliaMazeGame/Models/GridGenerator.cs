using System.Collections.Generic;
using JuliaMazeGame.Models.Base;
using JuliaMazeGame.Models.Enums;

namespace JuliaMazeGame.Models
{
    public static class GridGenerator
    {
        public static Tile[][] RandomRoomGenerator()
        {
            var randomNum = Room.TheRandom.Next(ExampleGrids().Count);
            return ExampleGrids()[randomNum];
        }

        public static Tile[][] GetEmptyRoomGrid()
        {
            var emptyGrid = new Tile[Room.Dimension][];
            for (var x = 0; x < Room.Dimension; x++)
            {
                emptyGrid[x] = new Tile[Room.Dimension];
                for (var y = 0; y < Room.Dimension; y++)
                {
                    emptyGrid[x][y] = new Tile { X = x, Y = y };
                }
            }

            return emptyGrid;
        }

        public static Tile[][] GetFinishRoom()
        {
            var grid = GetEmptyRoomGrid();
            grid[4][4].Type = TileType.Finish;
            grid[4][5].Type = TileType.Finish;
            grid[5][4].Type = TileType.Finish;
            grid[5][5].Type = TileType.Finish;
            return grid;
        }

        private static void EmptyTilesByDoors(Tile[][] grid)
        {
            var doorTiles = new List<Tile>();
            const int middleDimension = Room.Dimension / 2;
            doorTiles.AddRange
            (new[]
                {
                    grid[Room.Dimension - 1][middleDimension - 1],
                    grid[Room.Dimension - 1][middleDimension],
                    grid[middleDimension - 1][0],
                    grid[middleDimension][0],
                    grid[middleDimension - 1][Room.Dimension - 1],
                    grid[middleDimension][Room.Dimension - 1],
                    grid[0][middleDimension - 1],
                    grid[0][middleDimension],
                }
            );

            doorTiles.ForEach(tile => tile.Type = TileType.Empty);
        }
        private static List<Tile[][]> ExampleGrids()
        {
            const int maxGridIndex = Room.Dimension - 1;

            #region Grid1
            var grid1 = GetEmptyRoomGrid();
            grid1[1][1].Type = TileType.Treasure;
            grid1[maxGridIndex - 1][1].Type = TileType.Treasure;
            grid1[maxGridIndex - 1][maxGridIndex - 1].Type = TileType.Treasure;
            grid1[1][maxGridIndex - 1].Type = TileType.Treasure;
            #endregion

            #region Grid2
            var grid2 = GetEmptyRoomGrid();
            for (var i = 1; i < 9; i++)
            {
                grid2[i][1].Type = TileType.Wall;
                grid2[i][8].Type = TileType.Wall;
                grid2[1][i].Type = TileType.Wall;
                grid2[8][i].Type = TileType.Wall;
            }
            grid2[1][5].Type = TileType.Empty;
            grid2[5][1].Type = TileType.Empty;
            grid2[8][5].Type = TileType.Empty;
            grid2[5][8].Type = TileType.Empty;
            grid2[4][4].Type = TileType.Treasure;
            grid2[4][5].Type = TileType.Enemy;
            grid2[5][4].Type = TileType.Enemy;
            grid2[5][5].Type = TileType.Treasure;
            #endregion

            #region Grid3
            var grid3 = GetEmptyRoomGrid();
            for (var i = 1; i < 9; i++)
            {
                grid3[i][1].Type = TileType.Enemy;
                grid3[i][8].Type = TileType.Enemy;
                grid3[1][i].Type = TileType.Enemy;
                grid3[8][i].Type = TileType.Enemy;
            }
            grid3[1][5].Type = TileType.Empty;
            grid3[5][1].Type = TileType.Empty;
            grid3[8][5].Type = TileType.Empty;
            grid3[5][8].Type = TileType.Empty;
            for (var i = 2; i < 8; i++)
            {
                TileType type;
                if (i % 2 == 1)
                {
                    type = TileType.Treasure;
                }
                else
                {
                    type = TileType.Enemy;
                }

                grid3[i][i].Type = type;
            }
            grid3[2][2].Type = TileType.Treasure;
            grid3[2][7].Type = TileType.Treasure;
            grid3[7][2].Type = TileType.Treasure;
            grid3[7][7].Type = TileType.Treasure;


            #endregion

            #region Grid4
            var grid4 = GetEmptyRoomGrid();
            for (var x = 1; x < 9; x++)
            {
                if (x % 2 == 1)
                {
                    foreach (var tile in grid4[x])
                    {
                        tile.Type = TileType.Enemy;
                    }

                    if (x == 1 || x == 5)
                    {
                        grid4[x][0].Type = TileType.Empty;
                    }
                    else
                    {
                        grid4[x][8].Type = TileType.Empty;
                    }
                }
                else
                {
                    grid4[x][3].Type = TileType.Treasure;
                    grid4[x][7].Type = TileType.Treasure;
                }
            }
            #endregion

            var exampleGridList =  new List<Tile[][]>
            {
                grid1,
                grid2,
                grid3,
                grid4,
            };
            exampleGridList.ForEach(EmptyTilesByDoors);
            return exampleGridList;
        }
    }
}