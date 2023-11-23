
using System.Collections.Generic;

namespace ChceckersLogicComponents
{
    public struct BoardCell
    {
        private const int k_DefualtAxisPoint = -1;
        public int X {  get; set; }
        public int Y { get; set; }
        public GameUtilities.ePlayerSign PlayerSign { get; set; }
        public BoardCell DefualtCell
        {
            get
            {
                return new BoardCell(k_DefualtAxisPoint, k_DefualtAxisPoint);
            }
        }

        public BoardCell(int i_XAxis=0, int i_YAxis=0, GameUtilities.ePlayerSign i_PlayerSign=GameUtilities.ePlayerSign.empty)
        {
            X = i_XAxis;
            Y = i_YAxis;
            PlayerSign = i_PlayerSign;
        }

        public BoardCell(List<int> i_Axes, GameUtilities.ePlayerSign i_PlayerSign = GameUtilities.ePlayerSign.empty)
        {
            if( i_Axes.Count >= 2)
            {
                X = i_Axes[0];
                Y = i_Axes[1];
                PlayerSign = i_PlayerSign;
            }
            else
            {
                throw new System.Exception("Not enough parameters");
            }
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
            BoardCell otherCell = new BoardCell().DefualtCell;

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

        public override string ToString()
        {
            return ($"X: {this.X}, Y: {this.Y}, Sign: {this.PlayerSign}");
        }
    }
}
