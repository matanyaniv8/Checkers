using System;
using System.Collections.Generic;
using System.Linq;
using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class is a wrapper for random move generator given a Checkers Board.
    /// It collects all the Player's troops and generates all the possible moves 
    /// Like forward, backwards, Diagonal if the players eats an opponent troop.
    /// </summary>
    
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
                { "UpLeft", new BoardCell((int)eDirection.Up, (int)eDirection.Left, null, PlayerSign.second) },
                {"UpRight", new BoardCell((int)eDirection.Up, (int)eDirection.Right, null, PlayerSign.second) },
        };

        private Dictionary<string, BoardCell> r_PossibleMoveDirections = new Dictionary<string, BoardCell>
        {
            {"Forward", new BoardCell((int)eDirection.Up, 0, null, PlayerSign.second)},
            {"Backward", new BoardCell((int)eDirection.Down, 0, null, PlayerSign.second)},
            {"DoubleForward", new BoardCell((int)eDirection.StartRowUp, 0, null, PlayerSign.second)},
            {"DoubleBackward", new BoardCell((int)eDirection.Down, 0, null , PlayerSign.second) }
        };

        internal RandomMoveGenerator(Player i_ComputerPlayer, Board i_GameBoard)
        {
            r_CheckersBoard = i_GameBoard;
            ComputerPlayer = i_ComputerPlayer;
        }
        
        internal KeyValuePair<BoardCell, BoardCell> GenerateRandomMove()
        {
            List<BoardCell> potenailsCandidates = getAllLocationOfPlayerTroops();
            bool isRandomPointFound = false;
            BoardCell randomStartPoint = new BoardCell(0, 0,null, ComputerPlayer.PlayerSign);
            BoardCell randomTargetPoint = randomStartPoint;
            int randomIndex;

            while (!isRandomPointFound)
            {
                randomIndex = r_RandomIndexGenerator.Next(0, potenailsCandidates.Count);
                randomStartPoint = potenailsCandidates[randomIndex];
                randomTargetPoint = genrateRandomPointGivenAPoint(randomStartPoint);

                if (randomStartPoint != randomTargetPoint)
                {
                    randomStartPoint.CheckersReleventInfo = new CheckersBoardCell(ComputerPlayer.PlayerSign);
                    isRandomPointFound = true;
                }
            }

            return new KeyValuePair<BoardCell, BoardCell>(randomStartPoint, randomTargetPoint);
        }

        private List<BoardCell> getAllLocationOfPlayerTroops()
        {
            List<BoardCell> potenailsCandidates = new List<BoardCell>();
            int boardSize =r_CheckersBoard.BoardSize;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (r_CheckersBoard.GameBoard[i, j].CellSign == ComputerPlayer.PlayerSign)
                    {
                        potenailsCandidates.Add(new BoardCell(i, j, null, ComputerPlayer.PlayerSign));
                    }
                }
            }

            return potenailsCandidates;
        }

        private BoardCell genrateRandomPointGivenAPoint(BoardCell i_currentPoint)
        {
            List<BoardCell> possibleMoves = getPossibleEatMoves(i_currentPoint);
            int randomIndex = 0;
            BoardCell returningPoint = i_currentPoint;

            possibleMoves = possibleMoves.Concat(getPossibleMovingMoves(i_currentPoint)).ToList();
            
            if(possibleMoves.Count != 0)
            {
                randomIndex = r_RandomIndexGenerator.Next(possibleMoves.Count);
                returningPoint = possibleMoves[randomIndex];
            }

            returningPoint = new BoardCell(returningPoint.X, returningPoint.Y, returningPoint.CheckersReleventInfo, ComputerPlayer.PlayerSign);

            return returningPoint;
        }

        private List<BoardCell> getPossibleEatMoves(BoardCell i_currentPoint)
        {
            List<BoardCell> possibleMoves = new List<BoardCell>();
            BoardCell returningPoint;
            int xAxis = 0;
            int yAxis = 0;

            foreach (BoardCell DiagonalDiretionPoint in r_PossibleComputerEatDirections.Values)
            {
                try
                {
                    yAxis = (int)(DiagonalDiretionPoint.Y + i_currentPoint.Y);
                    xAxis = (int)(DiagonalDiretionPoint.X + i_currentPoint.X);
                    returningPoint = new BoardCell(xAxis, yAxis, null, ComputerPlayer.PlayerSign);

                    if (r_CheckersBoard.IsSlotEmpty(ComputerPlayer, returningPoint) &&
                         r_CheckersBoard.GameBoard[xAxis, yAxis].CellSign != PlayerSign.empty)
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

        private List<BoardCell> getPossibleMovingMoves(BoardCell i_currentPoint)
        {
            List<BoardCell> possibleMoves = new List<BoardCell>();
            BoardCell returningPoint = i_currentPoint;
            PlayerSign currentCellSign = PlayerSign.empty;
            int xAxis = 0;
            int yAxis = 0;

            foreach (BoardCell DiagonalDiretionPoint in r_PossibleMoveDirections.Values)
            {
                try
                {
                    yAxis = (int)(DiagonalDiretionPoint.Y + i_currentPoint.Y);
                    xAxis = (int)(DiagonalDiretionPoint.X + i_currentPoint.X);
                    returningPoint = new BoardCell(xAxis, yAxis, null, ComputerPlayer.PlayerSign);
                    currentCellSign = r_CheckersBoard.GameBoard[xAxis, yAxis].CellSign;

                    if (r_CheckersBoard.IsSlotEmpty(ComputerPlayer, returningPoint) && currentCellSign == PlayerSign.empty)
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
