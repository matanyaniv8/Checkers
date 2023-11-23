using System;
using System.Collections.Generic;
using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;

namespace ChceckersLogicComponents
{
    public class Board
    { 
        private const string k_LocationNotDiagonallyError = "Next Location is not Diagonally To Your Current Selected Troop";
        private const string k_IndicesNotInRangeError = "Indices Are Not In Range";
        private const int k_DefualtBoardSize = 8;
        private readonly PlayerSign[,] r_Board = null;
        private readonly List<int> r_RowsNumbersForADoubleMove = null;
        public PlayerSign[,] GameBoard { get { return r_Board; } }
        public bool IsFirstPlayerTurn { get; private set; }
        public int BoardSize { get; private set; }
        public bool IsSecondPlayerComputer {  get; private set; }

        public Board(bool i_IsSecondPlayerIsHuman, int i_BoardSize = k_DefualtBoardSize)
        {
            BoardSize = i_BoardSize;
            r_Board = new PlayerSign[BoardSize, BoardSize];
            IsSecondPlayerComputer = i_IsSecondPlayerIsHuman;
            r_RowsNumbersForADoubleMove = new List<int>(){0,2, BoardSize -3, BoardSize -1};
        }

        internal bool MakeAMove(Player i_PlayerThatMakesAMove, Player i_SecondPlayer,BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;
            bool isPlayerTryMoveBackwardOrForward = IsPlayerTryingToMoveForwardOrBackward(i_PlayerThatMakesAMove.PlayerSign, i_CurrentTroopLocation, i_NextTroopLocation);
            
            if (!isPlayerTryMoveBackwardOrForward)
            { 
                if(IsNextLocationIsDiagonalToCurrentLocation(i_CurrentTroopLocation, i_NextTroopLocation)
                    && 
                    getPlayerSignFromBoardCell(i_NextTroopLocation) != PlayerSign.empty)
                {
                    isPossibleToMakeAMove = makeMoveAndUpdatesPlayers(i_PlayerThatMakesAMove, i_SecondPlayer, i_CurrentTroopLocation, i_NextTroopLocation);
                }
                else
                {
                    throw new Exception(k_LocationNotDiagonallyError);
                }
            }
            else if(IsSlotEmpty(i_PlayerThatMakesAMove, i_NextTroopLocation) && isPlayerTryMoveBackwardOrForward)
            {
                isPossibleToMakeAMove = makeTheMove(i_PlayerThatMakesAMove, i_CurrentTroopLocation, i_NextTroopLocation);
            }


            return isPossibleToMakeAMove;
        }

        private PlayerSign getPlayerSignFromBoardCell(BoardCell i_CellLocation)
        {
            return GameBoard[(int)i_CellLocation.X, (int)i_CellLocation.Y];
        }

        internal bool IsPlayerTryingToMoveForwardOrBackward(PlayerSign i_PlayerThatMakesTheMove, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveForward = isMovingForwardAllowed(i_PlayerThatMakesTheMove,i_CurrentTroopLocation, i_NextTroopLocation);
            bool isTryingToMoveBackwards = isMovingBackwardAllowed(i_PlayerThatMakesTheMove, i_CurrentTroopLocation, i_NextTroopLocation);
            bool isNextCellLocationSignIsEmpty = getPlayerSignFromBoardCell(i_NextTroopLocation) == PlayerSign.empty;
            bool isCurrentPlayerSlotIsNotEmpty = getPlayerSignFromBoardCell(i_CurrentTroopLocation) != PlayerSign.empty;


            return (isTryingToMoveForward || isTryingToMoveBackwards) && isNextCellLocationSignIsEmpty && isCurrentPlayerSlotIsNotEmpty;
        }
        
        private bool isMovingBackwardAllowed(PlayerSign i_PlayerThatMakesTheMoveSign, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveBackward = i_NextTroopLocation.Y == i_CurrentTroopLocation.Y;
            int bound4MovingBackwards = 3;

            if(isTryingToMoveBackward == true)
            {
                if (i_PlayerThatMakesTheMoveSign == PlayerSign.first)
                {
                    bound4MovingBackwards = BoardSize - 3;
                    isTryingToMoveBackward &= (i_CurrentTroopLocation.X + 1 == i_NextTroopLocation.X && i_CurrentTroopLocation.X < bound4MovingBackwards);
                }
                else if(i_PlayerThatMakesTheMoveSign == PlayerSign.second)
                {
                    bound4MovingBackwards = 3;
                    isTryingToMoveBackward &= (i_CurrentTroopLocation.X - 1 == i_NextTroopLocation.X && i_CurrentTroopLocation.X >= bound4MovingBackwards);
                }
            }

            return isTryingToMoveBackward;
        }

