namespace OpenCodeChems.Server.Standar
{
    public class Constants
    {

        public const string RED_WON = "RED_TEAM";

        public const string BLUE_WON = "BLUE_TEAM";
        public const string EMPTY_ROL = "None";

        public const string TEAM_WON = "None";
        public const int NULL_ROL = -1;

        public const int EMPTY_COUNTER = 0 ;
        public const string RED_SPY_MASTER = "RedSpyMaster";

        public const string BLUE_SPY_MASTER = "BlueSpyMaster";

        public const string RED_PLAYER = "RedPlayer";

        public const string BLUE_PLAYER = "BluePlayer";

        public static string [] ROLES = {RED_SPY_MASTER, RED_PLAYER, BLUE_SPY_MASTER, BLUE_PLAYER};
        public const int MAX_RED_PLAYERS = 3;

        public const int MAX_BLUE_PLAYERS = 3;

        public const int MAX_MEMBERS = 8;

        public const int BLUE = 0;

        public const int RED = 1;

        public const int YELLOW = 2;

        public const int BLACK = 3;
        //SCENENUMBER
        //0
        public static int[] KeyBlueOne = new int[] {0,2,2,2,0,1,0,0,0,1,0,2,1,0,3,2,1,1,2,1,1,0,2,1,0}; 
        //1
        public static int[] KeyRedOne = new int[] {1,2,2,2,0,1,0,0,0,1,0,2,1,0,3,2,1,1,2,1,1,0,2,1,0}; 
        //2
        public static int[] KeyRedTwo = new int[] {0,2,2,3,0,1,0,0,1,1,0,2,1,0,2,2,1,1,2,1,1,0,2,1,0}; 
        //3
        public static int[] KeyBlueTwo = new int[] {3,2,2,0,0,1,0,0,0,1,0,2,1,0,2,2,1,1,2,1,1,0,2,1,0}; 
    }
    
}