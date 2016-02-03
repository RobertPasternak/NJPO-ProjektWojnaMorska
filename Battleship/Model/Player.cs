using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Battleship.Model
{
    internal class Player
    {
        protected const int GRID_SIZE_X = 14;
        protected const int GRID_SIZE_Y_SHIPS = 11;
        protected const int GRID_SIZE_Y_VEHICLES = 11;
        protected const int GRID_SIZE_Y = 22;

        protected static Random rnd = new Random();

        public List<List<SeaSquare>> MyGrid { get; set; }
        public List<List<SeaSquare>> EnemyGrid { get; set; }

       

        private List<Ship> _myShips = new List<Ship>();
        private List<Ship> _enemyShips = new List<Ship>();

        private List<Vehicle> _myVehicles = new List<Vehicle>();
        private List<Vehicle> _enemyVehicles = new List<Vehicle>();
        private int _shots;
        private int _enemyUnits;

        public Player()
        {
            _shots = 0;
            _enemyUnits = 16;
            MyGrid = new List<List<SeaSquare>>();
            EnemyGrid = new List<List<SeaSquare>>();

            for (int i = 0; i != GRID_SIZE_Y; ++i)
            {
                MyGrid.Add(new List<SeaSquare>());
                EnemyGrid.Add(new List<SeaSquare>());

                for (int j = 0; j != GRID_SIZE_X; ++j)
                {
                    MyGrid[i].Add(new SeaSquare(i, j));
                    EnemyGrid[i].Add(new SeaSquare(i, j));
                }
            }

            foreach (ShipType type in Enum.GetValues(typeof (ShipType)))
            {
                _myShips.Add(new Ship(type));
                _enemyShips.Add(new Ship(type));
            }
 
             foreach (VehicleType type in Enum.GetValues(typeof (VehicleType)))
              {
                  _myVehicles.Add(new Vehicle(type));
                  _enemyVehicles.Add(new Vehicle(type));
              }
            
            Reset();
        }

        public void Reset()
        {
            for (int i = 0; i != GRID_SIZE_Y; ++i)
            {
                if (i < 11)
                {
                    for (int j = 0; j != GRID_SIZE_X; ++j)
                    {
                        MyGrid[i][j].Reset(SquareType.Water);
                        EnemyGrid[i][j].Reset(SquareType.Unknown);
                    }
                }
                else
                {
                    for (int j = 0; j != GRID_SIZE_X; ++j)
                    {
                        MyGrid[i][j].Reset(SquareType.Grass);
                        EnemyGrid[i][j].Reset(SquareType.Unknown);
                    }
                }
            }

            _myShips.ForEach(s => s.Reincarnate());
            _enemyShips.ForEach(s => s.Reincarnate());
            _myVehicles.ForEach(s => s.Reincarnate());
            _enemyVehicles.ForEach(s => s.Reincarnate());
            PlaceShips();
            PlaceVehicles();
        }

        private bool SquareFree(int row, int col)
        {
            return (MyGrid[row][col].SquareIndex == -1) ? true : false;
        }

        private bool CheckAdjacentSquaresForShips(int row, int col)
        {
            if (row-1>-1 && col-1>-1)
            {
                if (MyGrid[row - 1][col - 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row-1>-1)
            {
                if (MyGrid[row - 1][col].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row-1>-1 && col+1<14)
            {
                if (MyGrid[row - 1][col + 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (col-1>-1)
            {
                if (MyGrid[row][col - 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (col+1<14)
            {
                if (MyGrid[row][col + 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row+1<12&&col-1>-1)
            {
                if (MyGrid[row + 1][col - 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row+1<12)
            {
                if (MyGrid[row + 1][col].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row+1<12&&col+1<14)
            {
                if (MyGrid[row + 1][col + 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckAdjacentSquaresForVehicles(int row, int col)
        {
            if (row - 1 > 1 && col - 1 > -1)
            {
                if (MyGrid[row - 1][col - 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row - 1 > 11)
            {
                if (MyGrid[row - 1][col].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row - 1 > 11 && col + 1 < 14)
            {
                if (MyGrid[row - 1][col + 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (col - 1 > -1)
            {
                if (MyGrid[row][col - 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (col + 1 < 14)
            {
                if (MyGrid[row][col + 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row + 1 < 22 && col - 1 > -1)
            {
                if (MyGrid[row + 1][col - 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row + 1 < 22)
            {
                if (MyGrid[row + 1][col].SquareIndex != -1)
                {
                    return false;
                }
            }
            if (row + 1 < 22 && col + 1 < 14)
            {
                if (MyGrid[row + 1][col + 1].SquareIndex != -1)
                {
                    return false;
                }
            }
            return true;
        }
        
        private bool PlaceVerticalShip(int shipIndex, int remainingLength)
        {
            int startPosRow = rnd.Next(GRID_SIZE_Y_SHIPS - remainingLength);
            int startPosCol = rnd.Next(GRID_SIZE_X);

            Func<bool> PlacementPossible = () =>
            {
                int tmp = remainingLength;
                for (int row = startPosRow; tmp != 0; ++row)
                {
                    if (!SquareFree(row, startPosCol))
                        return false;
                    if (!CheckAdjacentSquaresForShips(row, startPosCol))
                        return false;
                    --tmp;
                }
                return true;
            };

            if (PlacementPossible())
            {
                for (int row = startPosRow; remainingLength != 0; ++row)
                {
                    MyGrid[row][startPosCol].Type = SquareType.Undamaged;
                    MyGrid[row][startPosCol].SquareIndex = shipIndex;
                    --remainingLength;
                }
                return true;
            }

            return false;
        }

        private bool PlaceHorizontalShip(int shipIndex, int remainingLength)
        {
            int startPosRow = rnd.Next(GRID_SIZE_Y_SHIPS);
            int startPosCol = rnd.Next(GRID_SIZE_X - remainingLength);

            Func<bool> PlacementPossible = () =>
            {
                int tmp = remainingLength;
                for (int col = startPosCol; tmp != 0; ++col)
                {
                    if (!SquareFree(startPosRow, col))
                        return false;
                    if (!CheckAdjacentSquaresForShips(startPosRow, col))
                        return false;
                    --tmp;
                }
                return true;
            };

            if (PlacementPossible())
            {
                for (int col = startPosCol; remainingLength != 0; ++col)
                {
                    MyGrid[startPosRow][col].Type = SquareType.Undamaged;
                    MyGrid[startPosRow][col].SquareIndex = shipIndex;
                    --remainingLength;
                }
                return true;
            }

            return false;
        }

        private bool PlaceVerticalVehicle(int vehicleIndex, int remainingLength)
        {
            int startPosRow = rnd.Next(GRID_SIZE_Y_VEHICLES - remainingLength) + 11;
            int startPosCol = rnd.Next(GRID_SIZE_X);

            Func<bool> PlacementPossible = () =>
            {
                int tmp = remainingLength;
                for (int row = startPosRow; tmp != 0; ++row)
                {
                    if (!SquareFree(row, startPosCol))
                        return false;
                    if(!CheckAdjacentSquaresForVehicles(row, startPosCol))
                        return false;
                    --tmp;
                }
                return true;
            };

            if (PlacementPossible())
            {
                for (int row = startPosRow; remainingLength != 0; ++row)
                {
                    MyGrid[row][startPosCol].Type = SquareType.Undamaged;
                    MyGrid[row][startPosCol].SquareIndex = vehicleIndex;
                    --remainingLength;
                }
                return true;
            }

            return false;
        }


        private bool PlaceHorizontalVehicle(int vehicleIndex, int remainingLength)
        {
            int startPosRow = rnd.Next(GRID_SIZE_Y_VEHICLES) + 11;
            int startPosCol = rnd.Next(GRID_SIZE_X - remainingLength);

            Func<bool> PlacementPossible = () =>
            {
                int tmp = remainingLength;
                for (int col = startPosCol; tmp != 0; ++col)
                {
                    if (!SquareFree(startPosRow, col))
                        return false;
                    if (!CheckAdjacentSquaresForVehicles(startPosRow, col))
                        return false;
                    --tmp;
                }
                return true;
            };

            if (PlacementPossible())
            {
                for (int col = startPosCol; remainingLength != 0; ++col)
                {
                    MyGrid[startPosRow][col].Type = SquareType.Undamaged;
                    MyGrid[startPosRow][col].SquareIndex = vehicleIndex;
                    --remainingLength;
                }
                return true;
            }

            return false;
        }
        
        private void PlaceShips()
        {
            bool startAgain = false;

            for (int i = 0; i != _myShips.Count && !startAgain; ++i)
            {
                bool vertical = Convert.ToBoolean(rnd.Next(2));
                bool placed = false;

                int loopCounter = 0;
                for (; !placed && loopCounter != 10000; ++loopCounter)
                {
                    int remainingLength = _myShips[i].Length;

                    if (vertical)
                        placed = PlaceVerticalShip(i, remainingLength);
                    else
                        placed = PlaceHorizontalShip(i, remainingLength);
                }

                if (loopCounter == 10000)
                    startAgain = true;
            }

            if (startAgain)
                PlaceShips();
        }

        private void PlaceVehicles()
        {
            bool startAgain = false;

            for (int i = 0; i != _myVehicles.Count && !startAgain; ++i)
            {
                bool vertical = Convert.ToBoolean(rnd.Next(2));
                bool placed = false;

                int loopCounter = 0;
                for (; !placed && loopCounter != 10000; ++loopCounter)
                {
                    int remainingLength = _myVehicles[i].Length;

                    if (vertical)
                        placed = PlaceVerticalVehicle(i, remainingLength);
                    else
                        placed = PlaceHorizontalVehicle(i, remainingLength);
                }

                if (loopCounter == 10000)
                    startAgain = true;
            }

            if (startAgain)
                PlaceVehicles();
        }
        
        private void DestroyItem(int i, List<List<SeaSquare>> grid)
        {
            foreach (var row in grid)
            {
                foreach (var square in row)
                {
                    if (square.SquareIndex == i)
                        square.Type = SquareType.Destroyed;
                }
            }
        }

        private void MineDestroyed(int i)
        {
            DestroyItem(i, MyGrid);
            _enemyUnits--;
        }

        public void EnemyDestroyed(int i)
        {
            DestroyItem(i, EnemyGrid);
            
        }


        protected void Fire(int row, int col, Player otherPlayer)
        {
            //incrase number of shots and set number of enemy units
            _shots++;
            MainWindow._mainWindow.UpdateTb(_shots, _enemyUnits);

            int damagedIndex;
            bool isDestroyed;
            SquareType newType = otherPlayer.FiredAt(row, col, out damagedIndex, out isDestroyed);
            EnemyGrid[row][col].SquareIndex = damagedIndex;

            if (isDestroyed)
                EnemyDestroyed(damagedIndex);
            else
                EnemyGrid[row][col].Type = newType;
        }

        public SquareType FiredAt(int row, int col, out int damagedIndex, out bool isDestroyed)
        {
            isDestroyed = false;
            damagedIndex = -1;


           /* if (MyGrid[row][col].Type == SquareType.Destroyed)
            {
                
            }*/

            switch (MyGrid[row][col].Type)
            {
                case SquareType.Water:
                    return SquareType.WaterHit;
                case SquareType.Grass:
                    return SquareType.GrassHit;
                case SquareType.Undamaged:
                    var square = MyGrid[row][col];
                    damagedIndex = square.SquareIndex;
                    if (_myShips[damagedIndex].FiredAt() || _myVehicles[damagedIndex].FiredAt())
                    {
                        MineDestroyed(square.SquareIndex);
                        isDestroyed = true;
                    }
                    else
                    {
                        square.Type = SquareType.Damaged;
                    }
                    return square.Type;
                case SquareType.Damaged:
                    goto default;
                case SquareType.Unknown:
                    goto default;
                case SquareType.Destroyed:
                    goto default;
                case SquareType.WaterHit:
                    goto default;
                case SquareType.GrassHit:
                    goto default;
                default:
                    throw new Exception("fail");
            }
        }

        public bool NoShipsSadFace()
        {
            return _myShips.All(ship => ship.IsDestroyed);
        }

        public bool NoVehiclesSadFace()
        {
            return _myVehicles.All(vehicle => vehicle.IsDestroyed);
        }

        public void TakeTurnAutomated(Player otherPlayer)
        {
            bool takenShot = false;
            while (!takenShot)
            {
                int row = rnd.Next(GRID_SIZE_Y);
                int col = rnd.Next(GRID_SIZE_X);

                if (EnemyGrid[row][col].Type == SquareType.Unknown)
                {
                    Fire(row, col, otherPlayer);
                    takenShot = true;
                }
            }
        }
    }
}
