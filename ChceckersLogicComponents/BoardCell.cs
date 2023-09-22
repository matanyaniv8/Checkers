using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChceckersLogicComponents
{
    public struct BoardCell
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public GameUtilities.ePlayerSign PlayerSign { get; set; }
        public BoardCell Defualt
        {
            get
            {
                return new BoardCell(- 1, -1);
            }
        }

        public BoardCell(int i_XAxis, int i_YAxis, GameUtilities.ePlayerSign i_PlayerSign = GameUtilities.ePlayerSign.empty)
        {
            X = i_XAxis;
            Y = i_YAxis;
            PlayerSign = i_PlayerSign;
        }

        public static bool operator ==(BoardCell lhs, BoardCell rhs)
        {
            return (lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.PlayerSign == rhs.PlayerSign);
        }
        public static bool operator !=(BoardCell lhs, BoardCell rhs)
        {
            return !(lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.PlayerSign == rhs.PlayerSign);
        }

        public override bool Equals(object obj)
        {
            bool areEquals = false;
            BoardCell otherCell = new BoardCell().Defualt;

            if (obj != null || GetType() == obj.GetType())
            {
                areEquals = ((BoardCell)(obj) == this);
            }

            return areEquals;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ PlayerSign.GetHashCode();
        }
    }
}
