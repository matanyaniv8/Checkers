using System;
using System.Collections.Generic;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class is the facade of all the Checkers Game components.
    /// It Hides all the actual doers, so that the users won't knows how the game logic is built.
    /// </summary>
    
    public class CheckersGameLogic
    {
        private RandomMoveGenerator r_RandomMoveGenerator = null;
        private const int k_NumberOfTroops = 12;
        private const string k_NotCorrectPlayerHasChosenMessage = "Please choose a soldier from your troops";
        public Player FirstPlayer { get; private set; }
        public Player SecondPlayer { get; private set;}
        internal Board CheckersBoard { get; private set; }
        public Player CurrentPlayerTurn { get; private set; }
        public Player OpponentPlayer { get; private set; }
        public int BoardSize { get; private set; }

        public CheckersGameLogic(string i_FirstPlayerName, string i_SecondPlayerName="Computer", bool i_IsSecondPlayerHuman=false)
        {
            CheckersBoard = new Board(i_IsSecondPlayerHuman);
            BoardSize = CheckersBoard.BoardSize;
            FirstPlayer = new Player(i_FirstPlayerName,1 ,true, k_NumberOfTroops);
            FirstPlayer.PlayerSign = GameUtilities.ePlayerSign.first;
            SecondPlayer = new Player(i_SecondPlayerName, 2, i_IsSecondPlayerHuman, k_NumberOfTroops);
            SecondPlayer.PlayerSign = GameUtilities.ePlayerSign.second;
            CurrentPlayerTurn = FirstPlayer;
            OpponentPlayer = SecondPlayer;
            CheckersBoard.InitializeBoard(FirstPlayer.PlayerSign, SecondPlayer.PlayerSign) ;

            if(!i_IsSecondPlayerHuman)
            {
                r_RandomMoveGenerator = new RandomMoveGenerator(SecondPlayer, CheckersBoard);
            }
        }

        public Player MakeAMove(BoardCell i_CurrentPlayerTroopLocation, BoardCell i_SelectedLocationOnBoard )
        {
            Player lastPlayedPlayer = CurrentPlayerTurn;

            if (!IsThereAWin())
            {
                if (CurrentPlayerTurn.PlayerSign == CheckersBoard.GetPlayerCellFromBoard(i_CurrentPlayerTroopLocation).CellSign)
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

/*            if(CurrentPlayerTurn.PlayerType == GameUtilities.ePlayersType.Computer)
            {
                MakeRandomMove();
                CurrentPlayerTurn = FirstPlayer;
            }*/

            return lastPlayedPlayer;
        }

        public void MakeRandomMove()
        {
            KeyValuePair<BoardCell, BoardCell> currentAndTargetCells = r_RandomMoveGenerator.GenerateRandomMove();

            if(SecondPlayer.PlayerType == GameUtilities.ePlayersType.Computer 
                && currentAndTargetCells.Key != currentAndTargetCells.Value)
            {
                MakeAMove(currentAndTargetCells.Key, currentAndTargetCells.Value);
                Console.WriteLine(currentAndTargetCells.Key.ToString());
                Console.WriteLine(currentAndTargetCells.Value.ToString());
            }
        }

        public CheckersBoardCell GetCellValue(int i_RowIndex, int i_ColIndex) => CheckersBoard.GetPlayerCellFromBoard(new BoardCell(i_RowIndex, i_ColIndex));

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
