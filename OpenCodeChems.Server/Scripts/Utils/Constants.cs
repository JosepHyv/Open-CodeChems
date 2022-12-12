namespace OpenCodeChems.Server.Standar
{
    public class Constants
    {
        public static string RED_SPY_MASTER = "RedSpyMaster";

        public static string BLUE_SPY_MASTER = "BlueSpyMaster";

        public static string RED_PLAYER = "RedPlayer";

        public static string BLUE_PLAYER = "BluePlayer";

        public static string [] ROLES = {RED_SPY_MASTER, RED_PLAYER, BLUE_SPY_MASTER, BLUE_PLAYER};
        public static int MAX_RED_PLAYERS = 3;

        public static int MAX_BLUE_PLAYERS = 3;

        public static int MAX_MEMBERS = 8;

        public static int BLUE = 0;

        public static int RED = 1;

        public static int YELLOW = 2;

        public static int BLACK = 3;
        
        public int[] KeyBlueOne = new int[] {0,2,2,2,0,1,0,0,0,1,0,2,1,0,3,2,1,1,2,1,1,0,2,1,0};
        public int[] KeyBlueTwo = new int[] {3,2,2,0,0,1,0,0,0,1,0,2,1,0,2,2,1,1,2,1,1,0,2,1,0};
        public int[] KeyRedOne = new int[] {1,2,2,2,0,1,0,0,0,1,0,2,1,0,3,2,1,1,2,1,1,0,2,1,0};
        public int[] KeyRedTwo = new int[] {0,2,2,3,0,1,0,0,1,1,0,2,1,0,2,2,1,1,2,1,1,0,2,1,0};
    }
    
}