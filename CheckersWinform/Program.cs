using ChceckersLogicComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckersWinform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CheckersGameLogic board = new CheckersGameLogic("Matan","Computer" ,false);
            run(board);
        }

        private static void run(CheckersGameLogic i_GameBoard)
        {
            BoardCell[] indices = null;
            Player lastPlayer = i_GameBoard.CurrentPlayerTurn;
            bool isThereAWin = false;
            printBoard(i_GameBoard);

            while (!isThereAWin)
            {
                isThereAWin = i_GameBoard.IsThereAWin();
                lastPlayer = i_GameBoard.CurrentPlayerTurn;

                if (lastPlayer.PlayerType == GameUtilities.ePlayersType.Computer)
                {
                    try
                    {
                        i_GameBoard.MakeRandomMove();
                        Console.WriteLine("Computer Move:");
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                else
                {
                    try
                    {
                        indices = askUserForAMove(i_GameBoard);
                        lastPlayer = i_GameBoard.MakeAMove(indices[0], indices[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                printBoard(i_GameBoard);
            }

            Console.WriteLine($"The Winner is {lastPlayer.Name}");
        }

        private static void printBoard(CheckersGameLogic i_GameBoardToPrint)
        {
            Console.WriteLine(columnsNumbers(i_GameBoardToPrint.BoardSize));

            if(i_GameBoardToPrint != null)
            {
                for(int i = 0; i < i_GameBoardToPrint.BoardSize; i++)
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

            for (int i = 0; i< i_GameBoardToPrint.BoardSize; i++)
            {
                slotSignToPrint = defualtSlotSign;
                currentSlotValue = i_GameBoardToPrint.GetCellValue(i_LineNumber, i);

                if(currentSlotValue != GameUtilities.ePlayerSign.empty)
                {
                    slotSignToPrint = (currentSlotValue == GameUtilities.ePlayerSign.first) ? "X  |" : "O  |";
                }

                boardLine.Append(slotSignToPrint);
            }

            return boardLine.ToString();
        }

        private static BoardCell[] askUserForAMove(CheckersGameLogic i_CheckersGame)
        {
            string userTroopAnswer = null;
            string userMoveLocation = null;

            Console.WriteLine("Pls choose a troop that will make the move");
            userTroopAnswer = Console.ReadLine();
            Console.WriteLine("Please choose a legit location");
            userMoveLocation =  Console.ReadLine();
            
            return new BoardCell[] {getUserLocation(i_CheckersGame, userTroopAnswer), getUserLocation(i_CheckersGame, userMoveLocation)};
        }


        private static BoardCell getUserLocation(CheckersGameLogic i_CheckersGame,string i_UserChoiseAsString, char i_AnswerDividerSign = ',')
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

            return new BoardCell(userChoiceAsInt, i_CheckersGame.GetCellValue(userChoiceAsInt[0], userChoiceAsInt[1]));
        }
    }
}
