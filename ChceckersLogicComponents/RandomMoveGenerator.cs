using System;
using System.Collections.Generic;
using System.Linq;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class is a wrapper for random move generator given a Checkers Board.
    /// It collects all the Player's troops and generates all the possible moves.
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
        private readonly Board r_CheckersBoard = null;
        private readonly Random r_RandomIndexGenerator = new Random();

        internal Player ComputerPlayer {  get; private set; }
        
        private Dictionary<string, BoardCell> r_PossibleComputerEatDirections = new Dictionary<string, BoardCell>
        {
                {"UpLeft", new BoardCell((int) eDirection.Up * 2,(int) eDirection.Left * 2) },
                {"UpRight", new BoardCell((int) eDirection.Up * 2,(int) eDirection.Right * 2) },
        };

        private Dictionary<string, BoardCell> r_PossibleMoveDirections = new Dictionary<string, BoardCell>
        {
            {"ForwardLeft", new BoardCell((int) eDirection.Up,(int) eDirection.Left)},
            {"ForwardRight", new BoardCell((int) eDirection.Down,(int) eDirection.Right)}
        };

        private readonly Dictionary<string, BoardCell> r_PossibleKingComputerEatingMoves = new Dictionary<string, BoardCell>
        {
            {"DownLeft", new BoardCell((int)eDirection.Down * 2, (int)eDirection.Left * 2)},
            {"DownRight", new BoardCell((int)eDirection.Down * 2, (int)eDirection.Right *2)}
        };

        private Dictionary<string, BoardCell> r_PossibleComputerNonEatingMoves = new Dictionary<string, BoardCell>
        {
            {"BackwardLeft", new BoardCell((int)eDirection.Down, (int)eDirection.Left)},
            {"BackwardRight", new BoardCell((int) eDirection.Down,(int) eDirection.Right)}
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

            while (!isRandomPointFound && potenailsCandidates.Count != 0)
            {
                randomIndex = r_RandomIndexGenerator.Next(0, potenailsCandidates.Count);
                randomStartPoint = potenailsCandidates[randomIndex];
                potenailsCandidates.RemoveAt(randomIndex);
                randomTargetPoint = genrateRandomPointGivenAPoint(randomStartPoint);

                if (randomStartPoint != randomTargetPoint)
                {
                    isRandomPointFound = true;
                }
            }

            return new KeyValuePair<BoardCell, BoardCell>(randomStartPoint, randomTargetPoint);
        }

        private List<BoardCell> getAllLocationOfPlayerTroops()
        {
            List<BoardCell> potenailsCandidates = new List<BoardCell>();
            int boardSize =r_CheckersBoard.BoardSize;
            BoardCell troopLocation;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (r_CheckersBoard.GameBoard[i, j].CellSign == ComputerPlayer.PlayerSign)
                    {
                        troopLocation = new BoardCell(i, j);
                        troopLocation.CheckersReleventInfo = r_CheckersBoard.GetPlayerCellFromBoard(troopLocation);
                        potenailsCandidates.Add(troopLocation);
                    }
                }
            }

            return potenailsCandidates;
        }

        private BoardCell genrateRandomPointGivenAPoint(BoardCell i_currentPoint)
        {
            List<BoardCell> possibleEatingMoves = getPossibleEatMoves(i_currentPoint);
            List<BoardCell> possibleMoves = getPossibleMovingMoves(i_currentPoint);
            int randomIndex = 0;
            BoardCell returningPoint = i_currentPoint;

            possibleMoves = possibleMoves.Concat(possibleEatingMoves).ToList();
            
            if(possibleMoves.Count != 0)
            {
                randomIndex = r_RandomIndexGenerator.Next(possibleMoves.Count);
                returningPoint = possibleMoves[randomIndex];
            }

            return returningPoint;
        }

        private List<BoardCell> getPossibleEatMoves(BoardCell i_currentPoint)
        {
            bool isCurrentCellAKingTroop = r_CheckersBoard.MovesManager.IsCurrentTroopAKing(i_currentPoint);
            List<BoardCell> possibleDirection = isCurrentCellAKingTroop ? r_PossibleComputerEatDirections.Values.Concat(r_PossibleKingComputerEatingMoves.Values).ToList() : r_PossibleComputerEatDirections.Values.ToList();
            List<BoardCell> possibleMoves = new List<BoardCell>();
            BoardCell returningPoint = i_currentPoint;
            int xAxis = 0;
            int yAxis = 0;
            int listCount = possibleDirection.Count;

            foreach (BoardCell DiagonalDiretionPoint in possibleDirection)
            {
                try
                {
                    yAxis = DiagonalDiretionPoint.Y + i_currentPoint.Y;
                    xAxis = DiagonalDiretionPoint.X + i_currentPoint.X;
                    returningPoint = new BoardCell(xAxis, yAxis, null, ComputerPlayer.PlayerSign);
                    returningPoint.CheckersReleventInfo = r_CheckersBoard.GetPlayerCellFromBoard (returningPoint);

                    if(r_CheckersBoard.MovesManager.IsPlayerMoveAnEatMove(ComputerPlayer, i_currentPoint, returningPoint))
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
            bool isCurrentCellAKingTroop = r_CheckersBoard.MovesManager.IsCurrentTroopAKing(i_currentPoint);
            List<BoardCell> possibleDirection = isCurrentCellAKingTroop ? r_PossibleMoveDirections.Values.Concat(r_PossibleComputerNonEatingMoves.Values).ToList() : r_PossibleMoveDirections.Values.ToList();
            List<BoardCell> possibleMoves = new List<BoardCell>();
            BoardCell returningPoint = i_currentPoint;
            int xAxis = 0;
            int yAxis = 0;

            foreach (BoardCell DiagonalDiretionPoint in possibleDirection)
            {
                try
                {
                    yAxis = DiagonalDiretionPoint.Y + i_currentPoint.Y;
                    xAxis = DiagonalDiretionPoint.X + i_currentPoint.X;
                    returningPoint = new BoardCell(xAxis, yAxis);
                    returningPoint.CheckersReleventInfo = r_CheckersBoard.GetPlayerCellFromBoard(returningPoint);

                    if(r_CheckersBoard.MovesManager.IsCurrentMoveIsMovingWithoutEating(ComputerPlayer, i_currentPoint, returningPoint))
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
