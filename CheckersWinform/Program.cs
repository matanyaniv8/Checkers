using ChceckersLogicComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWinform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CheckersGameLogic board = new CheckersGameLogic("Matan");
            run(board);
            /*printBoard(board);
            board.MakeAMove(new BoardCell(5,0), new BoardCell(4,0));
            Console.WriteLine();
            printBoard(board);*/

        }

        private static void run(CheckersGameLogic i_GameBoard)
        {
            BoardCell[] indices = null;
            Player lastPlayer = null;

            while (!i_GameBoard.IsThereAWin())
            {
                printBoard(i_GameBoard);
                /*if (i_GameBoard.CurrentPlayerTurn.PlayerType = GameUtilities.ePlayersType.Computer)
                {
                    i_GameBoard.MakeAMove();
                }*/
                try
                {
                    indices = askUserForAMove();
                    Console.WriteLine(i_GameBoard.CheckersBoard.GameBoard[5, 0]);
                    lastPlayer = i_GameBoard.MakeAMove(indices[0], indices[1]);
                    Console.WriteLine(i_GameBoard.CheckersBoard.GameBoard[5, 0]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine($"The Winner is {lastPlayer.Name}");
        }

        private static void printBoard(CheckersGameLogic i_GameBoardToPrint)
        {
            Console.WriteLine(columnsNumbers(i_GameBoardToPrint.CheckersBoard.BoardSize));

            if(i_GameBoardToPrint != null)
            {
                for(int i = 0; i < i_GameBoardToPrint.CheckersBoard.BoardSize; i++)
                {
                    Console.WriteLine(getLineFromBoard(i_GameBoardToPrint, i));
                }
            }
        }
        private static string columnsNumbers(int i_BoardSize)
        {
            StringBuilder columnsNumbers = new StringBuilder();

            for (int i = 1; i <= i_BoardSize; i++)
            {
                if (i == 1)
                {
                    columnsNumbers.Append("  " + i.ToString() + "   ");
                }
                else
                {
                    columnsNumbers.Append(i.ToString() + "   ");
                }
            }

            return columnsNumbers.ToString();
        }

        private static string getLineFromBoard(CheckersGameLogic i_GameBoardToPrint, int i_LineNumber) 
        {
            StringBuilder boardLine = new StringBuilder();
            GameUtilities.ePlayerSign currentSlotValue = GameUtilities.ePlayerSign.empty;
            string defualtSlotSign = "   |";
            string slotSignToPrint = defualtSlotSign;

            boardLine.Append((i_LineNumber + 1) + "|");

            for (int i = 0; i< i_GameBoardToPrint.CheckersBoard.BoardSize; i++)
            {
                slotSignToPrint = defualtSlotSign;
                currentSlotValue = i_GameBoardToPrint.CheckersBoard.GameBoard[i_LineNumber, i];

                if(currentSlotValue != GameUtilities.ePlayerSign.empty)
                {
                    slotSignToPrint = (currentSlotValue == GameUtilities.ePlayerSign.first) ? "X  |" : "O  |";
                }

                boardLine.Append(slotSignToPrint);
            }

            return boardLine.ToString();
        }

        private static BoardCell[] askUserForAMove()
        {
            string userTroopAnswer = null;
            string userMoveLocation = null;

            Console.WriteLine("Pls choose a troop that will make the move");
            userTroopAnswer = Console.ReadLine();
            Console.WriteLine("Please choose a legit location");
            userMoveLocation =  Console.ReadLine();
            
            return new BoardCell[] {getUserLocation(userTroopAnswer), getUserLocation(userMoveLocation)};
        }


        private static BoardCell getUserLocation(string i_UserChoiseAsString, char i_AnswerDividerSign = ',')
        {
            List<int> userChoiceAsInt = new List<int>();

            foreach (string index in i_UserChoiseAsString.Split(i_AnswerDividerSign))
            {
                try
                {
                    userChoiceAsInt.Add(int.Parse(index) -1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return new BoardCell(userChoiceAsInt);
        }
    }
}
