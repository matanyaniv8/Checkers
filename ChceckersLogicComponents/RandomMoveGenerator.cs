using System;
using System.Collections.Generic;
using System.Linq;

namespace ChceckersLogicComponents
{
    internal enum eDirection
    {
        Down = -1,
        Up = 1,
        StartRowUp = 2,
        Left = -1,
        Right = 1,
    }

    internal class RandomMoveGenerator
    {
        private readonly Board r_CheckersBoard;
        private readonly Random r_RandomIndexGenerator = new Random();
        internal Player ComputerPlayer {  get; private set; }
        
        private Dictionary<string, BoardCell> r_PossibleComputerEatDirections = new Dictionary<string, BoardCell>
        {
                { "UpLeft", new BoardCell((int)eDirection.Up, (int)eDirection.Left) },
                {"UpRight", new BoardCell((int)eDirection.Up, (int)eDirection.Right) },
        };

        private Dictionary<string, BoardCell> r_PossibleMoveDirections = new Dictionary<string, BoardCell>
        {
            {"Forward", new BoardCell((int)eDirection.Up, 0)},
            {"Backward", new BoardCell((int)eDirection.Down, 0)},
            {"DoubleForward", new BoardCell((int)eDirection.StartRowUp, 0)}
        };

        internal RandomMoveGenerator(Player i_ComputerPlayer, Board i_GameBoard)
        {
            r_CheckersBoard = i_GameBoard;
            ComputerPlayer = i_ComputerPlayer;
        }
        
        internal KeyValuePair<BoardCell, BoardCell> GenerateRandomMove()
        {
            List<BoardCell> potenailsCandidates = getAllLocationOfPlayerTroops(ComputerPlayer);
            bool isRandomPointFound = false;
            BoardCell randomStartPoint = new BoardCell(0, 0, ComputerPlayer.PlayerSign);
            BoardCell randomTargetPoint = randomStartPoint;
            int randomIndex;

            while (!isRandomPointFound)
            {
                randomIndex = r_RandomIndexGenerator.Next(0, potenailsCandidates.Count);
                randomStartPoint = potenailsCandidates[randomIndex];
                randomTargetPoint = genrateRandomPointGivenAPoint(randomStartPoint, ComputerPlayer);

                if (randomStartPoint != randomTargetPoint)
                {
                    randomStartPoint.PlayerSign = ComputerPlayer.PlayerSign;
                    isRandomPointFound = true;
                }
            }

            return new KeyValuePair<BoardCell, BoardCell>(randomStartPoint, randomTargetPoint);
        }

        private List<BoardCell> getAllLocationOfPlayerTroops(Player i_ComputerPlayer)
        {
            List<BoardCell> potenailsCandidates = new List<BoardCell>();
            int boardSize =r_CheckersBoard.BoardSize;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (r_CheckersBoard.GameBoard[i, j] == i_ComputerPlayer.PlayerSign)
                    {
                        potenailsCandidates.Add(new BoardCell(i, j, GameUtilities.ePlayerSign.second));
                    }
                }
            }

            return potenailsCandidates;
        }

        private BoardCell genrateRandomPointGivenAPoint(BoardCell i_currentPoint, Player i_ComputerPlayer)
        {
            List<BoardCell> possibleMoves = getPossibleEatMoves(i_currentPoint, i_ComputerPlayer);
            int randomIndex = 0;
            BoardCell returningPoint = i_currentPoint;

            possibleMoves = possibleMoves.Concat(getPossibleMovingMoves(i_currentPoint, i_ComputerPlayer)).ToList();
            
            if(possibleMoves.Count != 0)
            {
                randomIndex = r_RandomIndexGenerator.Next(possibleMoves.Count);
                returningPoint = possibleMoves[randomIndex];
            }

            returningPoint.PlayerSign = i_ComputerPlayer.PlayerSign;

            return returningPoint;
        }

        private List<BoardCell> getPossibleEatMoves(BoardCell i_currentPoint, Player i_ComputerPlayer)
        {
            List<BoardCell> possibleMoves = new List<BoardCell>();
            BoardCell returningPoint = i_currentPoint;
            double xAxis = 0;
            double yAxis = 0;

            foreach (BoardCell DiagonalDiretionPoint in r_PossibleComputerEatDirections.Values)
            {
                try
                {
                    yAxis = DiagonalDiretionPoint.Y + i_currentPoint.Y;
                    xAxis = DiagonalDiretionPoint.X + i_currentPoint.X;
                    returningPoint = new BoardCell((int)xAxis, (int)yAxis);

                    if (r_CheckersBoard.IsSlotEmpty(i_ComputerPlayer, returningPoint) &&
                         r_CheckersBoard.GameBoard[(int)xAxis, (int)yAxis] != GameUtilities.ePlayerSign.empty)
                    {
                        possibleMoves.Add(returningPoint);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return possibleMoves;
        }

        private List<BoardCell> getPossibleMovingMoves(BoardCell i_currentPoint, Player i_ComputerPlayer)
        {
            List<BoardCell> possibleMoves = new List<BoardCell>();
            BoardCell returningPoint = i_currentPoint;
            GameUtilities.ePlayerSign currentCellSign = GameUtilities.ePlayerSign.empty;
            double xAxis;
            double yAxis;

            foreach (BoardCell DiagonalDiretionPoint in r_PossibleMoveDirections.Values)
            {
                try
                {
                    yAxis = DiagonalDiretionPoint.Y + i_currentPoint.Y;
                    xAxis = DiagonalDiretionPoint.X + i_currentPoint.X;
                    returningPoint = new BoardCell((int)xAxis, (int)yAxis);
                    currentCellSign = r_CheckersBoard.GameBoard[(int)xAxis, (int)yAxis];
                    bool checkIsEmpty = r_CheckersBoard.IsSlotEmpty(i_ComputerPlayer, returningPoint);
                    if (checkIsEmpty == true && 
                        currentCellSign == GameUtilities.ePlayerSign.empty)
                    {
                        possibleMoves.Add(returningPoint);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return possibleMoves;
        }
    }
}
