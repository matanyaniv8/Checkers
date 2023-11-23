using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChceckersLogicComponents
{
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
