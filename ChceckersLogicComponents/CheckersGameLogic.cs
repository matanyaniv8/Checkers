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
        private readonly BoardCell r_DefualtPoint = new BoardCell().Defualt;
        private const int k_NumberOfTroops = 12;
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set;}
        public Board CheckersBoard { get; private set; }
        public Player CurrentPlayerTurn { get; private set; }
        public Player OpponentPlayer { get; private set; }

        public CheckersGameLogic(string i_FirstPlayerName, string i_SecondPlayerName, bool i_IsSecondPlayerComputer)
        {
            CheckersBoard = new Board(i_IsSecondPlayerComputer);
            FirstPlayer = new Player(i_FirstPlayerName,1 ,true, k_NumberOfTroops);
            SecondPlayer = new Player(i_SecondPlayerName, 2, i_IsSecondPlayerComputer, k_NumberOfTroops);
            CurrentPlayerTurn = FirstPlayer;
            OpponentPlayer = SecondPlayer;
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
