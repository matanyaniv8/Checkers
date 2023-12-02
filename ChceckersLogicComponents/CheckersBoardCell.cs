using PlayerSign = ChceckersLogicComponents.GameUtilities.ePlayerSign;

namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class Represents a cell in the Checkers Board.
    /// </summary>
    
    public class CheckersBoardCell
    {
        public bool IsCellPlayerAKing {  get; set; }
        public PlayerSign CellSign { get; set; }

        public CheckersBoardCell(PlayerSign i_CellPlayerSign = PlayerSign.empty, bool i_IsCellPlayerAKing = false)
        {
            CellSign = i_CellPlayerSign;
            IsCellPlayerAKing = i_IsCellPlayerAKing;
        }

        public override bool Equals(object obj)
        {
            bool areEquals = false;
            CheckersBoardCell cell = obj as CheckersBoardCell;

            if (cell != null)
            {
                areEquals = IsCellPlayerAKing == cell.IsCellPlayerAKing && CellSign == cell.CellSign;
            }

            return areEquals;
        }

        public override int GetHashCode()
        {
            return CellSign.GetHashCode() ^ CellSign.GetHashCode();
        }
    }
}