        private bool isMovingForwardAllowed(PlayerSign i_PlayerThatMakesTheMoveSign, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveForward = i_CurrentTroopLocation.Y == i_NextTroopLocation.Y;
            int currentRowIndex = i_CurrentTroopLocation.X;

            if (isTryingToMoveForward)
            {
                if(i_PlayerThatMakesTheMoveSign == PlayerSign.first)
                {
                    currentRowIndex = (r_RowsNumbersForADoubleMove.Contains(i_CurrentTroopLocation.X)) ? currentRowIndex - 1 : currentRowIndex ;
                    isTryingToMoveForward &= currentRowIndex == i_NextTroopLocation.X || currentRowIndex - 1 == i_NextTroopLocation.X;
                }
                else if(i_PlayerThatMakesTheMoveSign == PlayerSign.second)
                {
                    currentRowIndex = (r_RowsNumbersForADoubleMove.Contains(i_CurrentTroopLocation.X)) ? currentRowIndex + 1: currentRowIndex ;
                    isTryingToMoveForward &= currentRowIndex == i_NextTroopLocation.X || currentRowIndex +1 == i_NextTroopLocation.X;
                }
            }

            return isTryingToMoveForward;
        }

        internal bool IsNextLocationIsDiagonalToCurrentLocation(BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            PlayerSign currentCellSign = getPlayerSignFromBoardCell(i_CurrentLocation);
            PlayerSign candidateCellSign = getPlayerSignFromBoardCell(i_CandidatePoint);
            int horizontalDistance = (int)Math.Abs(i_CurrentLocation.X - i_CandidatePoint.X);
            int verticalDistance = (int)Math.Abs(i_CurrentLocation.Y - i_CandidatePoint.Y);
            bool isCellsTypeAreDifferent = currentCellSign != candidateCellSign && 
                currentCellSign != PlayerSign.empty && candidateCellSign != PlayerSign.empty;

            return (horizontalDistance == 1 && verticalDistance == 1 && isCellsTypeAreDifferent);
        }

        private bool makeTheMove(Player i_PlayerThatMakesAMove, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;

            try
            {
                r_Board[(int)i_CurrentTroopLocation.X, (int)i_CurrentTroopLocation.Y] = PlayerSign.empty;
                r_Board[(int)i_NextTroopLocation.X, (int)i_NextTroopLocation.Y] = i_PlayerThatMakesAMove.PlayerSign;
                isPossibleToMakeAMove = true;
            }
            catch (Exception)
            {
                isPossibleToMakeAMove = false;
            }

            return isPossibleToMakeAMove;
        }

        private bool makeMoveAndUpdatesPlayers(Player i_PlayerThatMakesAMove, Player i_SecondPlayer, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            int rowIndex = (int)i_CurrentTroopLocation.X;
            int colIndex = (int)i_CurrentTroopLocation.Y;

            if (r_Board[rowIndex, colIndex] == i_SecondPlayer.PlayerSign)
            {
                i_SecondPlayer.NumberOfTroopsRemaining--;
            }

            return makeTheMove(i_PlayerThatMakesAMove, i_CurrentTroopLocation, i_NextTroopLocation);
        }

        internal bool IsIndicesWithinRange(int i_RowIndex, int i_ColIndex)
        {
            bool isRowOk = i_RowIndex < BoardSize && i_RowIndex >= 0;
            bool isColOk = i_ColIndex < BoardSize && i_ColIndex >= 0;

            return isRowOk && isColOk;
        }

        internal bool IsSlotEmpty(Player i_CurrentPlayer,BoardCell i_CandidatePoint)
        {
            bool isSlotEmpty = false;

            if(IsIndicesWithinRange((int)i_CandidatePoint.X, (int)i_CandidatePoint.Y))
            {
                isSlotEmpty = r_Board[(int)i_CandidatePoint.X, (int)i_CandidatePoint.Y] != i_CurrentPlayer.PlayerSign;
            }
            else
            {
                throw new Exception(k_IndicesNotInRangeError);
            }

            return isSlotEmpty;
        }
    }
}
