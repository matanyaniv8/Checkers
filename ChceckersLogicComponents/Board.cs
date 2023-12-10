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
        private const string k_IndicesNotInRangeError = "Indices Are Not In Range";
        private const int k_DefualtBoardSize = 8;
        private readonly CheckersBoardCell[,] r_Board = null;
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
    }
}
