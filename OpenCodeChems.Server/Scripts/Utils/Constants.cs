namespace OpenCodeChems.Server.Standar
{
    /// <summary>
    /// Constants of the server project
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Default rol of a player
        /// </summary>
        public const int NULL_ROL = -1;
        /// <summary>
        /// Name of the red spy master rol
        /// </summary>
        public const string RED_SPY_MASTER = "RedSpyMaster";
        /// <summary>
        /// Name of the blue spy master rol
        /// </summary>
        public const string BLUE_SPY_MASTER = "BlueSpyMaster";
        /// <summary>
        /// Name of the red spy rol
        /// </summary>
        public const string RED_PLAYER = "RedPlayer";
        /// <summary>
        /// Name of the blue spy rol
        /// </summary>
        public const string BLUE_PLAYER = "BluePlayer";
        /// <summary>
        /// Array with the roles of the game
        /// </summary>
        /// <value>string with name of each rol</value>
        public readonly static string [] ROLES = {RED_SPY_MASTER, RED_PLAYER, BLUE_SPY_MASTER, BLUE_PLAYER};
        /// <summary>
        /// Maximum number of players who can be in the red team
        /// </summary>
        public const int MAX_RED_PLAYERS = 3;
        /// <summary>
        /// Maximum number of players who can be in the blue team
        /// </summary>
        public const int MAX_BLUE_PLAYERS = 3;
        /// <summary>
        /// Maxium number of players who can join a room
        /// </summary>
        public const int MAX_MEMBERS = 8;
        /// <summary>
        /// Code of the blue card
        /// </summary>
        public const int BLUE = 0;
        /// <summary>
        /// Code of the red card
        /// </summary>
        public const int RED = 1;
        /// <summary>
        /// Code of the civil card
        /// </summary>
        public const int YELLOW = 2;
        /// <summary>
        /// Code of the assassin card
        /// </summary>
        public const int BLACK = 3;
        /// <summary>
        /// Allows to check if the player hit the clue or not for the first blue team master key
        /// </summary>
        public int[] KEY_BLUE_ONE = new int[] {0,2,2,2,0,1,0,0,0,1,0,2,1,0,3,2,1,1,2,1,1,0,2,1,0}; //0
        /// <summary>
        /// Allows to check if the player hit the clue or not for the first red team master key
        /// </summary>
        public int[] KEY_RED_ONE = new int[] {1,2,2,2,0,1,0,0,0,1,0,2,1,0,3,2,1,1,2,1,1,0,2,1,0}; //1
        /// <summary>
        /// Allows to check if the player hit the clue or not for the second red team master key
        /// </summary>
        public int[] KEY_RED_TWO = new int[] {0,2,2,3,0,1,0,0,1,1,0,2,1,0,2,2,1,1,2,1,1,0,2,1,0}; //2
        /// <summary>
        /// Allows to check if the player hit the clue or not for the second blue team master key
        /// </summary>
        public int[] KEY_BLUE_TWO = new int[] {3,2,2,0,0,1,0,0,0,1,0,2,1,0,2,2,1,1,2,1,1,0,2,1,0}; //3
    }
    
}