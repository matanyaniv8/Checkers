using System;
using static ChceckersLogicComponents.GameUtilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class is a static class for utilities,
    /// It has some static functions for the whole logic components.
    /// </summary>
    
    public static class GameUtilities
    {
        //
        private static readonly Dictionary<string, BoardCell> r_PossibleComputerEatDirections = new Dictionary<string, BoardCell>
        {
                { "UpLeft", new BoardCell((int)eDirection.Up, (int)eDirection.Left, null, ePlayerSign.empty) },
                {"UpRight", new BoardCell((int)eDirection.Up, (int)eDirection.Right, null, ePlayerSign.empty) },
        };

        //
        private static readonly Dictionary<string, BoardCell> r_PossibleMoveDirections = new Dictionary<string, BoardCell>
        {
            {"Forward", new BoardCell((int)eDirection.Up, 0, null, ePlayerSign.empty)},
            {"Backward", new BoardCell((int)eDirection.Down, 0, null, ePlayerSign.empty)},
            {"DoubleForward", new BoardCell((int)eDirection.StartRowUp, 0, null, ePlayerSign.empty)},
            {"DoubleBackward", new BoardCell((int)eDirection.Down, 0, null , ePlayerSign.empty) }
        };
        
        private const string k_NotValidEnumType = "Not Acceptable Value Inserted!";
        public enum ePlayersType
        {
            Human = 0,
            Computer = 1
        }

        public enum ePlayerSign
        {
            empty = 0,
            first= 1,
            second = 2
        }

        public static ePlayerSign GetPlayerSign(int i_PotentialPlayerSign)
        {
            ePlayerSign playerSignConverted;
            bool isPlayerSignConverted = Enum.TryParse(i_PotentialPlayerSign.ToString(), out playerSignConverted);

            if (!isPlayerSignConverted)
            {
                throw new Exception(k_NotValidEnumType);
            }

            return playerSignConverted;
        }

        public static ePlayersType GetPlayersType(bool i_PlayerType)
        {
            ePlayersType playerTypeConverted = ePlayersType.Human;

            if(i_PlayerType == false)
            {
                playerTypeConverted = ePlayersType.Computer;
            }

            return playerTypeConverted;
        }
    }
}
