using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChceckersLogicComponents
{
    public class CheckersGameLogic
    {
        private readonly BoardCell r_DefualtPoint = new BoardCell().DefualtCell;
        private const int k_NumberOfTroops = 12;
        private const int k_NumberOfRowsToPutTroopsInBoard = 3;
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set;}
        public Board CheckersBoard { get; private set; }
        public Player CurrentPlayerTurn { get; private set; }
        public Player OpponentPlayer { get; private set; }

        public CheckersGameLogic(string i_FirstPlayerName, string i_SecondPlayerName="Computer", bool i_IsSecondPlayerComputer=true)
        {
            CheckersBoard = new Board(i_IsSecondPlayerComputer);
            FirstPlayer = new Player(i_FirstPlayerName,1 ,true, k_NumberOfTroops);
            FirstPlayer.PlayerSign = GameUtilities.ePlayerSign.first;
            SecondPlayer = new Player(i_SecondPlayerName, 2, i_IsSecondPlayerComputer, k_NumberOfTroops);
            SecondPlayer.PlayerSign = GameUtilities.ePlayerSign.second;
            CurrentPlayerTurn = FirstPlayer;
            OpponentPlayer = SecondPlayer;
            initializeBoard();
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
                CheckersBoard.MakeAMove(CurrentPlayerTurn, OpponentPlayer, i_CurrentPlayerTroopLocation, i_SelectedLocationOnBoard);   
            }

            CurrentPlayerTurn = OpponentPlayer;
            OpponentPlayer = lastPlayedPlayer;

            return lastPlayedPlayer;
        }

        public void MakeRandomMove()
        {
            ///TODO: TO complete using machine learning 
            ///like a tree of all possible moves 
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
