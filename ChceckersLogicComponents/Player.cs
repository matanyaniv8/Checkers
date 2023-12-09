
namespace ChceckersLogicComponents
{
    /// <summary>
    /// This Class Repesents a Player and Hold Some properties like player's name, number of troops etc.
    /// </summary>
    public class Player
    { 
        public string Name {  get; private set; }
        public GameUtilities.ePlayerSign PlayerSign { get; set; }
        public GameUtilities.ePlayersType PlayerType { get; private set; } 
        public int NumberOfTroopsRemaining { get; set; }
        public int Score {  get; set; }

        public Player(string i_PlayerName, int i_PlayerSign, bool i_IsHumanPlayer, int i_NumberOfTroops) 
        {
            Name = i_PlayerName;
            PlayerSign = GameUtilities.GetPlayerSign(i_PlayerSign);
            PlayerType = GameUtilities.GetPlayersType(i_IsHumanPlayer);
            Score = 0;
            NumberOfTroopsRemaining = i_NumberOfTroops;
        }
    }
}
