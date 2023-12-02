using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;
using System.Collections.Generic;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Structs is aimed to move some Coords, PlayerCell properties for the CheckersBoard.
    /// It used as a drawer for some useful aruments that the user required to insert as an argument.
    /// </summary>
    
    public struct BoardCell
    {
        private const int k_DefualtAxisPoint = -1;
        public int X { get; set; }
        public int Y { get; set; }
        public CheckersBoardCell CheckersReleventInfo { get; set; }
        //public PlayerSign PlayerSign { get; set; }
        public BoardCell DefualtCell
        {
            get
            {
                return new BoardCell(k_DefualtAxisPoint, k_DefualtAxisPoint);
            }
        }

        public BoardCell(int i_XAxis = 0, int i_YAxis = 0,CheckersBoardCell i_PlayerCell = null ,PlayerSign i_PlayerSign = PlayerSign.empty)
        {
            X = i_XAxis;
            Y = i_YAxis;
            CheckersReleventInfo = i_PlayerCell;

            if(i_PlayerCell != null)
            {
                if (i_PlayerCell.IsCellPlayerAKing || i_PlayerCell.CellSign != PlayerSign.empty)
                {
                    CheckersReleventInfo = i_PlayerCell;
                }
            }
            else
            {
                CheckersReleventInfo = new CheckersBoardCell(i_PlayerSign, false);
            }
        }

        public BoardCell(List<int> i_Axes, CheckersBoardCell i_PlayerCell = null)
        {
            if( i_Axes.Count >= 2)
            {
                X = i_Axes[0];
                Y = i_Axes[1];
                CheckersReleventInfo = (i_PlayerCell != null) ? i_PlayerCell: new CheckersBoardCell();
            }
            else
            {
                throw new System.Exception("Not enough parameters");
            }
        }

        public static bool operator ==(BoardCell lhs, BoardCell rhs)
        {
            return (lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.CheckersReleventInfo.Equals(rhs.CheckersReleventInfo));
        }
        public static bool operator !=(BoardCell lhs, BoardCell rhs)
        {
            return !(lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.CheckersReleventInfo.Equals(rhs.CheckersReleventInfo));
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
            return X.GetHashCode() ^ Y.GetHashCode() ^ CheckersReleventInfo.CellSign.GetHashCode();
        }

        public override string ToString()
        {
            return ($"X: {this.X}, Y: {this.Y}, Sign: {this.CheckersReleventInfo.CellSign}");
        }
    }
}
