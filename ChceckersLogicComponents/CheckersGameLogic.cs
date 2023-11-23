using System;
using System.Collections.Generic;

namespace ChceckersLogicComponents
{
    public class CheckersGameLogic
    {
        private RandomMoveGenerator r_RandomMoveGenerator = null;
        private const int k_NumberOfTroops = 12;
        private const int k_NumberOfRowsToPutTroopsInBoard = 3;
        private const string k_NotCorrectPlayerHasChosenMessage = "please choose a soldier from your troops";
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set;}
        public Board CheckersBoard { get; private set; }
        public Player CurrentPlayerTurn { get; private set; }
        public Player OpponentPlayer { get; private set; }

        public CheckersGameLogic(string i_FirstPlayerName, string i_SecondPlayerName="Computer", bool i_IsSecondPlayerHuman=false)
        {
            CheckersBoard = new Board(i_IsSecondPlayerHuman);
            FirstPlayer = new Player(i_FirstPlayerName,1 ,true, k_NumberOfTroops);
            FirstPlayer.PlayerSign = GameUtilities.ePlayerSign.first;
            SecondPlayer = new Player(i_SecondPlayerName, 2, i_IsSecondPlayerHuman, k_NumberOfTroops);
            SecondPlayer.PlayerSign = GameUtilities.ePlayerSign.second;
            CurrentPlayerTurn = FirstPlayer;
            OpponentPlayer = SecondPlayer;
            initializeBoard();

            if(!i_IsSecondPlayerHuman)
            {
                r_RandomMoveGenerator = new RandomMoveGenerator(SecondPlayer, CheckersBoard);
            }
        }

        private void initializeBoard()
        {
            int bottomDownIndex = 0;

            for(int upDownIndex = 0; upDownIndex < k_NumberOfRowsToPutTroopsInBoard; upDownIndex++)
            {
                bottomDownIndex = CheckersBoard.BoardSize - upDownIndex - 1;
                initializeARow(upDownIndex, SecondPlayer);
                initializeARow(bottomDownIndex, FirstPlayer);
            }
        }

        private void initializeARow(int i_RowIndex, Player i_GamePlayer)
        {
            for(int colIndex = (i_RowIndex % 2 != 0) ? 0 : 1; colIndex< CheckersBoard.BoardSize; colIndex+=2)
            {
                CheckersBoard.GameBoard[i_RowIndex, colIndex] = i_GamePlayer.PlayerSign;
            }
        }

        public Player MakeAMove(BoardCell i_CurrentPlayerTroopLocation, BoardCell i_SelectedLocationOnBoard )
        {
            Player lastPlayedPlayer = CurrentPlayerTurn;

            if (!IsThereAWin())
            {
                if (CurrentPlayerTurn.PlayerSign == i_CurrentPlayerTroopLocation.PlayerSign)
                {
                    CheckersBoard.MakeAMove(CurrentPlayerTurn, OpponentPlayer, i_CurrentPlayerTroopLocation, i_SelectedLocationOnBoard);
                }
                else
                {
                    throw new Exception(k_NotCorrectPlayerHasChosenMessage + $" - {CurrentPlayerTurn.Name}");
                }
            }

            CurrentPlayerTurn = OpponentPlayer;
            OpponentPlayer = lastPlayedPlayer;

            return lastPlayedPlayer;
        }

        public void MakeRandomMove()
        {
            KeyValuePair<BoardCell, BoardCell> currentAndTargetCells = r_RandomMoveGenerator.GenerateRandomMove();

            if(SecondPlayer.PlayerType == GameUtilities.ePlayersType.Computer 
                && currentAndTargetCells.Key != currentAndTargetCells.Value)
            {
                MakeAMove(currentAndTargetCells.Key, currentAndTargetCells.Value);
            }
        }

        public bool IsThereAWin()
        {
            return isFirstPlayerWinning() || isSecondPlayerWinning();
        }

        private bool isFirstPlayerWinning()
        {
            return SecondPlayer.NumberOfTroopsRemaining == 0;
        }
        private bool isSecondPlayerWinning()
        {
            return FirstPlayer.NumberOfTroopsRemaining == 0;
        }
    }
}
