using System;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class is a static class for utilities,
    /// It has some static functions for the whole logic components.
    /// </summary>
    
    public static class GameUtilities
    {
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
