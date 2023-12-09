using System;
using System.Collections.Generic;
using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class Represents a Checkers Board, and manages all the moves, win checks.
    /// This is class is the actual doer (Facade) and we want to hide from our users.
    /// </summary>

    internal class Board
    {
        private const int k_NumberOfRowsToPutTroopsInBoard = 3;
        private const string k_LocationNotDiagonallyError = "Next Location is not Diagonally To Your Current Selected Troop";
        private const string k_IndicesNotInRangeError = "Indices Are Not In Range";
        private const int k_DefualtBoardSize = 8;
        private readonly CheckersBoardCell[,] r_Board = null;
        private readonly List<int> r_RowsNumbersForADoubleMove = null; // review
        private readonly MovesManager r_MovesManager = null;
        public CheckersBoardCell[,] GameBoard { get { return r_Board; } }
        public bool IsFirstPlayerTurn { get; private set; }
        public int BoardSize { get; private set; }
        public bool IsSecondPlayerComputer {  get; private set; }
        public MovesManager MovesManager { get { return r_MovesManager; } }

        public Board(bool i_IsSecondPlayerIsHuman, int i_BoardSize = k_DefualtBoardSize)
        {
            BoardSize = i_BoardSize;
            r_Board = new CheckersBoardCell[BoardSize, BoardSize];
            r_MovesManager = new MovesManager(this);
            IsSecondPlayerComputer = i_IsSecondPlayerIsHuman;
            r_RowsNumbersForADoubleMove = new List<int>(){0,2, BoardSize -3, BoardSize -1};
        }

        internal void InitializeBoard(PlayerSign i_FirstPlayerSign, PlayerSign i_SecondPlayerSign)
        {
            int bottomDownIndex = 0;

            for(int rowIndex = 0; rowIndex < BoardSize; rowIndex++) 
            {
                for(int columnIndex = 0; columnIndex < BoardSize; columnIndex++)
                {
                    r_Board[rowIndex, columnIndex] = new CheckersBoardCell(PlayerSign.empty, false);
                }
            }

            for (int upDownIndex = 0; upDownIndex < k_NumberOfRowsToPutTroopsInBoard; upDownIndex++)
            {
                bottomDownIndex = BoardSize - upDownIndex - 1;
                initializeARow(upDownIndex, i_SecondPlayerSign);
                initializeARow(bottomDownIndex, i_FirstPlayerSign);
            }
        }

        private void initializeARow(int i_RowIndex, PlayerSign i_GamePlayerSign)
        {
            for (int colIndex = (i_RowIndex % 2 != 0) ? 0 : 1; colIndex <BoardSize; colIndex += 2)
            {
                GameBoard[i_RowIndex, colIndex] = new CheckersBoardCell(i_GamePlayerSign, false);
            }
        }

        internal CheckersBoardCell GetPlayerCellFromBoard(BoardCell i_CellLocation)
        {
            CheckersBoardCell boardCell = new CheckersBoardCell();

            if (IsIndicesWithinRange(i_CellLocation.X, i_CellLocation.Y))
            {
                boardCell = GameBoard[(int)i_CellLocation.X, (int)i_CellLocation.Y];
            }
            else
            {
                throw new Exception(k_IndicesNotInRangeError);
            }

            return boardCell;
        }

        internal void SetPlayerCellInGameBoard(BoardCell i_CellLocation, CheckersBoardCell i_CellInfoToInsert)
        {
            try
            {
                GameBoard[i_CellLocation.X, i_CellLocation.Y] = i_CellInfoToInsert;
            }
            catch
            {
                throw new ArgumentException(k_IndicesNotInRangeError);
            }
        }

        internal bool MakeAMove(Player i_PlayerThatMakesAMove, Player i_SecondPlayer, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            return MovesManager.MakeMove(i_PlayerThatMakesAMove, i_SecondPlayer, i_CurrentTroopLocation, i_NextTroopLocation);
        }

        internal bool IsIndicesWithinRange(int i_RowIndex, int i_ColIndex)
        {
            bool isRowOk = i_RowIndex < BoardSize && i_RowIndex >= 0;
            bool isColOk = i_ColIndex < BoardSize && i_ColIndex >= 0;

            return isRowOk && isColOk;
        }

        internal bool IsIndicesWithinRange(BoardCell i_Coords)
        {
            return IsIndicesWithinRange(i_Coords.X, i_Coords.Y);
        }

        internal bool IsSlotEmpty(Player i_CurrentPlayer, BoardCell i_CandidatePoint)
        {
            bool isSlotEmpty = false;

            if (IsIndicesWithinRange((int)i_CandidatePoint.X, (int)i_CandidatePoint.Y))
            {
                isSlotEmpty = r_Board[(int)i_CandidatePoint.X, (int)i_CandidatePoint.Y].CellSign != i_CurrentPlayer.PlayerSign;
            }
            else
            {
                throw new Exception(k_IndicesNotInRangeError);
            }

            return isSlotEmpty;
        }

        /*internal bool IsPlayerTryingToMoveForwardOrBackward(PlayerSign i_PlayerThatMakesTheMove, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveForward = isMovingForwardAllowed(i_PlayerThatMakesTheMove,i_CurrentTroopLocation, i_NextTroopLocation);
            bool isTryingToMoveBackwards = isMovingBackwardAllowed(i_PlayerThatMakesTheMove, i_CurrentTroopLocation, i_NextTroopLocation);
            bool isNextCellLocationSignIsEmpty = GetPlayerCellFromBoard(i_NextTroopLocation).CellSign == PlayerSign.empty;
            bool isCurrentPlayerSlotIsNotEmpty = GetPlayerCellFromBoard(i_CurrentTroopLocation).CellSign != PlayerSign.empty;

            return (isTryingToMoveForward || isTryingToMoveBackwards) && isNextCellLocationSignIsEmpty && isCurrentPlayerSlotIsNotEmpty;
        }
        
        private bool isMovingBackwardAllowed(PlayerSign i_PlayerThatMakesTheMoveSign, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveBackward = i_NextTroopLocation.Y == i_CurrentTroopLocation.Y && isCurrentTroopAKing(i_CurrentTroopLocation);
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
        }*/

        /*private bool isMovingForwardAllowed(PlayerSign i_PlayerThatMakesTheMoveSign, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isTryingToMoveForward = i_CurrentTroopLocation.Y == i_NextTroopLocation.Y && isCurrentTroopAKing(i_CurrentTroopLocation);
            bool isCurrentPlayerAKing = *//*r_RowsNumbersForADoubleMove.Contains(i_CurrentTroopLocation.X) && *//*isCurrentTroopAKing(i_CurrentTroopLocation);
            int currentRowIndex = i_CurrentTroopLocation.X;

            if (isTryingToMoveForward)
            {
                if(i_PlayerThatMakesTheMoveSign == PlayerSign.first)
                {
                    //currentRowIndex = (isCurrentPlayerAKing) ? currentRowIndex - 1 : currentRowIndex ;
                    isTryingToMoveForward &= (*//*currentRowIndex == i_NextTroopLocation.X ||*//* currentRowIndex - 1 == i_NextTroopLocation.X);
                }
                else if(i_PlayerThatMakesTheMoveSign == PlayerSign.second)
                {
                    //currentRowIndex = (isCurrentPlayerAKing) ? currentRowIndex + 1: currentRowIndex ;
                    isTryingToMoveForward &= *//*currentRowIndex == i_NextTroopLocation.X ||*//* currentRowIndex +1 == i_NextTroopLocation.X;
                }
            }

            return isTryingToMoveForward && isCurrentPlayerAKing;
        }

        private bool isCurrentTroopAKing(BoardCell i_CurrentMovingTroop)
        {
            bool isTroopAKing = false;

            try
            {
                isTroopAKing = GameBoard[i_CurrentMovingTroop.X, i_CurrentMovingTroop.Y].IsCellPlayerAKing;
            }
            catch
            {
                isTroopAKing = false;
            }

            return isTroopAKing;
        }

        internal bool IsNextLocationIsDiagonalToCurrentLocation(BoardCell i_CurrentLocation, BoardCell i_CandidatePoint)
        {
            PlayerSign currentCellSign = GetPlayerCellFromBoard(i_CurrentLocation).CellSign;
            PlayerSign candidateCellSign = GetPlayerCellFromBoard(i_CandidatePoint).CellSign;
            int horizontalDistance = (int)Math.Abs(i_CurrentLocation.X - i_CandidatePoint.X);
            int verticalDistance = (int)Math.Abs(i_CurrentLocation.Y - i_CandidatePoint.Y);
            bool isCellsTypeAreDifferent = currentCellSign != candidateCellSign && 
                currentCellSign != PlayerSign.empty && candidateCellSign != PlayerSign.empty;

            return (horizontalDistance == 1 && verticalDistance == 1 && isCellsTypeAreDifferent);
        }*/


        /*internal bool MakeAMove(Player i_PlayerThatMakesAMove, Player i_SecondPlayer, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;
            bool isPlayerTryMoveBackwardOrForward = IsPlayerTryingToMoveForwardOrBackward(i_PlayerThatMakesAMove.PlayerSign, i_CurrentTroopLocation, i_NextTroopLocation);

            if (!isPlayerTryMoveBackwardOrForward)
            {
                if (IsNextLocationIsDiagonalToCurrentLocation(i_CurrentTroopLocation, i_NextTroopLocation)
                    &&
                    GetPlayerCellFromBoard(i_NextTroopLocation).CellSign != PlayerSign.empty)
                {
                    isPossibleToMakeAMove = makeMoveAndUpdatesPlayers(i_PlayerThatMakesAMove, i_SecondPlayer, i_CurrentTroopLocation, i_NextTroopLocation);
                }
                else
                {
                    throw new Exception(k_LocationNotDiagonallyError);
                }
            }
            else if (IsSlotEmpty(i_PlayerThatMakesAMove, i_NextTroopLocation) && isPlayerTryMoveBackwardOrForward)
            {
                isPossibleToMakeAMove = makeMoveAndUpdatesPlayers(i_PlayerThatMakesAMove, i_SecondPlayer, i_CurrentTroopLocation, i_NextTroopLocation);
            }

            return isPossibleToMakeAMove;
            return MovesManager.MakeMove(i_PlayerThatMakesAMove, i_SecondPlayer, i_CurrentTroopLocation, i_NextTroopLocation);
        }

        private bool makeTheMove(Player i_PlayerThatMakesAMove, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            bool isPossibleToMakeAMove = false;

            try
            {
                r_Board[(int)i_CurrentTroopLocation.X, (int)i_CurrentTroopLocation.Y].CellSign = PlayerSign.empty;
                r_Board[(int)i_CurrentTroopLocation.X, (int)i_CurrentTroopLocation.Y].IsCellPlayerAKing = false;
                r_Board[(int)i_NextTroopLocation.X, (int)i_NextTroopLocation.Y].CellSign = i_PlayerThatMakesAMove.PlayerSign;

                if (i_NextTroopLocation.X == 0 || i_NextTroopLocation.X == BoardSize - 1 && i_NextTroopLocation != i_CurrentTroopLocation)
                {
                    r_Board[(int)i_NextTroopLocation.X, (int)i_NextTroopLocation.Y].IsCellPlayerAKing = true;
                }

                isPossibleToMakeAMove = true;
            }
            catch (Exception)
            {
                isPossibleToMakeAMove = false;
            }

            return isPossibleToMakeAMove;
        }*/

/*        private bool makeMoveAndUpdatesPlayers(Player i_PlayerThatMakesAMove, Player i_SecondPlayer, BoardCell i_CurrentTroopLocation, BoardCell i_NextTroopLocation)
        {
            int rowIndex = (int)i_NextTroopLocation.X;
            int colIndex = (int)i_NextTroopLocation.Y;

            if (r_Board[rowIndex, colIndex].CellSign == i_SecondPlayer.PlayerSign)
            {
                i_SecondPlayer.NumberOfTroopsRemaining--;
            }

            return makeTheMove(i_PlayerThatMakesAMove, i_CurrentTroopLocation, i_NextTroopLocation);
        }*/
    }
}
