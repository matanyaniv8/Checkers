using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChceckersLogicComponents
{
    public class Board
    { 
        public enum eDirection
        {
            Down = 1,
            Up = -1,
            Left = -1,
            Right = 1,
        }

        private const string k_SlotIsTakenError = "Selected Location Is Already Taken!";
        private const string k_LocationNotDiagonallyError = "Next Location is not Diagonally To Your Current Selected Troop";
        private const string k_IndicesNotInRangeError = "Indices Are Not In Range";
        private const int k_DefualtBoardSize = 8;
        private readonly GameUtilities.ePlayerSign[,] r_Board = null;
        private Dictionary<string, BoardCell> r_PossibleComputerDirections = new Dictionary<string, BoardCell>
        {
            { "UpLeft", new BoardCell((int)eDirection.Up, (int)eDirection.Left) },
            {"UpRight", new BoardCell((int)eDirection.Up, (int)eDirection.Right) },
            {"DownLeft", new BoardCell((int)eDirection.Down, (int)eDirection.Left) },
            {"DownRight", new BoardCell((int)eDirection.Down, (int)eDirection.Right) }
        };
        public  GameUtilities.ePlayerSign[,] GameBoard { get { return r_Board; } }
        public bool IsFirstPlayerTurn { get; private set; }
        public int BoardSize { get; private set; }
        public bool IsSecondPlayerComputer {  get; private set; }

        public Board(bool i_IsSecondPlayerIsHuman, int i_BoardSize = k_DefualtBoardSize)
        {
            BoardSize = i_BoardSize;
            r_Board = new GameUtilities.ePlayerSign[BoardSize, BoardSize];
            IsSecondPlayerComputer = i_IsSecondPlayerIsHuman;
            
        }

        internal bool MakeAMove(Player i_PlayerThatMakesAMove, Player i_SecondPlayer,BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;
            
            if(!isPlayerTryingToMoveForwardOrBackward(i_CurrentTroopLocation, i_NextTroopLocation))
            { 
                if(isNextLocationIsDiagonalToCurrentLocation(i_CurrentTroopLocation, i_NextTroopLocation))
                {
                    if (isSlotEmpty(i_PlayerThatMakesAMove, i_NextTroopLocation))
                    {
                       isPossibleToMakeAMove = makeMoveAndUpdatesPlayers(i_PlayerThatMakesAMove, i_SecondPlayer, i_CurrentTroopLocation, i_NextTroopLocation);
                    }
                    else
                    {
                        throw new Exception(k_SlotIsTakenError);
                    }
                }
                else
                {
                    throw new Exception(k_LocationNotDiagonallyError);
                }
            }
            else if(isSlotEmpty(i_PlayerThatMakesAMove, i_NextTroopLocation))
            {
                isPossibleToMakeAMove = makeTheMove(i_PlayerThatMakesAMove, i_CurrentTroopLocation, i_NextTroopLocation);
            }


            return isPossibleToMakeAMove;
        }

        private GameUtilities.ePlayerSign getPlayerSignFromBoardCell(BoardCell i_CellLocation)
        {
            return GameBoard[(int)i_CellLocation.X, (int)i_CellLocation.Y];
        }

        private bool isPlayerTryingToMoveForwardOrBackward(BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveBackward = i_NextTroopLocation.Y == i_CurrentTroopLocation.Y && (i_CurrentTroopLocation.X - 1 == i_NextTroopLocation.X || i_CurrentTroopLocation.X + 1 == i_NextTroopLocation.X);
            bool isNextCellLocationSignIsEmpty = getPlayerSignFromBoardCell(i_NextTroopLocation) == GameUtilities.ePlayerSign.empty;
            bool isCurrentPlayerSlotIsNotEmpty = getPlayerSignFromBoardCell(i_CurrentTroopLocation) != GameUtilities.ePlayerSign.empty;

            return isTryingToMoveBackward && isNextCellLocationSignIsEmpty && isCurrentPlayerSlotIsNotEmpty;
        }

        private bool makeTheMove(Player i_PlayerThatMakesAMove, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;
            
            r_Board[(int)i_CurrentTroopLocation.X, (int)i_CurrentTroopLocation.Y] = GameUtilities.ePlayerSign.empty;
            r_Board[(int)i_NextTroopLocation.X, (int)i_NextTroopLocation.Y] = i_PlayerThatMakesAMove.PlayerSign;
            isPossibleToMakeAMove = true;

            return isPossibleToMakeAMove;
        }
        private bool makeMoveAndUpdatesPlayers(Player i_PlayerThatMakesAMove, Player i_SecondPlayer, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;
            int rowIndex = (int)i_CurrentTroopLocation.X;
            int colIndex = (int)i_CurrentTroopLocation.Y;

            if(i_PlayerThatMakesAMove.PlayerType == GameUtilities.ePlayersType.Computer)
            {
                i_NextTroopLocation = GenerateRandomMove(i_PlayerThatMakesAMove);
            }

            if (r_Board[rowIndex, colIndex] == i_SecondPlayer.PlayerSign)
            {
                i_SecondPlayer.NumberOfTroopsRemaining--;
            }

            isPossibleToMakeAMove = makeTheMove(i_PlayerThatMakesAMove,i_CurrentTroopLocation, i_NextTroopLocation);
/*
            r_Board[rowIndex, colIndex] = GameUtilities.ePlayerSign.empty;
            r_Board[(int)i_NextTroopLocation.X, (int)i_NextTroopLocation.Y] = i_PlayerThatMakesAMove.PlayerSign;
            isPossibleToMakeAMove = true;*/

            return isPossibleToMakeAMove;
        }

        private bool isIndicesWithinRange(int i_RowIndex, int i_ColIndex)
        {
            bool isRowOk = i_RowIndex < BoardSize && i_RowIndex >= 0;
            bool isColOk = i_ColIndex < BoardSize && i_ColIndex >= 0;

            return isRowOk && isColOk;
        }

        private bool isSlotEmpty(Player i_CurrentPlayer,BoardCell i_CandidatePoint)
        {
            bool isSlotEmpty = false;

            if(isIndicesWithinRange((int)i_CandidatePoint.X, (int)i_CandidatePoint.Y))
            {
                isSlotEmpty = r_Board[(int)i_CandidatePoint.X, (int)i_CandidatePoint.Y] != i_CurrentPlayer.PlayerSign;
            }
            else
            {
                throw new Exception(k_IndicesNotInRangeError);
            }

            return isSlotEmpty;
        }

        private bool isNextLocationIsDiagonalToCurrentLocation(BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            int horizontalDistance = (int)Math.Abs(i_CurrentLocation.X - i_CandidatePoint.X);
            int verticalDistance = (int)Math.Abs(i_CurrentLocation.Y - i_CandidatePoint.Y);

            return (horizontalDistance == 1 && verticalDistance == 1);
        }

        public BoardCell GenerateRandomMove(Player i_ComputerPlayer)
        {
            List<BoardCell> potenailsCandidates = getAllLocationOfPlayerTroops(i_ComputerPlayer);
            Random randomIndexGenerator = new Random();
            bool isRandomPointFound = false;
            BoardCell randomPoint = new BoardCell(0,0);
            int randomIndex;

            while (!isRandomPointFound)
            {
                randomIndex = randomIndexGenerator.Next(0,potenailsCandidates.Count);
                randomPoint = genrateRandomPointGivenAPoint(potenailsCandidates[randomIndex], i_ComputerPlayer);

                if(randomPoint != potenailsCandidates[randomIndex])
                {
                    isRandomPointFound = true;
                }
            }

            return randomPoint;
        }

        private List<BoardCell> getAllLocationOfPlayerTroops(Player i_ComputerPlayer)
        {
            List<BoardCell> potenailsCandidates = new List<BoardCell>();

            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (r_Board[i, j] == i_ComputerPlayer.PlayerSign)
                    {
                        potenailsCandidates.Add(new BoardCell(i, j));
                    }
                }
            }

            return potenailsCandidates;
        }

        private BoardCell genrateRandomPointGivenAPoint(BoardCell i_currentPoint, Player i_ComputerPlayer)
        {
            BoardCell returningPoint = i_currentPoint;
            double xAxis = 0;
            double yAxis = 0;

            foreach(BoardCell DiagonalDiretionPoint in r_PossibleComputerDirections.Values)
            {
                try
                {
                    yAxis = DiagonalDiretionPoint.Y + i_currentPoint.Y;
                    xAxis = DiagonalDiretionPoint.X + i_currentPoint.X;
                    returningPoint = new BoardCell((int)xAxis, (int)yAxis);

                    if(isSlotEmpty(i_ComputerPlayer, returningPoint))
                    {
                        break;
                    }
                }
                catch(Exception)
                {
                    returningPoint = i_currentPoint; 
                }
            }

            return returningPoint;
        }
    }
}
